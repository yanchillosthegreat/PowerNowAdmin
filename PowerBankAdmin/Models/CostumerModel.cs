using System;
using System.Collections.Generic;

namespace PowerBankAdmin.Models
{
    public class CostumerModel
    {
		public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public CostumerStatus CostumerStatus { get; set; }

        public IEnumerable<VerificationCodeModel> Verifications { get; set; }
        public IEnumerable<CostumerAuthorizationModel> Authorizations { get; set; }

        public CostumerModel()
        {
            Authorizations = new List<CostumerAuthorizationModel>();
        }
    }

    public enum CostumerStatus
    {
        New,
        NotVeryfied,
        Veryfied
    }
}
