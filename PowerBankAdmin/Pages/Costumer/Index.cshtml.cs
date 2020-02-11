using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;
using PowerBankAdmin.Pages.Take;
using Yandex.Checkout.V3;

namespace PowerBankAdmin.Pages.Costumer
{
    public class IndexModel : BaseAuthCostumerPage
    {
        public IEnumerable<PowerbankSessionModel> Sessions { get; set; }

        private readonly AppRepository _appRepository;
        public IndexModel(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public void OnGet()
        {
            IdentifyCostumer();
            ViewData["Title"] = "ПРОФИЛЬ";
            ViewData["HideFooter"] = true;
            Sessions = _appRepository.PowerbankSessions.Include(x => x.Powerbank).ThenInclude(x => x.Holder).Where(x => x.Costumer.Id == Costumer.Id);
            
            
            
            /*
            var _httpClient = new WebClient();
            var urlString = $"https://3dsec.sberbank.ru/payment/rest/getBindings.do?userName=power-now-api&password=power-now&clientId=778";
            var responseText = _httpClient.DownloadString(new Uri(urlString));

            JsonSerializer serializer = new JsonSerializer();
            GetBindingsResponse response = JsonConvert.DeserializeObject<GetBindingsResponse>(responseText);

            var binding = response.Bindings.FirstOrDefault();
            if (binding != null)
            {
                var costumerToEdit = _appRepository.Costumers.FirstOrDefault(x => x.Id == Costumer.Id);
                //costumerToEdit.BindId = binding.BindingId;
                costumerToEdit.Card = binding.MaskedPan;
                _appRepository.SaveChanges();
                Costumer = costumerToEdit;
            }*/
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var costumerToEdit = await _appRepository.Costumers.FirstOrDefaultAsync(x => x.Id == Costumer.Id);
            if (costumerToEdit == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);
            _appRepository.Entry(Costumer).Property(x => x.Name).IsModified = true;
            _appRepository.Entry(Costumer).Property(x => x.Email).IsModified = true;
            await _appRepository.SaveChangesAsync();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        public async Task<IActionResult> OnPostDeleteCardAsync(string bindingId)
        {
            base.IdentifyCostumer();

            var costumer = await _appRepository.Costumers.FirstOrDefaultAsync(x => x.Id == Costumer.Id);
            var card = costumer.CardBindings.FirstOrDefault(x => x.BindingId == bindingId);

            if (card.IsLocked) return Redirect("Costumer");

            _appRepository.Attach(card);
            _appRepository.Remove(card);
            await _appRepository.SaveChangesAsync();

            return Redirect("Costumer");
        }

        public async Task<IActionResult> OnPostAddCardAsync()
        {
            base.IdentifyCostumer();
            var client = new Yandex.Checkout.V3.Client(shopId: Strings.YandexShopId, secretKey: Strings.YandexAPIKey);

            var newPayment = new NewPayment
            {
                Amount = new Amount { Value = 100.00m, Currency = "RUB" },
                SavePaymentMethod = true,
                Confirmation = new Confirmation
                {
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = $"https://power-now.ru/costumer"
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

        public IActionResult Test()
        {
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
    }
}
