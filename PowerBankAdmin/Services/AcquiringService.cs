using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Models;
using System;
using System.Threading.Tasks;
using Yandex.Checkout.V3;

namespace PowerBankAdmin.Services
{
    public class AcquiringService : IAcquiringService
    {
        public AcquiringService()
        {
        }

        public void ProceedPayment(PowerbankSessionModel session)
        {
            var client = new Client(shopId: Strings.YandexShopId, secretKey: Strings.YandexAPIKey);

            ICalculationStrategy strategy;

            switch (session.RentModel.RentStrategy)
            {
                case RentStrategy.Day:
                    strategy = new DayCalculationStrategy(99, false);
                    break;
                case RentStrategy.Hour:
                    strategy = new OneHourCalculationStrategy(49, 99, false);
                    break;
                case RentStrategy.FirstHourFree:
                    strategy = new OneHourCalculationStrategy(49, 99, true);
                    break;
                default:
                    return;
            }

            var amount = strategy.Calculate(session.Start, session.Finish);

            if (amount <= 0) return;

            client.CreatePayment(new NewPayment
            {
                Amount = new Amount { Currency = "RUB", Value = new decimal(amount) },
                PaymentMethodId = session.CardId,
                Capture = true,
                Description = $"{session.Costumer.Phone}, {session.Start} - {session.Finish}",
                Confirmation = new Confirmation
                {
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = ""
                },
            });
        }
    }

    public interface ICalculationStrategy
    {
        double Calculate(DateTime startTime, DateTime endTime);
    }

    public class OneHourCalculationStrategy : ICalculationStrategy
    {
        private double firstHourPrice = 0;
        private double dayPrice = 0;
        private bool isFirstHourFree = false;

        public OneHourCalculationStrategy(double firstHourPrice, double dayPrice, bool isFirstHourFree)
        {
            this.firstHourPrice = firstHourPrice;
            this.dayPrice = dayPrice;
            this.isFirstHourFree = isFirstHourFree;
        }

        public double Calculate(DateTime startTime, DateTime endTime)
        {
            var totalMinutes = (int)(endTime - startTime).TotalMinutes;
            totalMinutes = (totalMinutes == 0) ? ((int)(endTime - startTime).TotalSeconds > 0 ? 1 : 0) : (totalMinutes);
            var totalHours = totalMinutes / 60 + (totalMinutes % 60 == 0 ? 0 : 1);

            if (isFirstHourFree)
            {
                totalHours = Math.Max(totalHours - 1, 0);
            }

            if (totalHours == 0)
            {
                return 0;
            }
            else if (totalHours == 1)
            {
                return firstHourPrice;
            }
            else
            {
                return ((totalHours / 24) + (totalHours % 24 == 0 ? 0 : 1)) * dayPrice;
            }
        }
    }

    public class DayCalculationStrategy : ICalculationStrategy
    {
        private double dayPrice = 0;
        private bool isFirstHourFree = false;

        public DayCalculationStrategy(double dayPrice, bool isFirstHourFree)
        {
            this.dayPrice = dayPrice;
            this.isFirstHourFree = isFirstHourFree;
        }

        public double Calculate(DateTime startTime, DateTime endTime)
        {
            var totalMinutes = (int)(endTime - startTime).TotalMinutes;
            totalMinutes = (totalMinutes == 0) ? ((int)(endTime - startTime).TotalSeconds > 0 ? 1 : 0) : (totalMinutes);
            var totalHours = totalMinutes / 60 + (totalMinutes % 60 == 0 ? 0 : 1);

            if (isFirstHourFree)
            {
                totalHours = Math.Max(totalHours - 1, 0);
            }

            if (totalHours == 0)
            {
                return 0;
            }
            else
            {
                return ((totalHours / 24) + (totalHours % 24 == 0 ? 0 : 1)) * dayPrice;
            }
        }
    }
}
