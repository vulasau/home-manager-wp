using HomeManager.Entities;

namespace HomeManager.DataExport.Mappers
{
    public class OperationMapper: MapperBase
    {
        public override string MapObject(object obj)
        {
            var entity = (Operation)obj;
            var operation = string.Join(",",
                entity.Account.Name,
                entity.Date.ToShortDateString(),
                entity.Category.Name,
                entity.Amount,
                entity.Currency,
                !string.IsNullOrEmpty(entity.Description) ? entity.Description : string.Empty);

            return string.Format("{0}\n", operation);
        }

        public override string GetHeader()
        {
            var header = string.Join(",", "Account", "Date", "Category", "Amount", "Currency", "Description");
            return string.Format("{0}\n", header);
        }
    }
}
