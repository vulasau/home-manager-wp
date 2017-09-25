using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager.Core
{
    public interface IDataExportService
    {
        void Export<T>(IEnumerable<T> collection, string filename);
    }
}
