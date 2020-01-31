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

        public async Task<IActionResult> OnPutAsync()
        {
            var costumerToEdit = await _appRepository.Costumers.FirstOrDefaultAsync(x => x.Id == Costumer.Id);
            if (costumerToEdit == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);
            _appRepository.Entry(Costumer).Property(x => x.Name).IsModified = true;
            _appRepository.Entry(Costumer).Property(x => x.Email).IsModified = true;
            await _appRepository.SaveChangesAsync();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        public async Task<IActionResult> OnPostAddCardAsync()
        {
            base.IdentifyCostumer();
            //var _httpClient = new WebClient();
            //var random = new Random();
            //var orderNumber = random.Next(15000, 16000);
            //var urlString = $"https://3dsec.sberbank.ru/payment/rest/register.do?userName=power-now-api&clientId={Costumer.Id}&password=power-now&orderNumber={orderNumber}&amount=1&returnUrl=https://power-now.ru/acquiring/{Strings.GoToClientPage}";
            //var responseText = await _httpClient.DownloadStringTaskAsync(new Uri(urlString));

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
