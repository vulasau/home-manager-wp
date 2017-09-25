using HomeManager.Tools.Entities;
using System;

namespace HomeManager.Tools
{
    public class CreditCalculator
    {
        public CreditInfo Calculate(double amount, double firstPayment, double percent, int periodMonth, double monthEarnings)
        {
            double sum = amount - firstPayment;
            double a = 1 + percent / 1200;
            double k = Math.Pow(a, periodMonth) * (a - 1) / (Math.Pow(a, periodMonth) - 1);

            double payment = k * sum;
            double fullPrice = payment * periodMonth + firstPayment;
            bool possible = payment < monthEarnings;

            var info = new CreditInfo()
            {
                FullPrice = Math.Round(fullPrice, 2),
                PaymentMonth = Math.Round(payment, 2),
                IsPossible = possible
            };

            return info;
        }
    }
}
