using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Admin.Powerbanks
{
    public class IndexModel : BaseAuthedPageModel
    {
        [BindProperty]
        public IEnumerable<PowerbankModel> Powerbanks { get; set; }

        private AppRepository _appRepository;
        private IHolderService _holderService;

        public IndexModel(IHolderService holderService, AppRepository appRepository)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }

        public async Task OnGetAsync()
        {
            IdentifyUser();
            Powerbanks = await _appRepository.Powerbanks.Include(x => x.Sessions).ThenInclude(x => x.Costumer).ToListAsync();
        }

        public async Task<IActionResult> OnPostStopAsync(int? id)
        {
            if(id == null || id < 1)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Wrong id");
            var result = await _holderService.ReleasePowerBank(id ?? 0);
            if(!result)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpServerErrorCode, "Cant Stop Session");
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }


    }
}
