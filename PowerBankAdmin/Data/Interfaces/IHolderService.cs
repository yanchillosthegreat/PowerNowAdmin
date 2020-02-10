using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PowerBankAdmin.Models;
using PowerBankAdmin.Pages.Equipment;

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
        Task<bool> ProvidePowerBank(int idCostumer, int holderCode, RentModel rentModel, string cardBindingId);
        /// <summary>
        /// Последняя сессия пользователя
        /// </summary>
        /// <param name="idClient">ID клиента</param>
        /// <returns>Последняя сессия пользователя</returns>
        Task<PowerbankSessionModel> LastSession(int idClient); //Надо вернуть последнюю сессию пользователя, в ней сколько секунд у него PB и не вернул ли он его
        /// <summary>
        /// //Вызывать эту функцию, когда ответ от китайцев пришел, что сдали PB
        /// </summary>
        /// <returns>true, если получилось</returns>
        Task<bool> ReleasePowerBank(string powerBankCode, string holderCode, int position);

        Task<bool> CanProvidePowerBank(int idHolder);

        Task UpdatePowerbanksInfo(IEnumerable<EquipmentNotifyPowerbank> powerbanksNotifies);

        Task UpdateHolderInfo(string holderCode);
    }
}
