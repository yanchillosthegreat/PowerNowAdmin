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

    public class GetBindingsResponse
    {
        public string ErrorCode { get; set; }
        public string ErorMessage { get; set; }
        public List<SberbankBinding> Bindings { get; set; }
    }

    public class SberbankBinding
    {
        public string BindingId { get; set; }
        public string MaskedPan { get; set; }
        public string ExpiryDate { get; set; }
    }

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


        public IndexModel(IHolderService holderService, AppRepository appRepository)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }


        public async Task OnGetAsync()
        {
            IdentifyCostumer();

            ViewData["City"] = "/css/patterns/city_3.png";

            if (IsAuthorized())
            {
                Session = await _holderService.LastSession(Costumer.Id);
                // REDIRECT
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostCheckEquipmentAsync(string c1, string c2, string c3, string c4)
        {
            var code = string.Format("{0}{1}{2}{3}", c1, c2, c3, c4);

            IdentifyCostumer();
            if (!IsAuthorized())
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Not Authorized");
            if (string.IsNullOrEmpty(code))
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.InvalidCode, "Wrong code");
            var holder = await _appRepository.Holders.FirstOrDefaultAsync(x => x.LocalCode == code);
            if (holder == null)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.NoSuchHolder, "No such Holder");

            var result = await _holderService.CanProvidePowerBank(holder.Id);
            if (!result)
            {
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.NoAvailablePowebank, "Couldn't provide powerbank");
            }

            //return Redirect($"/Take/SelectTariff/{holder.Id}");

            return JsonHelper.JsonResponse(Strings.StatusOK, 4, holder.Id.ToString());
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

        //public async Task<IActionResult> OnPostCheckEquipmentAsync()
        //{
        //    IdentifyCostumer();
        //    if (!IsAuthorized())
        //        return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Not Authorized");
        //    var session = await _appRepository.PowerbankSessions.FirstOrDefaultAsync(x => x.Costumer.Id == Costumer.Id && x.IsActive);
        //    var message = session == null ? "0" : "1"; // 1 - session is Active
        //    return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode, message);
        //}
    }
}
