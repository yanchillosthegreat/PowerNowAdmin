using System;
namespace PowerBankAdmin.Models
{
    public class PowerbankModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public HolderModel Holder { get; set; }
        public CostumerModel Costumer { get; set; }


        public PowerbankModel()
        {
        }
    }
}
