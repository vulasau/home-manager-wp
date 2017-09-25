using HomeManager.Entities.Enums;
using HomeManager.Statistics.Entities;
using HomeManager.Statistics.Interfaces;
using HomeManager.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class BudgetViewModel : ViewModelBase
    {
        private readonly IStatisticsManager _statisticsManager;

        public IEnumerable<BudgetSummary> Budget { get; private set; }

        public BudgetViewModel()
        {
            _statisticsManager = App.Container.GetInstance<IStatisticsManager>();

            Budget = _statisticsManager.GetBudgetStatistics(_dataContext.Operations.Collection.Where(x => x.Type == OperationType.Expense && x.Account.Id == _options.Account.Id));
        }
    }
}
