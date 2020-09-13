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
using static PowerBankAdmin.Pages.Admin.Holders.IndexModel;

namespace PowerBankAdmin.Pages.Admin.Holders
{
    public class EdutModel : BaseAuthedPageModel
    {
        private readonly AppRepository _appRepository;
        private readonly IGeocodeService _geocode;

        [BindProperty]
        public HolderModel Holder { get; set; }

        [BindProperty]
        public List<int> PayAvialabilities { get; set; } //1 - Hour 2 - Day 3 - First Hour Free

        public EdutModel(AppRepository appRepository, IGeocodeService geocode)
        {
            _appRepository = appRepository;
            _geocode = geocode;

            PayAvialabilities = new List<int>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            IdentifyUser();
            Holder = await _appRepository.Holders.Include(x => x.HolderRentModels).ThenInclude(x => x.RentModel).FirstOrDefaultAsync(x => x.Id == id);

            Holder.HolderRentModels.ToList().ForEach(x =>
            {
                if (x.RentModel.RentStrategy == RentStrategy.Hour)
                {
                    PayAvialabilities.Add(1);
                }
                else if (x.RentModel.RentStrategy == RentStrategy.Day)
                {
                    PayAvialabilities.Add(2);
                }
                else if (x.RentModel.RentStrategy == RentStrategy.FirstHourFree)
                {
                    PayAvialabilities.Add(3);
                }
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var holderToEdit = await _appRepository.Holders.Include(x => x.HolderRentModels).ThenInclude(x => x.RentModel).FirstOrDefaultAsync(x => x.Id == Holder.Id);
            if (holderToEdit == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);

            _appRepository.Entry(Holder).Property(x => x.LocalCode).IsModified = true;
            _appRepository.Entry(Holder).Property(x => x.Code).IsModified = true;
            _appRepository.Entry(Holder).Property(x => x.OwnerName).IsModified = true;
            _appRepository.Entry(Holder).Property(x => x.OwnerAddress).IsModified = true;
            _appRepository.Entry(Holder).Property(x => x.Schedule).IsModified = true;
            _appRepository.Entry(Holder).Property(x => x.Comment).IsModified = true;

            var rentModels = await _appRepository.RentModels.ToListAsync();
            if (PayAvialabilities.Contains(1))
            {
                await holderToEdit.AddRentModel(_appRepository, rentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.Hour));
            }
            else
            {
                await holderToEdit.RemoveRentModel(_appRepository, rentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.Hour));
            }
            if (PayAvialabilities.Contains(2))
            {
                await holderToEdit.AddRentModel(_appRepository, rentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.Day));
            }
            else
            {
                await holderToEdit.RemoveRentModel(_appRepository, rentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.Day));
            }
            if (PayAvialabilities.Contains(3))
            {
                await holderToEdit.AddRentModel(_appRepository, rentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.FirstHourFree));
            }
            else
            {
                await holderToEdit.RemoveRentModel(_appRepository, rentModels.FirstOrDefault(x => x.RentStrategy == RentStrategy.FirstHourFree));
            }
            _appRepository.Entry(holderToEdit).Collection(x => x.HolderRentModels).IsModified = true;

            await _appRepository.SaveChangesAsync();
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        public async Task<IActionResult> OnGetAddresssuggestAsync(string search)
        {
            var adresses = await _geocode.AutosuggestAddress(search);
            var sel2l = new List<Select2Obj>();
            if (adresses != null)
            {
                foreach (var ad in adresses)
                {
                    sel2l.Add(new Select2Obj { Id = ad.FormattedAddress, Text = ad.FormattedAddress });
                }
            }
            return new JsonResult(sel2l);
        }
    }
}