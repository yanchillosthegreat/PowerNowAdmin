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

namespace PowerBankAdmin.Pages.Admin.Holders
{
    public class IndexModel : BaseAuthedPageModel
    {
        private readonly AppRepository _appRepository;

        [BindProperty]
        public IEnumerable<HolderModel> Holders { get; set; }
        [BindProperty]
        public HolderModel HolderToAdd { get; set; }


        public IndexModel(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            IdentifyUser();
            Holders = await _appRepository.Holders.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!string.IsNullOrEmpty(HolderToAdd?.LocalCode) && string.IsNullOrEmpty(HolderToAdd?.Code))
            {
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Не указаны необходимые параметры");
            }
            if (!await CheckHolder())
            {
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Холдер с таким кодом уже существует");
            }

            await _appRepository.Holders.AddAsync(HolderToAdd);
            await _appRepository.SaveChangesAsync();
            var newRowHtml = BuildHtmlHolderRow();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode, newRowHtml);
        }

        public async Task<IActionResult> OnDeleteAsync(int? id)
        {
            if (id == null || id <= 0) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);

            var holderToRemove = await _appRepository.Holders.FirstOrDefaultAsync(x => x.Id == id);
            if (holderToRemove == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);

            _appRepository.Holders.Remove(holderToRemove);
            await _appRepository.SaveChangesAsync();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        private string BuildHtmlHolderRow()
        {
            return $"<tr id=\"{HolderToAdd.Id}\"> <td> {HolderToAdd.Id} </td> <td> {HolderToAdd.LocalCode} </td> <td> {HolderToAdd.Code} </td><td> <button class=\"button btn-xs btn-danger\" onclick=\"deleteHolder({HolderToAdd.Id})\">x</button></td></tr>";
        }

        private async Task<bool> CheckHolder()
        {
            var holderToCheck = await _appRepository.Holders.FirstOrDefaultAsync(x => x.LocalCode == HolderToAdd.LocalCode);
            if (holderToCheck != null) return false;
            holderToCheck = await _appRepository.Holders.FirstOrDefaultAsync(x => x.Code == HolderToAdd.Code);
            return holderToCheck == null;
        }
    }
}
