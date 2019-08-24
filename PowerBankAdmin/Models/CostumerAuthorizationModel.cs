using System;
namespace PowerBankAdmin.Models
{
    public class CostumerAuthorizationModel
	{
		public int Id { get; set; }
		public string AuthToken { get; set; }
		public CostumerModel Costumer { get; set; }
        public AuthType AuthType { get; set; }
        public DateTime CreationDate { get; set; }

		public CostumerAuthorizationModel()
		{
			CreationDate = DateTime.Now;
		}
	}

    public enum AuthType
    {
        Phone,
        Email,
        Social
    }
}
