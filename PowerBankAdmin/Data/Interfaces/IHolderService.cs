using System;
using System.Threading.Tasks;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Data.Interfaces
{
    public interface IHolderService
    {
        Task<bool> ProvidePowerBank(int idCostumer, int idHolder);
        Task<PowerbankSessionModel> LastSession(int idClient); //Надо вернуть последнюю сессию пользователя, в ней сколько секунд у него PB и не вернул ли он его
    }
}
