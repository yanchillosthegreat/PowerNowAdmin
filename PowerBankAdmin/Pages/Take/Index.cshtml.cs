using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Services;

namespace PowerBankAdmin.Pages.Take
{
    public class IndexModel : BaseAuthCostumerPage
    {
        private readonly AppRepository _appRepository;
        private readonly HolderService _holderService;

        [BindProperty]
        public double SessionDuration { get; set; }


        public IndexModel(AppRepository appRepository, HolderService holderService)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }


        public async Task OnGetAsync()
        {
            IdentifyCostumer();
            if (IsAuthorized())
            {
                var session = await _holderService.LastSession(Costumer.Id);
                if(session != null && session.IsActive)
                {
                    SessionDuration = (DateTime.Now - session.Start).TotalSeconds;
                }
            }
        }

        public async Task<IActionResult> OnPostTakeAsync(string code)
        {
            IdentifyCostumer();
            if (!IsAuthorized())
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Not Authorized");
            if (string.IsNullOrEmpty(code))
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Wrong code");
            var holder = _appRepository.Holders.FirstOrDefaultAsync(x => x.LocalCode == code);
            if(holder == null)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "No such Holder");

            var result = await _holderService.ProvidePowerBank(Costumer.Id, holder.Id);
            if(!result)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Couldn't provide powerbank");

            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        public async Task<IActionResult> OnPostCheckAsync()
        {
            IdentifyCostumer();
            if(!IsAuthorized())
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Not Authorized");
            var session = await _appRepository.PowerbankSessions.FirstOrDefaultAsync(x => x.Costumer.Id == Costumer.Id && x.IsActive);
            var message = session == null ? "0" : "1"; // 1 - session is Active
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode, message);
        }
    }
}
