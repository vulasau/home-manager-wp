using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager.DataExport.Mappers
{
    public abstract class MapperBase
    {
        public abstract string MapObject(object entity);
        public abstract string GetHeader();
    }
}
