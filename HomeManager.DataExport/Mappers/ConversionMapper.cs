using HomeManager.Entities;

namespace HomeManager.DataExport.Mappers
{
    public class ConversionMapper: MapperBase
    {
        public override string MapObject(object obj)
        {
            var entity = (Conversion)obj;
            var conversion = string.Join(",",
                entity.Account.Name,
                entity.Date.ToShortDateString(),
                entity.Type,
                entity.From,
                entity.To,
                entity.Amount,
                entity.Rate);

            return string.Format("{0}\n", conversion);
        }

        public override string GetHeader()
        {
            var header = string.Join(",", "Account", "Date", "Type", "From", "To", "Amount", "Rate");
            return string.Format("{0}\n", header);
        }
    }
}
