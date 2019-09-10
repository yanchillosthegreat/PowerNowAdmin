using System;
namespace PowerBankAdmin.Models
{
    public class PowerbankSessionModel
    {
		public int Id { get; set; }
        public CostumerModel Costumer { get; set; }
        public PowerbankModel Powerbank { get; set; }

        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public bool IsActive => Start < Finish;

    }
}
