using System;
namespace PowerBankAdmin.Models
{
    public class TransactionModel
    {
		public int Id { get; set; }
		public int Amount { get; set; }
		public DateTime Date { get; set; }

        public TransactionModel()
        {
            Date = DateTime.Now;
        }
    }
}
