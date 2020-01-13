using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Pages.Acquiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBankAdmin.Models
{
    public class CostumerModel : ICloneable
    {
		public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Card { get; set; }
        public int BindId { get; set; }
        public CostumerStatus CostumerStatus { get; set; }
        public string OrderId { get; set; }
        public CardsStatus CardsStatus { get; set; }

        public IEnumerable<VerificationCodeModel> Verifications { get; set; }
        public IEnumerable<CostumerAuthorizationModel> Authorizations { get; set; }
        public IEnumerable<PowerbankSessionModel> Sessions { get; set; }
        public IEnumerable<TransactionModel> Transaction { get; set; }
        public IEnumerable<CardBindingModel> CardBindings { get; set; }

        public CostumerModel()
        {
            Authorizations = new List<CostumerAuthorizationModel>();
            CardBindings = new List<CardBindingModel>();
        }

        public static async Task<CostumerModel> GetCostumerByOrderId(AppRepository appRepository, string orderId)
        {
            var costumer = await appRepository.Costumers.FirstOrDefaultAsync(x => x.OrderId == orderId);
            return costumer;
        }

        public async Task SetOrderId(AppRepository appRepository, string OrderId)
        {
            var costumerFromDb = await appRepository.Costumers.FirstOrDefaultAsync(x => x.Id == this.Id);
            costumerFromDb.OrderId = OrderId;
            appRepository.Entry(costumerFromDb).Property(x => x.OrderId).IsModified = true;
            await appRepository.SaveChangesAsync();
        }

        public async Task ClearBindings(AppRepository appRepository)
        {
            if (this.CardBindings == null || !CardBindings.Any()) return;
            (this.CardBindings as List<CardBindingModel>)?.Clear();
            appRepository.Entry(this).Collection(x => x.CardBindings).IsModified = true;
            await appRepository.SaveChangesAsync();
        }

        public async Task AddBinding(AppRepository appRepository, CardBindingModel bindingModel)
        {
            (this.CardBindings as List<CardBindingModel>)?.Add(bindingModel);
            appRepository.Entry(this).Collection(x => x.CardBindings).IsModified = true;
            await appRepository.SaveChangesAsync();
        }

        public async Task SetCardStatus(AppRepository appRepository, CardsStatus cardsStatus)
        {
            this.CardsStatus = cardsStatus;
            appRepository.Entry(this).Property(x => x.CardsStatus).IsModified = true;
            await appRepository.SaveChangesAsync();
        }

        public object Clone()
        {
            return new CostumerModel
            {
                Id = Id,
                Phone = Phone,
                Email = Email,
                Password = Password,
                Name = Name,
                CostumerStatus = CostumerStatus,
                Verifications = Verifications,
                Authorizations = Authorizations,
                CardBindings = CardBindings,
                Transaction = Transaction
            };
        }

    }

    public enum CostumerStatus
    {
        New,
        NotVeryfied,
        Veryfied
    }

    public enum CardsStatus
    {
        None,
        Progress,
        Ok
    }
}
