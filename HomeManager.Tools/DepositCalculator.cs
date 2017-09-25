using HomeManager.Tools.Entities;
using System;

namespace HomeManager.Tools
{
    public class DepositCalculator
    {
        public DepositInfo CalculateDeposit(double deposit, double monthDeposit, double persentageYear, int periodMonth)
        {
            double totalAmount = Calculate(deposit, monthDeposit, persentageYear, periodMonth);
            double clearAmount = Calculate(deposit, monthDeposit, periodMonth);
            double income = totalAmount - clearAmount;

            var info = new DepositInfo()
            {
                TotalAmount = Math.Round(totalAmount, 2),
                ClearAmount = Math.Round(clearAmount, 2),
                Income = Math.Round(income, 2),
                PeriodMonth = periodMonth
            };

            return info;
        }

        private double Calculate(double deposit, double monthDeposit, double persentage, int periodMonth)
        {
            double result = deposit;
            for (int i = 1; i <= periodMonth; i++)
            {
                result += GetPersent(result, persentage) + monthDeposit;
            }
            return Math.Round(result, 2);
        }

        private double Calculate(double deposit, double monthIncome, int periodMonth)
        {
            double result = deposit;
            for (int i = 1; i <= periodMonth; i++)
            {
                result += monthIncome;
            }
            return Math.Round(result, 2);
        }

        private double GetPersent(double value, double persentage)
        {
            double result = value * (persentage / 12) / 100;
            return Math.Round(result, 2);
        }
    }
}
