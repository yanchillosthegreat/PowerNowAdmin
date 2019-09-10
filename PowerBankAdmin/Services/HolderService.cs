using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Services
{
    public class HolderService : IHolderService
    {
        private readonly AppRepository _appRepository;
        public HolderService(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<PowerbankSessionModel> LastSession(int idClient)
        {
            return await _appRepository.PowerbankSessions.LastOrDefaultAsync(x => x.IsActive && x.Costumer.Id == idClient);
        }

        public async Task<bool> ProvidePowerBank(int idCostumer, int idHolder)
        {
            var costumer = await _appRepository.Costumers.FirstOrDefaultAsync(x => x.Id == idCostumer);
            var holder = await _appRepository.Holders.Include(x => x.Powerbanks).ThenInclude(x => x.Sessions).FirstOrDefaultAsync(x => x.Id == idHolder);
            if (costumer == null || holder == null) return false;
            var powerBank = holder.Powerbanks.ToList().FirstOrDefault(x => x.Sessions.Count() == 0 || x.Sessions.All(x => !x.IsActive));
            if (powerBank == null) return false;
            var session = new PowerbankSessionModel
            {
                Costumer = costumer,
                Start = DateTime.Now,
                Powerbank = powerBank
            };

            await _appRepository.PowerbankSessions.AddAsync(session);
            await _appRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleasePowerBank(int idPowerBank)
        {
            var powerBank = await _appRepository.Powerbanks.Include(x => x.Sessions).FirstOrDefaultAsync(x => x.Id == idPowerBank);
            if (powerBank == null) return false;

            var session = powerBank.Sessions.FirstOrDefault(x => x.IsActive);
            if (session == null) return false;

            session.Finish = DateTime.Now;
            _appRepository.Entry(session).Property(x => x.Finish).IsModified = true;
            await _appRepository.SaveChangesAsync();
            return true;
        }
    }
}
