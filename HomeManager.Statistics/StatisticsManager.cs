using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Entities.Extensions;
using HomeManager.Statistics.Entities;
using HomeManager.Statistics.Enums;
using HomeManager.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManager.Statistics
{
    public class StatisticsManager: IStatisticsManager
    {
        #region Category usage
        public IEnumerable<CategoryStatistics> GetCategoryStatistics(IEnumerable<Operation> operations, OperationType type, CashAccount account, DateTime period = default(DateTime))
        {
            var statistics = new List<CategoryStatistics>();
            var filtered = operations.ByType(account, type).ByMonth(period);
            var currencies = filtered.UsedCurrencies();

            foreach (var currency in currencies)
            {
                var byCurrency = filtered.ByCurrency(currency);
                if(byCurrency.Any())
                    statistics.Add(GetCategoryStatistics(byCurrency, currency));
            }
            return statistics;
        }

        private CategoryStatistics GetCategoryStatistics(IEnumerable<Operation> operations, CurrencyName currency)
        {
            var statistics = new CategoryStatistics();
            var info = new List<CategoryInfo>();
            var categories = operations.UsedCategories();

            foreach (var category in categories)
            {
                info.Add(GetCategoryInfo(operations, category, currency));
            }
            statistics.Currency = currency.ToString();
            statistics.Info = info.OrderByDescending(x => x.Persentage);
            return statistics;
        }

        public IEnumerable<CategoryInfo> GetCategoryInfo(IEnumerable<Operation> operations, OperationCategory category, CashAccount account)
        {
            var info = new List<CategoryInfo>();
            var usedCurrencies = operations.UsedCurrencies();
            foreach (var currency in usedCurrencies)
            {
                var filtered = operations.ByAccount(account).ByCurrency(currency).CurrentMonth();
                if(filtered.Any())
                    info.Add(GetCategoryInfo(filtered, category, currency));
            }
            return info;
        }

        private CategoryInfo GetCategoryInfo(IEnumerable<Operation> operations, OperationCategory category, CurrencyName currency)
        {
            var categorized = operations.ByCategory(category);
            var amountTotal = operations.ByType(category.Type).Sum(x => x.Amount);
            var amountCategorized = categorized.Sum(x => x.Amount);
            var iterations = categorized.Count();
            var persentage = amountTotal > 0 ? amountCategorized * 100 / amountTotal : 0;
            
            return new CategoryInfo()
            {
                Name = category.Name,
                Amount = amountCategorized,
                Iterations = iterations,
                Persentage = Math.Round(persentage, 1),
                BaseCurrency = currency.ToString()
            };
        }
        #endregion

        #region Currency usage
        public IEnumerable<CashInfo> Total(IEnumerable<Operation> operations, OperationType type, CashAccount account)
        {
            var info = new List<CashInfo>();
            var usedCurrencies = operations.UsedCurrencies();
            foreach (var currency in usedCurrencies)
            {
                var filtered = operations.ByType(account, type).ByCurrency(currency);
                if (filtered.Any())
                    info.Add(GetOperationInfo(filtered, currency));
            }
            return info;
        }

        public IEnumerable<CashInfo> TotalWeek(IEnumerable<Operation> operations, OperationType type, CashAccount account)
        {
            var info = new List<CashInfo>();
            var usedCurrencies = operations.UsedCurrencies();
            foreach (var currency in usedCurrencies)
            {
                var filtered = operations.ByType(account, type).ByCurrency(currency);
                filtered = filtered.LastWeek();
                if (filtered.Any())
                    info.Add(GetOperationInfo(filtered, currency));
            }
            return info;
        }

        public IEnumerable<CashInfo> TotalMonth(IEnumerable<Operation> operations, OperationType type, CashAccount account)
        {
            var info = new List<CashInfo>();
            var usedCurrencies = operations.UsedCurrencies();
            foreach (var currency in usedCurrencies)
            {
                var filtered = operations.ByType(account, type).ByCurrency(currency);
                filtered = filtered.CurrentMonth();
                if (filtered.Any())
                    info.Add(GetOperationInfo(filtered, currency));
            }
            return info;
        }

        private CashInfo GetOperationInfo(IEnumerable<Operation> operations, CurrencyName currency)
        {
            var count = operations.ByCurrency(currency).Count();
            var amount = operations.Sum(x => x.Amount);
            return new CashInfo()
            {
                Iterations = count,
                Amount = amount,
                Currency = currency
            };
        }
        #endregion

        #region Earning
        public double GetMonthEarnings(IEnumerable<Operation> operations, CurrencyName currency)
        {
            var collection = operations.ByCurrency(currency);
            var periods = GetPeriods(collection);
            var earnings = new List<double>();
            
            foreach (var period in periods)
            {
                double income = collection.ByType(OperationType.Income).Sum(x => x.Amount);
                double expense = collection.ByType(OperationType.Expense).Sum(x => x.Amount);
                double earning = income - expense;
                earnings.Add(earning);
            }

            double average = earnings.Average();

            return average / earnings.Count();
        }

        private IEnumerable<KeyValuePair<int, int>> GetPeriods(IEnumerable<Operation> operations)
        {
            List<KeyValuePair<int, int>> periods = new List<KeyValuePair<int, int>>();
            foreach (var operation in operations)
            {
                var date = new KeyValuePair<int, int>(operation.Date.Year, operation.Date.Month);
                if (!periods.Contains(date))
                    periods.Add(date);
            }
            return periods;
        }
        #endregion

        #region Limits
        public IEnumerable<LimitInfo> GetCriticalLimits(IEnumerable<Operation> operations, CurrencyName currency, CashAccount account)
        {
            var criticals = new List<LimitInfo>();

            if (account.Limited)
            {
                var accountLimit = GetLimitInfo(operations, currency, account);
                if(accountLimit != null)
                    criticals.Add(accountLimit);
            }

            var categories = operations.UsedCategories().Where(x => x.Limited);
            foreach (var category in categories)
            {
                var info = GetLimitInfo(category, operations, currency, account);
                if(info != null)
                    if (info.SpeedDanger == SpeedDangerLevel.Red || info.Percentage >= 90)
                        criticals.Add(info);
            }
            //return criticals.Any() ? criticals : null;
            return criticals;
        }

        public LimitInfo GetLimitInfo(IEnumerable<Operation> operations, CurrencyName currency, CashAccount account)
        {
            if (!account.Limited || account.Limit == 0 || !operations.Any())
                return null;

            var date = DateTime.Now;

            var filtered = operations.ByType(account, OperationType.Expense).ByCurrency(currency).ByMonth(date);
            var info = CalculateLimitInfo(filtered, date, account.Limit, account.Name, currency);

            return info;
        }

        public LimitInfo GetLimitInfo(OperationCategory category, IEnumerable<Operation> operations, CurrencyName currency, CashAccount account)
        {
            if (category.Limited == false || category.Limit == 0 || !operations.Any())
                return null;

            var date = DateTime.Now;

            var filtered = operations.ByType(account, OperationType.Expense).ByCurrency(currency).ByCategory(category).ByMonth(date);
            var info = CalculateLimitInfo(filtered, date, category.Limit, category.Name, currency);

            return info;
        }

        private LimitInfo CalculateLimitInfo(IEnumerable<Operation> filteredOperations, DateTime date, double limit, string limitName, CurrencyName currency)
        {
            var totalExpense = filteredOperations.Sum(x => x.Amount);
            var actualSpeed = Normalize(filteredOperations).GroupBy(x => x.Date.Day).Average(group => group.Sum(operation => operation.Amount));
            var prognosedTotalExpense = actualSpeed * DateTime.DaysInMonth(date.Year, date.Month);
            var prognosedPercentage = prognosedTotalExpense * 100 / limit;

            var limitInfo = new LimitInfo();

            limitInfo.Limit = limit;
            limitInfo.AmountSpent = Math.Round(totalExpense, 2);
            limitInfo.Speed = Math.Round(actualSpeed, 2);
            limitInfo.Percentage = Math.Round(totalExpense * 100 / limit, 2);
            limitInfo.Currency = currency;
            limitInfo.Name = limitName;
            limitInfo.RunOutDays = Math.Round((limit - totalExpense) / actualSpeed, 0);

            if (prognosedPercentage <= 70)
                limitInfo.SpeedDanger = SpeedDangerLevel.Green;
            else if (prognosedPercentage > 70 && prognosedPercentage <= 90)
                limitInfo.SpeedDanger = SpeedDangerLevel.Orange;
            else
                limitInfo.SpeedDanger = SpeedDangerLevel.Red;

            return limitInfo;
        }

        private IEnumerable<Operation> Normalize(IEnumerable<Operation> operations)
        {
            var normalized = new List<Operation>();
            var existingDays = operations.GroupBy(x => x.Date.Day).Select(x => x.Key);

            var date = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            for (int i = 1; i <= date.Day; i++)
            {
                if (existingDays.Contains(i))
                    normalized.Add(operations.First(x => x.Date.Day == i));
                else
                {
                    var fakeDate = new DateTime(date.Year, date.Month, i);
                    normalized.Add(new Operation()
                    {
                        Date = fakeDate,
                        Amount = 0
                    });
                }
            }

            return normalized;

        }
        #endregion

        #region Budget
        public Dictionary<CurrencyName, double> GetMonthlyBudget(DateTime period, IEnumerable<Operation> operations)
        {
            if (!operations.Any())
                return null;

            var budget = new Dictionary<CurrencyName, double>();

            var filtered = operations.Where(x => x.Account.Id.Equals(0))
                .Where(x => x.Type == OperationType.Expense)
                .Where(x => x.Date.Year == period.Year && x.Date.Month == period.Month)
                .Where(x => x.Category.BudgetCategory);
            var currencies = filtered.UsedCurrencies();

            foreach (var curr in currencies)
            {
                var sum = filtered.Where(x => x.Currency == curr).Sum(x => x.Amount);
                if (sum > 0)
                    budget.Add(curr, sum);
            }

            return budget.Any()? budget : null;
        }

        public IEnumerable<BudgetSummary> GetBudgetStatistics(IEnumerable<Operation> operations)
        {
            var results = new List<BudgetSummary>();

            var filtered = operations.Where(x => x.Category.BudgetCategory);
            var currencies = filtered.UsedCurrencies();
            foreach (var curr in currencies)
            {
                var byCurrency = filtered.ByCurrency(curr);
                var periods = byCurrency.PeriodMonths();
                var amounts = new Dictionary<DateTime, double>();
                var items = new List<BudgetInfo>();

                foreach (var period in periods)
                {
                    var byDate = byCurrency.ByMonth(period);
                    var amount = byDate.Sum(x => x.Amount);
                    amounts.Add(period, amount);
                }

                var maxAmount = amounts.Max(x => x.Value);
                foreach (var amount in amounts)
                {
                    var percentage = 100 * amount.Value / maxAmount;
                    items.Add(new BudgetInfo(amount.Key, amount.Value, curr, percentage));
                }
                results.Add(new BudgetSummary(curr, items));
            }
           
            return results.Any() ? results : null;
        }
        #endregion
    }
}
