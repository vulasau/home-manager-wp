using HomeManager.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManager.Entities.Extensions
{
    public static class CollectionFilters
    {
        #region Operations
        public static IEnumerable<Operation> ByAccount(this IEnumerable<Operation> operations, CashAccount account)
        {
            return operations.Where(x => x.Account.Equals(account));
        }

        public static IEnumerable<Operation> ByType(this IEnumerable<Operation> operations, CashAccount account, OperationType type)
        {
            return operations.Where(x => x.Account.Equals(account) && x.Type.Equals(type));
        }

        public static IEnumerable<Operation> ByType(this IEnumerable<Operation> operations, OperationType type)
        {
            return operations.Where(x => x.Type.Equals(type));
        }

        public static IEnumerable<Operation> ByCurrency(this IEnumerable<Operation> operations, CurrencyName currency)
        {
            return operations.Where(x => x.Currency.Equals(currency));
        }

        public static IEnumerable<Operation> ByCategory(this IEnumerable<Operation> operations, OperationCategory category)
        {
            return operations.Where(x => x.Category.Equals(category));
        }

        public static IEnumerable<Operation> ByMonth(this IEnumerable<Operation> operations, DateTime date = default(DateTime))
        {
            if (date == default(DateTime))
                return operations;

            return operations.Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month);
        }

        public static IEnumerable<Operation> LastMonth(this IEnumerable<Operation> operations)
        {
            var now = DateTime.Now;
            return operations.Where(x => x.Date.Year == now.Year
                && x.Date.DayOfYear >= now.Date.AddMonths(-1).DayOfYear);
        }

        public static IEnumerable<Operation> CurrentMonth(this IEnumerable<Operation> operations)
        {
            var now = DateTime.Now;
            return operations.Where(x => x.Date.Year == now.Year
                && x.Date.Month == now.Month);
        }

        public static IEnumerable<Operation> LastWeek(this IEnumerable<Operation> operations)
        {
            var now = DateTime.Now;
            return operations.Where(x => x.Date.Year == now.Year
                && x.Date.DayOfYear >= now.Date.AddDays(-7).DayOfYear);
        }


        public static IEnumerable<CurrencyName> UsedCurrencies(this IEnumerable<Operation> operations)
        {
            var currencies = new List<CurrencyName>();

            foreach (var operation in operations)
            {
                if (!currencies.Contains(operation.Currency))
                    currencies.Add(operation.Currency);
            }

            return currencies;
        }

        public static IEnumerable<OperationCategory> UsedCategories(this IEnumerable<Operation> operations)
        {
            var categories = new List<OperationCategory>();

            foreach (var operation in operations)
            {
                if (!categories.Contains(operation.Category))
                    categories.Add(operation.Category);
            }

            return categories;
        }

        public static IEnumerable<DateTime> PeriodMonths(this IEnumerable<Operation> operations)
        {
            var periods = new List<DateTime>();
            foreach (var operation in operations)
            {
                if (periods.Any(x => CompareDatesByMonth(x, operation.Date)))
                    continue;

                periods.Add(operation.Date);
            }

            return periods;
        }
        #endregion

        public static IEnumerable<Conversion> ByAccount(this IEnumerable<Conversion> conversions, CashAccount account)
        {
            return conversions.Where(x => x.Account.Equals(account));
        }

        public static IEnumerable<OperationCurrency> ByAccount(this IEnumerable<OperationCurrency> cash, CashAccount account)
        {
            return cash.Where(x => x.Account.Equals(account));
        }

        public static IEnumerable<OperationCurrency> ByName(this IEnumerable<OperationCurrency> cash, CurrencyName name, CashAccount account)
        {
            return cash.Where(x => x.Name.Equals(name) && x.Account.Equals(account));
        }

        public static IEnumerable<OperationCategory> ByType(this IEnumerable<OperationCategory> categories, OperationType type)
        {
            return categories.Where(x => x.Type.Equals(type));
        }

        public static bool CompareDatesByMonth(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month;
        }
    }
}
