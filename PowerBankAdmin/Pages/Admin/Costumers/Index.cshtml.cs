using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Admin.Costumers
{
    public class IndexModel : BaseAuthedPageModel
    {
        private readonly AppRepository _appRepository;

        [BindProperty]
        public IEnumerable<CostumerModel> Costumers { get; set; }

        public IndexModel(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            IdentifyUser();
            Costumers = await _appRepository.Costumers.Include(x => x.Verifications).Include(y => y.Authorizations).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnDeleteAsync(int? id)
        {
            if(id == null || id <=0 ) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);

            var costumer = await _appRepository.Costumers.FirstOrDefaultAsync(x => x.Id == id);
            if(costumer == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);

            _appRepository.Costumers.Remove(costumer);
            await _appRepository.SaveChangesAsync();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
    }
}
