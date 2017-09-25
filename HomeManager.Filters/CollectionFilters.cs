using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager.Filters
{
    public class CollectionFilters
    {
        public static bool AccepWeek(DateTime current)
        {
            return current > DateTime.Now.AddDays(-7);
        }

        public static bool AccepMonth(DateTime current)
        {
            return current.Year == DateTime.Now.Year
                && current.Month == DateTime.Now.Month;
        }
    }
}
