using System;
namespace PowerBankAdmin.Models
{
    public class PowerbankModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public HolderModel Powerbanks { get; set; }


        public PowerbankModel()
        {
        }
    }
}
