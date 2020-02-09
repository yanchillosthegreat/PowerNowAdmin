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
            var client = new Client(shopId: "667169", secretKey: "test_yaa_BuTea1360q-9lXQVQRzdqSiThR_2b_6U_P2wXas");

            var oneHourCalculationStrategy = new OneHourCalculationStrategy(49, 99, true);
            var amount1 = oneHourCalculationStrategy.Calculate(session.Start, session.Finish);

            client.CreatePayment(new NewPayment
            {
                Amount = new Amount { Currency = "RUB", Value = 99m },
                //PaymentMethodId = customer.CardBindings.LastOrDefault().BindingId,
                PaymentMethodId = "25c95fa8-000f-5000-9000-192ee86a71b2",
                Description = "Автоплатеж Тест #1",
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
