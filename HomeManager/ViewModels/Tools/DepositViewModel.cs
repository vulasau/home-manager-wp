using HomeManager.Tools;
using HomeManager.Tools.Entities;
using HomeManager.ViewModels.Base;

namespace HomeManager.ViewModels.Tools
{
    public class DepositViewModel : ViewModelBase
    {
        #region Dependencies
        private DepositCalculator _calculator;
        #endregion

        #region Public fields
        private double _deposit;
        public double Deposit
        {
            get { return _deposit; }
            set
            {
                _deposit = value;
                OnPropertyChanged("Ready");
            }
        }

        private double _depositMonth;
        public double DepositMonth
        {
            get { return _depositMonth; }
            set
            {
                _depositMonth = value;
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

        private int _period;
        public int Period
        {
            get { return _period; }
            set
            {
                _period = value;
                OnPropertyChanged("Ready");
            }
        }

        public bool Ready
        {
            get 
            { 
                bool ready = Deposit > 0 && Percent > 0 && Period > 0;
                
                if(ready)
                    OnPropertyChanged("Info");
                return ready;

            }
        }

        public DepositInfo Info { get { return _calculator.CalculateDeposit(Deposit, DepositMonth, Percent, Period); } }
        #endregion

        public DepositViewModel()
        {
            _calculator = App.Container.GetInstance<DepositCalculator>();
        }
    }
}
