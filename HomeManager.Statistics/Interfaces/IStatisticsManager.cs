using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Statistics.Entities;
using System;
using System.Collections.Generic;

namespace HomeManager.Statistics.Interfaces
{
    public interface IStatisticsManager
    {
        IEnumerable<CategoryInfo> GetCategoryInfo(IEnumerable<Operation> operations, OperationCategory category, CashAccount account);
        IEnumerable<CategoryStatistics> GetCategoryStatistics(IEnumerable<Operation> operations, OperationType type, CashAccount account, DateTime period = default(DateTime));

        IEnumerable<CashInfo> Total(IEnumerable<Operation> operations, OperationType type, CashAccount account);
        IEnumerable<CashInfo> TotalWeek(IEnumerable<Operation> operations, OperationType type, CashAccount account);
        IEnumerable<CashInfo> TotalMonth(IEnumerable<Operation> operations, OperationType type, CashAccount account);

        double GetMonthEarnings(IEnumerable<Operation> operations, CurrencyName currency);

        LimitInfo GetLimitInfo(IEnumerable<Operation> operations, CurrencyName currency, CashAccount account);
        LimitInfo GetLimitInfo(OperationCategory category, IEnumerable<Operation> operations, CurrencyName currency, CashAccount account);
        IEnumerable<LimitInfo> GetCriticalLimits(IEnumerable<Operation> operations, CurrencyName currency, CashAccount account);

        Dictionary<CurrencyName, double> GetMonthlyBudget(DateTime period, IEnumerable<Operation> operations);
        IEnumerable<BudgetSummary> GetBudgetStatistics(IEnumerable<Operation> operations);
    }
}
