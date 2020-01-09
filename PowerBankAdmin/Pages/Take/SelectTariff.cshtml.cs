using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;

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

        public SelectTariffModel(IHolderService holderService, AppRepository appRepository)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }

        public async Task OnGetAsync()
        {
            IdentifyCostumer();

            if (IsAuthorized())
            {
                //Session = await _holderService.LastSession(Costumer.Id);
                //if (Session != null && Session.IsActive)
                //{
                //    SessionDuration = (DateTime.Now - Session.Start).TotalSeconds;
                //}


            }

            ViewData["HideFooter"] = true;
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
    }
}