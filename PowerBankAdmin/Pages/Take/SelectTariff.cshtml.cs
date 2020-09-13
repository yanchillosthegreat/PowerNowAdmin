using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;
using Yandex.Checkout.V3;

namespace PowerBankAdmin.Pages.Take
{
    public class SelectTariffModel : BaseAuthCostumerPage
    {
        private readonly AppRepository _appRepository;
        private readonly IHolderService _holderService;

        [BindProperty]
        public PowerbankSessionModel Session { get; set; }
        [BindProperty]
        public double SessionDuration { get; set; }
        [BindProperty]
        public int? HolderId { get; set; }
        [BindProperty]
        public HolderModel Holder { get; set; }

        public SelectTariffModel(IHolderService holderService, AppRepository appRepository)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }

        public async Task OnGetAsync(int? holderId)
        {
            HolderId = holderId;
            Holder = await _appRepository.Holders.Include(x => x.HolderRentModels).ThenInclude(x => x.RentModel).FirstOrDefaultAsync(x => x.Id == holderId);
            IdentifyCostumer();
            if (IsAuthorized())
            {
                Session = await _holderService.LastSession(Costumer.Id);
                if (Session != null && Session.IsActive)
                {
                    ViewData["SessionDuration"] = (DateTime.Now - Session.Start).TotalSeconds;
                }
            }
            ViewData["HideFooter"] = true;
        }

        public async Task<IActionResult> OnPostTariffAsync(int? holderId, string tariff, string card)
        {
            IdentifyCostumer();
            if(!IsAuthorized()) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Not Authorized");
            if(holderId == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Wrong Data");

            var _holder = await _appRepository.Holders.Include(x => x.HolderRentModels).ThenInclude(x => x.RentModel).FirstOrDefaultAsync(x => x.Id == holderId);

            RentModel rentModel = null;
            switch (tariff)
            {
                case "hour":
                    rentModel = _holder.HolderRentModels.FirstOrDefault(x => x.RentModel.RentStrategy == RentStrategy.Hour).RentModel;
                    break;
                case "day":
                    rentModel = _holder.HolderRentModels.FirstOrDefault(x => x.RentModel.RentStrategy == RentStrategy.Day).RentModel;
                    break;
                case "firstHourFree":
                    rentModel = _holder.HolderRentModels.FirstOrDefault(x => x.RentModel.RentStrategy == RentStrategy.FirstHourFree).RentModel;
                    break;
                default:
                    break;
            }

            if (rentModel == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "No such rent model");

            var result = await _holderService.ProvidePowerBank(Costumer.Id, holderId ?? 0, rentModel, card);
            
            if(!result) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpServerErrorCode, "Can't provide powerbank");
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
        public async Task<IActionResult> OnPostCheckAsync()
        {
            IdentifyCostumer();
            if (!IsAuthorized())
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Not Authorized");
            var session = await _appRepository.PowerbankSessions.FirstOrDefaultAsync(x => x.Costumer.Id == Costumer.Id && x.IsActive);
            var message = session == null ? "0" : "1"; // 1 - session is Active
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode, message);
        }

        public async Task<IActionResult> OnPostAddCardAsync(int? holderId)
        {
            base.IdentifyCostumer();

            var client = new Yandex.Checkout.V3.Client(shopId: Strings.YandexShopId, secretKey: Strings.YandexAPIKey);

            var newPayment = new NewPayment
            {
                Amount = new Amount { Value = 1.00m, Currency = "RUB" },
                SavePaymentMethod = true,
                Capture = true,
                Confirmation = new Confirmation
                {
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = $"https://power-now.ru/take/selecttariff/{holderId}"
                },
                PaymentMethodData = new PaymentMethod
                {
                    Type = PaymentMethodType.BankCard
                },
                Description = "Для привязки карты списываем сумму в 1 рубль, которую сразу же вернем на Вашу карту"
            };

            Payment payment = client.CreatePayment(newPayment, Costumer.Id.ToString());
            string url = payment.Confirmation.ConfirmationUrl;

            await Costumer.SetOrderId(_appRepository, payment.Id);
            await Costumer.SetCardStatus(_appRepository, CardsStatus.Progress);

            return Redirect(url);
        }
    }



    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BaseRequest
    {
        [JsonProperty(PropertyName = "equipmentSn")]
        public string EquipmentSn { get; set; }

        [JsonProperty(PropertyName = "deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty(PropertyName = "deviceVersion")]
        public string DeviceVersion { get; set; }

        [JsonProperty(PropertyName = "sessionId")]
        public string SessionId { get; set; }

        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }

        [JsonProperty(PropertyName = "packageType")]
        public string PackageType { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BorrowRequest : BaseRequest
    {
        [JsonProperty(PropertyName = "position")]
        public string Position { get; set; }

        [JsonProperty(PropertyName = "powerBankSn")]
        public string PowerBankSn { get; set; }
    }

    public class FooRequest
    {
        [JsonProperty(PropertyName = "ciphertext")]
        public string CipherText { get; set; }

        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }
    }
}