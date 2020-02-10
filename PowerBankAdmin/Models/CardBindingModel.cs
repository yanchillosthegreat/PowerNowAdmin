 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBankAdmin.Models
{
    public class CardBindingModel
    {
        public int Id { get; set; }
        public string BindingId { get; set; }
        public string ExpiryDate { get; set; }
        public string FirstDigits { get; set; }
        public string LastDigits { get; set; }
        public bool IsLocked { get; set; }
    }
}
