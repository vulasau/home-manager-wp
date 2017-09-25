using HomeManager.Statistics.Interfaces;
using HomeManager.Tools;
using HomeManager.Tools.Entities;
using HomeManager.ViewModels.Base;

namespace HomeManager.ViewModels.Tools
{
    public class CreditViewModel: ViewModelBase
    {
        #region Dependencies
        private IStatisticsManager _statistics;
        private CreditCalculator _calculator;
        #endregion

        #region Public fields
        private double _amount;
        public double Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Ready");
            }
        }

        private double _firstPayment;
        public double FirstPayment
        {
            get { return _firstPayment; }
            set
            {
                _firstPayment = value;
                OnPropertyChanged("Ready");
            }
        }

        private double _percent;
        public double Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                OnPropertyChanged("Ready");
            }
        }

        private int _periodMonth;
        public int PeriodMonth
        {
            get { return _periodMonth; }
            set
            {
                _periodMonth = value;
                OnPropertyChanged("Ready");
            }
        }

        public CreditInfo Info
        {
            get
            {
                double monthEarnings = _statistics.GetMonthEarnings(_dataContext.Operations.Collection, _options.DefaultCurrency);
                return _calculator.Calculate(Amount, FirstPayment, Percent, PeriodMonth, monthEarnings);
            }
        }

        public bool Ready
        {
            get 
            { 
                bool ready = Amount > 0 && Percent > 0 && PeriodMonth > 0;
                
                if(ready)
                    OnPropertyChanged("Info");

                return ready;
            }
        }
        #endregion

        public CreditViewModel()
        {
            _statistics = App.Container.GetInstance<IStatisticsManager>();
            _calculator = App.Container.GetInstance<CreditCalculator>();
        }
    }
}
