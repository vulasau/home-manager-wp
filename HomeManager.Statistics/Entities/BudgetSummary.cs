using HomeManager.Entities.Enums;
using System.Collections.Generic;

namespace HomeManager.Statistics.Entities
{
    public class BudgetSummary
    {
        public string Currency { get; set; }
        public IEnumerable<BudgetInfo> Items { get; set; }

        public BudgetSummary()
        {

        }

        public BudgetSummary(CurrencyName currency, IEnumerable<BudgetInfo> items)
        {
            Currency = currency.ToString();
            Items = items;
        }
    }
}
