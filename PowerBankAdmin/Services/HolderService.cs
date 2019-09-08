using System;
using System.Threading.Tasks;
using PowerBankAdmin.Data.Interfaces;

namespace PowerBankAdmin.Services
{
    public class HolderService : IHolderService
    {
        public Task<object> LastSession(int idClient)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProvidePowerBank(int idCostumer, int idHolder)
        {
            throw new NotImplementedException();
        }
    }
}
