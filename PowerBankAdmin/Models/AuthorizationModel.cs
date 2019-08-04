
using System;
using PowerBankAdmin.Models;

namespace PowerBankAdmin
{
    public class AuthorizationModel
    {
        public int Id { get; set; }
        public string AuthToken { get; set; }
        public UserModel User { get; set; }
        public DateTime CreationDate { get; set; }

        public AuthorizationModel()
        {
            CreationDate = DateTime.Now;
        }
    }
}
