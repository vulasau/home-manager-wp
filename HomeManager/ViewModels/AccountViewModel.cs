using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Statistics.Entities;
using HomeManager.Statistics.Interfaces;
using HomeManager.ViewModels.Base;
using System;
using System.ComponentModel;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class AccountViewModel: EntityViewModel<CashAccount>
    {

        #region Dependencies
        private IStatisticsManager _statistics;
        #endregion

        #region Public fields
        public CurrencyName DefaultCurrency { get { return _options.DefaultCurrency; } }
        public LimitInfo Limit { get { return _statistics.GetLimitInfo(_dataContext.Operations.Collection, _options.DefaultCurrency, State); } }
        
        public bool CanRemove { get { return State.Id != 0; } }

        public override bool Ready
        {
            get
            {
                bool limitCorrect = State.Limited ? State.Limit > 0 : true;
                return limitCorrect && !string.IsNullOrEmpty(State.Name) && !NameError;
            }
        }

        public bool NameError
        {
            get
            {
                return _dataContext.Accounts.Collection.Any(x => x.Name.Equals(State.Name))
                    && !State.Name.Equals(_entity.Name);
            }
        }
        #endregion

        #region Constructors
        public AccountViewModel(CashAccount account)
            : base(account)
        {
        }

        #endregion

        #region UI event handlers
        public override void OnSave()
        {
            if (EditMode)
                _dataContext.Accounts.Update(_entity, State);
        }

        public override void OnRemove()
        {
            
        }

        public bool TryRemove()
        {
            if (!CanRemove)
                return false;

            int operations = _dataContext.Operations.Collection.Count(x => x.Account.Equals(State));
            if (operations == 0)
            {
                _dataContext.Accounts.Remove(_entity);
                return true;
            }
            else
            {
                OnRemoving(State, operations);
                return false;
            }
        }

        public void KeepRemoving(CashAccount account)
        {
            _dataContext.Operations.RemoveRange(_dataContext.Operations.Where(x => x.Account.Equals(account)));
            _dataContext.Accounts.Remove(account);
        }
        #endregion

        #region State event handlers
        protected override void OnStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnStatePropertyChanged(sender, e);

            if (e.PropertyName.Equals("Name"))
                OnPropertyChanged("NameError");

            if (e.PropertyName.Equals("Limit"))
                OnPropertyChanged("Limit");
        } 
        #endregion

        #region Helper methods
        protected override void InitializeDependencies()
        {
            base.InitializeDependencies();

            _statistics = App.Container.GetInstance<IStatisticsManager>();
        }

        protected override void InitializeState()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Events
        public delegate void RemoveAccountEventHandler(CashAccount account, int operationsCount);

        public event RemoveAccountEventHandler Removing;

        protected void OnRemoving(CashAccount account, int operationsCount)
        {
            if (Removing != null)
                Removing(account, operationsCount);
        }
        #endregion
    }
}
