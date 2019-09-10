using System;
using System.Collections.Generic;

namespace PowerBankAdmin.Models
{
    public class CostumerModel : ICloneable
    {
		public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public CostumerStatus CostumerStatus { get; set; }
        

        public IEnumerable<VerificationCodeModel> Verifications { get; set; }
        public IEnumerable<CostumerAuthorizationModel> Authorizations { get; set; }
        public IEnumerable<PowerbankSessionModel> Sessions { get; set; }


        public CostumerModel()
        {
            Authorizations = new List<CostumerAuthorizationModel>();
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
                Authorizations = Authorizations
            };
        }
    }

    public enum CostumerStatus
    {
        New,
        NotVeryfied,
        Veryfied
    }
}
