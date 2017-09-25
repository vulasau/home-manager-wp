using HomeManager.Entities.Enums;
using HomeManager.Statistics.Entities;
using HomeManager.Statistics.Interfaces;
using HomeManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        #region Dependencies
        private readonly IStatisticsManager _statisticsManager;
        #endregion

        #region Private fields
        public DateTime _period { get; private set; }
        #endregion

        #region Public fields
        public string Period
        {
            get
            {
                return _period != default(DateTime) ?
                    _period.ToShortDateString() :
                    Resources.AppResources.AllTime;
            }
        }

        public bool CanMoveBack { get; private set; }
        public bool CanMoveNext { get; private set; }

        public bool HasData { get { return Statistics.Any(); } }
        public bool NoData { get { return !HasData; } }
        public IEnumerable<CategoryStatistics> Statistics { get; private set; }
        #endregion

        public StatisticsViewModel()
        {
            _statisticsManager = App.Container.GetInstance<IStatisticsManager>();

            OnCurrent();
        }

        #region UI event handlers
        public void OnAllTime()
        {
            _period = default(DateTime);
            CanMoveBack = false;
            CanMoveNext = false;
            Statistics = GetStatistics();

            OnPropertyChanged("Period", "Statistics", "CanMoveBack", "CanMoveNext");
        }

        public void OnBack()
        {
            UpdateInfo(_period.AddMonths(-1));
        }

        public void OnCurrent()
        {
            UpdateInfo(DateTime.Now);

            if (!Statistics.Any() && CanMoveBack)
            {
                OnBack();
            }
        }

        public void OnNext()
        {
            UpdateInfo(_period.AddMonths(1));
        }
        #endregion

        #region Private Methods
        private void UpdateInfo(DateTime period)
        {
            _period = period;
            CanMoveBack = GetStatistics(_period.AddMonths(-1)).Any();
            CanMoveNext = GetStatistics(_period.AddMonths(1)).Any();
            Statistics = GetStatistics(_period);

            OnPropertyChanged("Period", "Statistics", "NoData", "HasData", "CanMoveBack", "CanMoveNext");
        }

        private IEnumerable<CategoryStatistics> GetStatistics(DateTime period = default(DateTime))
        {
            return _statisticsManager.GetCategoryStatistics(_dataContext.Operations.Collection, OperationType.Expense, SelectedAccount, period);
        }
        #endregion
    }
}
