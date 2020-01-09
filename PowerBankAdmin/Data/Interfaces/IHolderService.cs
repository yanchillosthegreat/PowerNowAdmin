using System;
using System.Threading.Tasks;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Data.Interfaces
{
    public interface IHolderService
    {
        /// <summary>
        /// Выдать PB
        /// </summary>
        /// <param name="idCostumer">ID Пользователя</param>
        /// <param name="idHolder">ID Холдера</param>
        /// <returns>true, если получилось</returns>
        Task<bool> ProvidePowerBank(int idCostumer, int idHolder);
        /// <summary>
        /// Последняя сессия пользователя
        /// </summary>
        /// <param name="idClient">ID клиента</param>
        /// <returns>Последняя сессия пользователя</returns>
        Task<PowerbankSessionModel> LastSession(int idClient); //Надо вернуть последнюю сессию пользователя, в ней сколько секунд у него PB и не вернул ли он его
        /// <summary>
        /// //Вызывать эту функцию, когда ответ от китайцев пришел, что сдали PB
        /// </summary>
        /// <param name="idPowerBank">ID PB</param>
        /// <returns>true, если получилось</returns>
        Task<bool> ReleasePowerBank(int idPowerBank);

        Task<bool> CanProvidePowerBank(int idHolder);
    }
}
