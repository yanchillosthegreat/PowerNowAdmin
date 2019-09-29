using System;
using System.Collections.Generic;

namespace PowerBankAdmin.Models
{
    public class HolderModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string LocalCode { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public IEnumerable<PowerbankModel> Powerbanks { get; set; }

        public HolderModel()
        {

        }
    }
}
