using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Admin.Holders
{
    public class IndexModel : BaseAuthedPageModel
    {
        private readonly AppRepository _appRepository;
        private readonly IGeocodeService _geocode;

        [BindProperty]
        public IEnumerable<HolderModel> Holders { get; set; }
        [BindProperty]
        public HolderModel HolderToAdd { get; set; }
        [BindProperty]
        public List<int> PayAvialabilities { get; set; } //1 - Hour 2 - Day 3 - First Hour Free
        [BindProperty]
        public bool FirstHourFree { get; set; }

        public IndexModel(AppRepository appRepository, IGeocodeService geocode)
        {
            _appRepository = appRepository;
            _geocode = geocode;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            IdentifyUser();
            Holders = await _appRepository.Holders.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetAddresssuggestAsync(string search)
        {
            var adresses = await _geocode.AutosuggestAddress(search);
            var sel2l = new List<Select2Obj>();
            if(adresses != null)
            {
                foreach(var ad in adresses)
                {
                    sel2l.Add(new Select2Obj {Id = ad.FormattedAddress, Text = ad.FormattedAddress });
                }
            }
            return new JsonResult(sel2l);
        }
        public class Select2Obj
        {
            public string Id { get; set; }
            public string Text { get; set; }
        };
        IEnumerable<RentModel> RentModels { get; set; }
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
            if (!await CalculateCoords())
            {
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Неверный адрес, не удалось определить координаты");
            }
            RentModels = await _appRepository.RentModels.ToListAsync();
            
            await _appRepository.Holders.AddAsync(HolderToAdd);
            await _appRepository.SaveChangesAsync();
            await CalculateTariffs(HolderToAdd);
            var newRowHtml = BuildHtmlHolderRow();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode, newRowHtml);
        }

        private async Task CalculateTariffs(HolderModel holderToAdd)
        {
            if (PayAvialabilities == null) return;
            var rentModels = new List<RentModel>();
            if (PayAvialabilities.Contains(1)) await HolderToAdd.AddRentModel(_appRepository, RentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.Hour));
            if (PayAvialabilities.Contains(2)) await HolderToAdd.AddRentModel(_appRepository, RentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.Day));
            if (PayAvialabilities.Contains(3)) await HolderToAdd.AddRentModel(_appRepository, RentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.FirstHourFree));
        }

        private async Task<bool> CalculateCoords()
        {
            var cords = await _geocode.Geocode(HolderToAdd.OwnerAddress);
            if (String.IsNullOrEmpty(cords.latitude)) return false;
            HolderToAdd.OwnerLatitude = cords.latitude;
            HolderToAdd.OwnerLongitude = cords.longitude;
            return true;
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
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
            return $"<tr id=\"{HolderToAdd.Id}\"> <td> {HolderToAdd.Id} </td> <td> {HolderToAdd.LocalCode} </td> <td> {HolderToAdd.Code} </td> <td> {HolderToAdd.OwnerName} </td> <td> {HolderToAdd.OwnerAddress} </td> <td> <button class=\"button btn-xs btn-danger\" onclick=\"deleteHolder({HolderToAdd.Id})\">x</button></td></tr>";
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
