

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;

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
            Sessions = _appRepository.PowerbankSessions.Include(x => x.Powerbank).ThenInclude(x => x.Holder).Where(x => x.Costumer.Id == Costumer.Id);
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

    }
}
