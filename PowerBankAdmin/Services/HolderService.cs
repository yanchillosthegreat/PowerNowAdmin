using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;
using PowerBankAdmin.Pages.Equipment;
using Yandex.Checkout.V3;

namespace PowerBankAdmin.Services
{
    public class HolderService : IHolderService
    {
        private readonly AppRepository _appRepository;
        public HolderService(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<bool> CanProvidePowerBank(int idHolder)
        {
            var holder = await _appRepository.Holders.Include(x => x.Powerbanks).ThenInclude(x => x.Sessions).FirstOrDefaultAsync(x => x.Id == idHolder);
            if (holder == null)
            {
                return false;
            }

            var powerBank = holder.Powerbanks.ToList().FirstOrDefault(x => x.Sessions.Count() == 0 || x.Sessions.All(y => !y.IsActive));
            if (powerBank == null)
            {
                return false;
            }

            return true;
        }

        public async Task<PowerbankSessionModel> LastSession(int idClient)
        {
            return await _appRepository.PowerbankSessions.LastOrDefaultAsync(x => x.IsActive && x.Costumer.Id == idClient);
        }

        public async Task<bool> ProvidePowerBank(int idCostumer, int holderId, RentModel rentModel, string cardBindingId)
        {
            var costumer = await _appRepository.Costumers.FirstOrDefaultAsync(x => x.Id == idCostumer);
            var holder = await _appRepository.Holders.Include(x => x.Powerbanks).ThenInclude(x => x.Sessions).FirstOrDefaultAsync(x => x.Id == holderId);
            if (costumer == null || holder == null) return false;

            var powerBank = holder.Powerbanks.Where(x => x.Sessions.Where(y => y.IsActive).Count() == 0 && x.Electricity > 75).OrderBy(x => x.Position).FirstOrDefault();
            if (powerBank == null) return false;

            if(!await PrvidePowerBank(holder, powerBank))
            {
                return false;
            }

            var card = await _appRepository.CardBidnings.FirstOrDefaultAsync(x => x.BindingId == cardBindingId);
            card.IsLocked = true;
            _appRepository.Entry(card).Property(x => x.IsLocked).IsModified = true;

            var session = new PowerbankSessionModel
            {
                Costumer = costumer,
                Start = DateTime.Now,
                Powerbank = powerBank,
                CardId = cardBindingId,
                RentModel = rentModel
            };

            await _appRepository.PowerbankSessions.AddAsync(session);
            await _appRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleasePowerBank(string powerBankCode, string holderCode, int position)
        {
            var powerBank = await _appRepository.Powerbanks.Include(x => x.Sessions).ThenInclude(x => x.RentModel).Include(x => x.Holder).FirstOrDefaultAsync(x => x.Code == powerBankCode);
            if (powerBank == null) return false;

            var session = powerBank.Sessions.FirstOrDefault(x => x.IsActive);
            if (session == null) return false;

            session.Finish = DateTime.Now;
            _appRepository.Entry(session).Property(x => x.Finish).IsModified = true;
            var oldHolder = powerBank.Holder;
            var newHolder = await _appRepository.Holders.FirstOrDefaultAsync(x => x.Code == holderCode);
            var oldHolderPowerBanks = oldHolder.Powerbanks.ToList();
            oldHolderPowerBanks.Remove(powerBank);
            oldHolder.Powerbanks = oldHolderPowerBanks;
            var newHolderPowerBanks = newHolder.Powerbanks.ToList();
            newHolderPowerBanks.Add(powerBank);
            newHolder.Powerbanks = newHolderPowerBanks;
            _appRepository.Entry(oldHolder).Collection(x => x.Powerbanks).IsModified = true;
            _appRepository.Entry(newHolder).Collection(x => x.Powerbanks).IsModified = true;

            var card = await _appRepository.CardBidnings.FirstOrDefaultAsync(x => x.BindingId == session.CardId);
            card.IsLocked = false;
            _appRepository.Entry(card).Property(x => x.IsLocked).IsModified = true;

            ICalculationStrategy strategy;

            switch (session.RentModel.RentStrategy)
            {
                case RentStrategy.Day:
                    strategy = new DayCalculationStrategy(99, false);
                    break;
                case RentStrategy.Hour:
                    strategy = new OneHourCalculationStrategy(49, 99, false);
                    break;
                case RentStrategy.FirstHourFree:
                    strategy = new OneHourCalculationStrategy(49, 99, true);
                    break;
                default:
                    strategy = null;
                    break;
            }

            if (strategy != null) {
                var amount = strategy.Calculate(session.Start, session.Finish);
                session.Price = amount;
                _appRepository.Entry(session).Property(x => x.Price).IsModified = true;
            }

            await _appRepository.SaveChangesAsync();
            return true;
        }

        private string _apiKey = "fa11c2ec-7c4a-4330-b2dd-bfad66375b6c";
        private string _baseUrl = "https://dry-wildwood-23355.herokuapp.com/operation";



        private async Task<bool> PrvidePowerBank(HolderModel holderModel, PowerbankModel powerbank)
        {
            var request = WebRequest.Create(_baseUrl);
            request.Method = "POST";
            using (Stream requestStream = request.GetRequestStream())
            {
                var holderRequest = new HolderRequest()
                {
                    Position = powerbank.Position.ToString(),
                    EquipmentSn = holderModel.Code,
                    DeviceType = "8",
                    DeviceVersion = "1",
                    SessionId = "94868768687778",
                    PackageType = "171"
                };
                var holderRequestString = JsonConvert.SerializeObject(holderRequest);
                var holderRequestBuf = Encoding.UTF8.GetBytes(holderRequestString);
                requestStream.Write(holderRequestBuf);
            }

            var response = await request.GetResponseAsync();
            string responceString = "";

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responceString = await reader.ReadToEndAsync();
            }
            return true;
        }

        public async Task UpdateHolderInfo(string holderCode)
        {
            var request = WebRequest.Create(_baseUrl);
            request.Method = "POST";
            using (Stream requestStream = request.GetRequestStream())
            {
                var holderRequest = new HolderRequest()
                {
                    EquipmentSn = holderCode,
                    DeviceType = "8",
                    DeviceVersion = "1",
                    SessionId = "94868768687778",
                    PackageType = "175"
                };
                var holderRequestString = JsonConvert.SerializeObject(holderRequest);
                var holderRequestBuf = Encoding.UTF8.GetBytes(holderRequestString);
                requestStream.Write(holderRequestBuf);
            }

            var response = await request.GetResponseAsync();
            string responceString = "";

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responceString = await reader.ReadToEndAsync();
            }
        }

