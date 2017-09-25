using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager.Statistics.Entities
{
    public class CategoryStatistics
    {
        public string Currency { get; set; }
        public IEnumerable<CategoryInfo> Info { get; set; }
    }
}
