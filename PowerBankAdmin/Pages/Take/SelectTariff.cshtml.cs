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

        public SelectTariffModel(IHolderService holderService, AppRepository appRepository)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }

        public async Task OnGetAsync(int? holderId)
        {
            HolderId = holderId;
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
            var result = await _holderService.ProvidePowerBank(Costumer.Id, holderId ?? 0, tariff, card);
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

        public void TakePowerbank()
        {
            var borrowRequest = new BorrowRequest
            {
                EquipmentSn = "1800008823",
                PackageType = "171",
                Position = "1",
                DeviceType = "8",
                DeviceVersion = "1",
                SessionId = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()
            };

            WebRequest request = WebRequest.Create("https://dry-wildwood-23355.herokuapp.com/operation");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string body = JsonConvert.SerializeObject(borrowRequest);
                streamWriter.Write(body);
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        public async Task<IActionResult> OnPostAddCardAsync()
        {
            base.IdentifyCostumer();
            //var _httpClient = new WebClient();
            //var random = new Random();
            //var orderNumber = random.Next(15000, 16000);
            //var urlString = $"https://3dsec.sberbank.ru/payment/rest/register.do?userName=power-now-api&clientId={Costumer.Id}&password=power-now&orderNumber={orderNumber}&amount=1&returnUrl=https://power-now.ru/acquiring/{Strings.GoToTakePage}"; 
            // var responseText = await _httpClient.DownloadStringTaskAsync(new Uri(urlString));

            //JsonSerializer serializer = new JsonSerializer();
            //RegisterDoResponse response = JsonConvert.DeserializeObject<RegisterDoResponse>(responseText);
            //await Costumer.SetOrderId(_appRepository, response.OrderId);
            //await Costumer.SetCardStatus(_appRepository, CardsStatus.Progress);
            //return Redirect(response.FormUrl);

            var client = new Yandex.Checkout.V3.Client(
    shopId: "667169",
    secretKey: "test_yaa_BuTea1360q-9lXQVQRzdqSiThR_2b_6U_P2wXas");

            var newPayment = new NewPayment
            {
                Amount = new Amount { Value = 1.00m, Currency = "RUB" },
                SavePaymentMethod = true,
                Confirmation = new Confirmation
                {
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = $"https://power-now.ru/take/selecttariff"
                },
                PaymentMethodData = new PaymentMethod
                {
                    Type = PaymentMethodType.BankCard
                },
                Description = "Test #1"
            };

            Payment payment = client.CreatePayment(newPayment);
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