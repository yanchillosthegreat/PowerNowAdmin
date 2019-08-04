using System;
using System.Collections.Generic;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Data.Moq
{
    public class UsersMoq
    {
		public IEnumerable<UserModel> Users { get; set; }
		public UsersMoq()
        {
            Users = new List<UserModel>
            {
                new UserModel
                {
                    Id = 1, Login = "sprodan", Password = "123"
                },
                new UserModel
                {
                    Id = 2, Login = "moroz", Password = "456"
                }
            };
        }
    }
}
