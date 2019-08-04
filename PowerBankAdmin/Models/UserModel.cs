using System;
using System.Collections.Generic;

namespace PowerBankAdmin.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public IEnumerable<AuthorizationModel> Authorizations { get; set; }

        public UserModel()
        {
            Authorizations = new List<AuthorizationModel>();
        }
    }
}
