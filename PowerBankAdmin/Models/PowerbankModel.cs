using System;
using System.Collections.Generic;

namespace PowerBankAdmin.Models
{
    public class PowerbankModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public HolderModel Holder { get; set; }

        public IEnumerable<PowerbankSessionModel> Sessions { get; set; }

        public PowerbankModel()
        {
        }
    }
}
