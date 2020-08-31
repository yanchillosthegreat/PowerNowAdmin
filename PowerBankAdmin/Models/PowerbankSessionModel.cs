using System;
namespace PowerBankAdmin.Models
{
    public class PowerbankSessionModel
    {
        public int Id { get; set; }
        public CostumerModel Costumer { get; set; }
        public PowerbankModel Powerbank { get; set; }

        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public string CardId { get; set; }


        public bool IsActive => Start > Finish;

        public double Price { get; set; }

        public string SessionDuration
        {
            get
            {
                if (IsActive) return "Активна";
                var duration = Finish - Start;
                if (duration.TotalDays >= 1) return $"{Math.Ceiling(duration.TotalDays)} д";
                if (duration.TotalHours >= 1) return $"{Math.Ceiling(duration.TotalHours)} ч";
                if (duration.TotalMinutes >= 1) return $"{Math.Ceiling(duration.TotalMinutes)} мин";
                return $"{Math.Ceiling(duration.TotalSeconds)} сек";
            }
        }

        public RentModel RentModel { get; set; }

    }
}
