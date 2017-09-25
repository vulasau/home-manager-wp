using System;
using System.Collections.Generic;
using System.Text;
using HomeManager.Entities;
using HomeManager.PhoneMemoryAccess.Interfaces;
using HomeManager.DataExport.Mappers;
using HomeManager.Core;

namespace HomeManager.DataExport
{
    public class CsvDataExportService: IDataExportService
    {
        private readonly IPhoneMemoryService _memoryService;

        public CsvDataExportService(IPhoneMemoryService memoryService)
        {
            _memoryService = memoryService;
        }

        public void Export<T>(IEnumerable<T> collection, string fileName)
        {
            var mapper = GetMapperForType(typeof(T));
            var csv = MapCollection(collection, mapper);
            _memoryService.WriteFile(fileName, "HomeManager", csv, true);
        }

        private string MapCollection<T>(IEnumerable<T> collection, MapperBase mapper)
        {
            var builder = new StringBuilder();
            builder.Append(mapper.GetHeader());
            foreach (var item in collection)
            {
                builder.Append(mapper.MapObject(item));
            }
            return builder.ToString();
        }

        private MapperBase GetMapperForType(Type type)
        {
            if (type == typeof(Operation))
                return new OperationMapper();
            else if (type == typeof(Conversion))
                return new ConversionMapper();
            throw new NotSupportedException(string.Format("There is no mapper available for {0} type.", type));
        }
    }
}
