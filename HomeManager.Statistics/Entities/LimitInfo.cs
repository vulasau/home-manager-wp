using HomeManager.Entities.Enums;
using HomeManager.Statistics.Enums;

namespace HomeManager.Statistics.Entities
{
    public class LimitInfo
    {
        public string Name { get; set; }
        public CurrencyName Currency { get; set; }
        public double AmountSpent { get; set; }
        public double Limit { get; set; }
        public double RunOutDays { get; set; }
        public double Percentage { get; set; }
        public double Speed { get; set; }
        public SpeedDangerLevel SpeedDanger { get; set; }
    }
}
