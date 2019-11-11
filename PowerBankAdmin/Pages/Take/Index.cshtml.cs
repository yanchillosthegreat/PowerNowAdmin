using System;
using System.Collections.Generic;
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

namespace PowerBankAdmin.Pages.Take
{
    public class RegisterDoResponse
    {
        public string OrderId { get; set; }
        public string FormUrl { get; set; }
    }

    public class IndexModel : BaseAuthCostumerPage
    {
        private readonly AppRepository _appRepository;
        private readonly IHolderService _holderService;

        [BindProperty]
        public PowerbankSessionModel Session { get; set; }
        [BindProperty]
        public double SessionDuration { get; set; }


        public IndexModel(IHolderService holderService, AppRepository appRepository)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }


        public async Task OnGetAsync()
        {
            IdentifyCostumer();
            if (IsAuthorized())
            {
                Session = await _holderService.LastSession(Costumer.Id);
                if (Session != null && Session.IsActive)
                {
                    SessionDuration = (DateTime.Now - Session.Start).TotalSeconds;
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPost(string c1, string c2, string c3, string c4)
        {
            var code = string.Format("{0}_{1}_{2}_{3}", c1, c2, c3, c4);

            IdentifyCostumer();
            if (!IsAuthorized())
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Not Authorized");
            if (string.IsNullOrEmpty(code))
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Wrong code");
            var holder = await _appRepository.Holders.FirstOrDefaultAsync(x => x.LocalCode == code);
            if (holder == null)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "No such Holder");

            var result = await _holderService.ProvidePowerBank(Costumer.Id, holder.Id);
            if (!result)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Couldn't provide powerbank");

            var _httpClient = new WebClient();
            var random = new Random();
            var orderNumber = random.Next(15000, 16000);
            var urlString = $"https://3dsec.sberbank.ru/payment/rest/register.do?userName=power-now-api&password=power-now&orderNumber={orderNumber}&amount=100&returnUrl=http://power-now.ru/take";
            var responseText = _httpClient.DownloadString(new Uri(urlString));

            JsonSerializer serializer = new JsonSerializer();
            RegisterDoResponse response = JsonConvert.DeserializeObject<RegisterDoResponse>(responseText);

            return Redirect(response.FormUrl);

            //return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
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
    }
}
