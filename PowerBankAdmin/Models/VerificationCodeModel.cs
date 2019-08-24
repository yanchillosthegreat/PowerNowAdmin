using System;
namespace PowerBankAdmin.Models
{
    public class VerificationCodeModel
    {
		public int Id { get; set; }
        public int Code { get; set; }
        public CostumerModel  Costumer { get; set; }
        public DateTime CreationDate { get; set; }

        public VerificationCodeModel()
        {
            CreationDate = DateTime.Now;
        }
	}
}
