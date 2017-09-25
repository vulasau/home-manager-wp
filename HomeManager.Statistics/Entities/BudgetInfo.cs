using HomeManager.Entities.Enums;
using System;

namespace HomeManager.Statistics.Entities
{
    public class BudgetInfo
    {
        public string Period { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public double Percentage { get; set; }

        public BudgetInfo()
        {

        }

        public BudgetInfo(DateTime period, double amount, CurrencyName currency, double percentage)
        {
            Period = string.Format("{0:MM/yyyy}", period);
            Amount = Math.Round(amount, 2);
            Currency = currency.ToString();
            Percentage = Math.Round(percentage, 2);
        }
    }
}
