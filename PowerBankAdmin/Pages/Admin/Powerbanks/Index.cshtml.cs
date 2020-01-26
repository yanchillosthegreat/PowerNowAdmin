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
        [BindProperty]
        public IEnumerable<HolderModel> Holders { get; set; }

        [BindProperty]
        public PowerbankModel PowerbankToAdd { get; set; }
        //TODO: Find How to link object in select
        [BindProperty]
        public int PowerbankToAddHolderId { get; set; }

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
            Holders = await _appRepository.Holders.ToListAsync();
        }

        public async Task<IActionResult> OnPostStopAsync(int? id)
        {
            if(id == null || id < 1)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Wrong id");
            var powerBank = await _appRepository.Powerbanks.Include(x => x.Holder).FirstOrDefaultAsync(x => x.Id == id);
            var result = await _holderService.ReleasePowerBank(powerBank.Code, powerBank.Holder.Code, 1);
            if(!result)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpServerErrorCode, "Cant Stop Session");
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        public async Task<IActionResult> OnDeleteAsync(int? id)
        {
            if (id == null || id <= 0) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);

            var powerbankToRemove = await _appRepository.Powerbanks.FirstOrDefaultAsync(x => x.Id == id);
            if (powerbankToRemove == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);

            _appRepository.Powerbanks.Remove(powerbankToRemove);
            await _appRepository.SaveChangesAsync();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(PowerbankToAdd?.Code))
            {
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Не указаны необходимые параметры");
            }
            if (!await FixPowerBank(PowerbankToAddHolderId))
            {
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Powerbank с таким кодом уже существует");
            }

            await _appRepository.Powerbanks.AddAsync(PowerbankToAdd);
            await _appRepository.SaveChangesAsync();
            var newRowHtml = BuildHtmlHolderRow();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode, newRowHtml);
        }

        private string BuildHtmlHolderRow()
        {
            return $"<tr id=\"{PowerbankToAdd.Id}\"> <td> {PowerbankToAdd.Id} </td> <td> {PowerbankToAdd.Code} </td> <td> {PowerbankToAdd.Holder?.LocalCode} </td> <td> </td> <td> <span class=\"badge badge-warning\">Finished</span> </td><td> <button class=\"button btn-xs btn-warning\" onclick=\"stopSession({PowerbankToAdd.Id})\">stop</button> <button class=\"button btn-xs btn-warning\" onclick=\"deletePowerbank({PowerbankToAdd.Id})\">x</button></td></tr>";
        }

        private async Task<bool> FixPowerBank(int holderId)
        {
            var powerbankToCheck = await _appRepository.Powerbanks.FirstOrDefaultAsync(x => x.Code == PowerbankToAdd.Code);
            if (powerbankToCheck != null) return false;
            var holder = await _appRepository.Holders.FirstOrDefaultAsync(x => x.Id == holderId);
            if (holder == null) return false;
            PowerbankToAdd.Holder = holder;
            return true;

        }
    }
}
