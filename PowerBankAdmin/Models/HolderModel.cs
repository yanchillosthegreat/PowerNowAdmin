using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBankAdmin.Models
{
    public class HolderModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string LocalCode { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerLatitude { get; set; }
        public string OwnerLongitude { get; set; }
        public int AvailablePowerBanks
        {
            get
            {
                return Powerbanks.ToList().Count(x => x.Electricity > 75 && !x.Sessions.Any(y => y.IsActive));
            }
        }
        public int FreeSlots
        {
            get
            {
                return 8 - Powerbanks.ToList().Count(x => !x.Sessions.Any(y => y.IsActive));
            }
        }
        public string Schedule { get; set; }
        public string Comment { get; set; }
        public IEnumerable<PowerbankModel> Powerbanks { get; set; }
        public IEnumerable<HolderRentModel> HolderRentModels { get; set; }
        public HolderModel()
        {
            Powerbanks = new List<PowerbankModel>();
        }


        public async Task AddRentModel(AppRepository appRepository, RentModel rentModel)
        {
            var neededRentModel = await appRepository.RentModels.FirstOrDefaultAsync(x => x.Id == rentModel.Id);
            if (neededRentModel == null) return;
            if (HolderRentModels == null) HolderRentModels = new List<HolderRentModel>();
            if (HolderRentModels.Any(x => x.RentModel.Id == rentModel.Id)) return;

            var rentModelsList = this.HolderRentModels.ToList();
            rentModelsList.Add(new HolderRentModel() { RentModel = neededRentModel, HolderModel = this});
            this.HolderRentModels = rentModelsList;
            appRepository.Entry(this).Collection(x => x.HolderRentModels).IsModified = true;
            await appRepository.SaveChangesAsync();
        }
    }

    public class HolderRentModel
    {
        public int Id { get; set; }
        public HolderModel HolderModel { get; set; }
        public RentModel RentModel { get; set; }
        public HolderRentModel()
        {
        }
    }

    public class RentModel
    {
        public int Id { get; set; }
        public RentStrategy RentStrategy { get; set; }
        public IEnumerable<HolderRentModel> HolderRentModels { get; set; }
    }

    public enum RentStrategy
    {
        Hour,
        Day,
        FirstHourFree
    }
}