        public async Task UpdatePowerbanksInfo(IEnumerable<EquipmentNotifyPowerbank> powerbanksNotifies)
        {
            foreach (var powerbanksNotify in powerbanksNotifies)
            {
                var powerbankToUpdate = await _appRepository.Powerbanks.Include(x => x.Holder).FirstOrDefaultAsync(x => x.Code == powerbanksNotify.PowerBankSn);
                if (powerbankToUpdate == null)
                {
                    var holder = await _appRepository.Holders.FirstOrDefaultAsync(x => x.Code == powerbanksNotify.EquipmentSn);
                    if (holder == null)
                    {
                        continue;
                    }

                    var powerbankToAdd = new PowerbankModel
                    {
                        Code = powerbanksNotify.PowerBankSn,
                        Position = int.Parse(powerbanksNotify.Position),
                        Electricity = int.Parse(powerbanksNotify.Electricity),
                        Holder = holder
                    };

                    await _appRepository.Powerbanks.AddAsync(powerbankToAdd);
                    await _appRepository.SaveChangesAsync();

                    continue;
                }

                powerbankToUpdate.Position = int.Parse(powerbanksNotify.Position);
                _appRepository.Entry(powerbankToUpdate).Property(x => x.Position).IsModified = true;

                powerbankToUpdate.Electricity = int.Parse(powerbanksNotify.Electricity);
                _appRepository.Entry(powerbankToUpdate).Property(x => x.Electricity).IsModified = true;

                var currentHolder = powerbankToUpdate.Holder;
                var newHolder = await _appRepository.Holders.FirstOrDefaultAsync(x => x.Code == powerbanksNotify.EquipmentSn);

                if (currentHolder == null || newHolder == null) continue;
                
                if (currentHolder.Code != newHolder.Code)
                {
                    powerbankToUpdate.Holder = newHolder;
                }
            }

            await _appRepository.SaveChangesAsync();
        }

        private class HolderRequest
        {
            public string Position { get; set; }
            public string EquipmentSn { get; set; }
            public string DeviceType { get; set; }
            public string DeviceVersion { get; set; }
            public string SessionId { get; set; }
            public string PackageType { get; set; }
        }
    }
}
