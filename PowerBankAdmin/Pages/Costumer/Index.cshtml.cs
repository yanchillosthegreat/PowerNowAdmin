

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

            var _httpClient = new WebClient();
            var urlString = $"https://3dsec.sberbank.ru/payment/rest/getBindings.do?userName=power-now-api&password=power-now&clientId=778";
            var responseText = _httpClient.DownloadString(new Uri(urlString));

            JsonSerializer serializer = new JsonSerializer();
            GetBindingsResponse response = JsonConvert.DeserializeObject<GetBindingsResponse>(responseText);

            var binding = response.Bindings.FirstOrDefault();
            if (binding != null)
            {
                var costumerToEdit = _appRepository.Costumers.FirstOrDefault(x => x.Id == Costumer.Id);
                costumerToEdit.BindId = binding.BindingId;
                costumerToEdit.Card = binding.MaskedPan;
                _appRepository.SaveChanges();
                Costumer = costumerToEdit;
            }
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
            var _httpClient = new WebClient();
            var random = new Random();
            var orderNumber = random.Next(15000, 16000);
            var urlString = $"https://3dsec.sberbank.ru/payment/rest/register.do?userName=power-now-api&clientId=777&password=power-now&orderNumber={orderNumber}&amount=1&returnUrl=http://power-now.ru/api/api";
            var responseText = await _httpClient.DownloadStringTaskAsync(new Uri(urlString));

            JsonSerializer serializer = new JsonSerializer();
            RegisterDoResponse response = JsonConvert.DeserializeObject<RegisterDoResponse>(responseText);

            return Redirect(response.FormUrl);
        }

        public IActionResult Test()
        {
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
    }
}
