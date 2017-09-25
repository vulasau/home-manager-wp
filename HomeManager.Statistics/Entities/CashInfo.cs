using HomeManager.Entities.Enums;

namespace HomeManager.Statistics.Entities
{
    public class CashInfo
    {
        public int Iterations { get; set; }
        public double Amount { get; set; }
        public CurrencyName Currency { get; set; }
    }
}
