using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Entities.Extensions;
using HomeManager.Infrastructure.Enums;
using System.Collections.Generic;
using System.Linq;

namespace HomeManager.Extensions
{
    public static class CollectionFilters
    {
        #region Operations
        public static IEnumerable<Operation> Filter(this IEnumerable<Operation> operations, CashAccount account, OperationType type, OperationFilter date, string search = null)
        {
            var collection = operations.ByType(account, type);

            if (string.IsNullOrEmpty(search))
            {
                if (date == OperationFilter.Week)
                    collection = collection.LastWeek();
                else if (date == OperationFilter.Month)
                    collection = collection.CurrentMonth();
                else if (date == OperationFilter.LastMonth)
                    collection = collection.LastMonth();
                return collection.OrderByDescending(x => x.Id);
            }

            return collection.Where(x => AcceptSearch(x, search)).OrderByDescending(x => x.Id);
        }

        private static bool AcceptSearch(Operation operation, string searchString)
        {
            string search = string.IsNullOrEmpty(searchString) ? null : searchString.ToLower();

            if (string.IsNullOrEmpty(search))
                return true;
            if (operation.Date.ToShortDateString().Contains(search))
                return true;
            if (operation.Amount.ToString().Contains(search))
                return true;
            if (operation.Category.Name.ToLower().Contains(search))
                return true;
            if (operation.Currency.ToString().ToLower().Contains(search))
                return true;
            if (!string.IsNullOrEmpty(operation.Description)
                && operation.Description.ToLower().Contains(search))
                return true;

            return false;
        }
        #endregion

        #region Categories
        public static IEnumerable<OperationCategory> Order(this IEnumerable<OperationCategory> categories, CategorySortOrder order)
        {
            if (order == CategorySortOrder.Name)
                return categories.OrderBy(x => x.Name);
            else
                return categories.OrderByDescending(x => x.Usage);
        }
        #endregion
    }
}
