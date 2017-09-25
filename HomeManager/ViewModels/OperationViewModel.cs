using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Entities.Extensions;
using HomeManager.Extensions;
using HomeManager.Rates.Interfaces;
using HomeManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HomeManager.ViewModels
{
    public class OperationViewModel : AsyncEntityViewModel<Operation>
    {
        private bool _buying;

        #region Dependencies
        IRatesService _ratesService;
        #endregion

        #region Public fields
        public bool IsExpense
        {
            get { return State.Type == OperationType.Expense; }
            set { State.Type = value ? OperationType.Expense : OperationType.Income; OnPropertyChanged("Categories"); }
        }

        public bool IsIncome
        {
            get { return State.Type == OperationType.Income; }
            set { State.Type = value ? OperationType.Income : OperationType.Expense; OnPropertyChanged("Categories"); }
        }

        public override bool Ready
        {
            get
            {
                if (Convert)
                    return State.Amount > 0 && State.Category != null && Rate > 0;
                return State.Amount > 0 && State.Category != null;
            }
        }

        public string BaseCurrency { get { return _options.DefaultCurrency.ToString(); } }

        public double Amount
        {
            get
            {
                double amount = 0;
                if (_buying)
                {
                    if (Rate > 1)
                        amount = State.Amount / Rate;
                    else if (Rate >= 0 && Rate <= 1)
                        amount = State.Amount * Rate;
                }
                else
                {
                    if (Rate > 1)
                        amount = State.Amount * Rate;
                    else if (Rate >= 0 && Rate <= 1)
                        amount = State.Amount / Rate;
                }
                return Math.Round(amount, 2);
            }
        }

        public bool CanConvert { get { return State.Currency != _options.DefaultCurrency; } }

        private double _rate;
        public double Rate
        { 
            get { return _rate; }
            set
            {
                _rate = value;
                OnPropertyChanged("Rate", "Amount", "Ready");
            }
        }

        private bool _convert;
        public bool Convert
        {
            get { return _convert; }
            set
            {
                _convert = value;
                OnPropertyChanged("Convert");

                if (value && _options.LoadRates)
                    UpdateRate();
            }
        }

        public IEnumerable<OperationCategory> Categories { 
            get 
            { 
                return _dataContext.Categories.Collection
                .ByType(IsExpense ? OperationType.Expense : OperationType.Income)
                .Order(_options.DefaultSortOrder); 
            } 
        }
        
        public IEnumerable<CurrencyName> Currencies { get { return Enum.GetValues(typeof(CurrencyName)).Cast<CurrencyName>(); } }
        #endregion

        #region Constructors
        public OperationViewModel(OperationType type)
            : base()
        {
            State.Date = DateTime.Now;
            State.Type = type;
            State.Currency = _options.DefaultCurrency;
            State.Category = Categories.FirstOrDefault();
            State.Account = SelectedAccount;
        }

        public OperationViewModel(Operation operation)
            : base(operation)
        {
        }
        #endregion

        #region UI event handlers
        public override void OnSave()
        {
            if (Convert)
            {
                State.Amount = Amount;
                State.Currency = _options.DefaultCurrency;
            }

            if (EditMode)
                _dataContext.Operations.Update(_entity, State);
            else
                _dataContext.Operations.Add(State);
        }

        public override void OnRemove()
        {
            _dataContext.Operations.Remove(_entity);
        }
        #endregion

        #region Helpers
        protected override void InitializeDependencies()
        {
            base.InitializeDependencies();
            _ratesService = App.Container.GetInstance<IRatesService>();
            _ratesService.Updated += RatesUpdated;
            _ratesService.Failed += RatesFailed;
        }

        protected override void InitializeState()
        {
            State = new Operation();
        }

        protected override void OnStatePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnStatePropertyChanged(sender, e);

            if (e.PropertyName.Equals("Currency"))
                OnPropertyChanged("CanConvert");

        }

        private void UpdateRate()
        {
            OnStarted(Resources.AppResources.RatesProgressMessage);
            _ratesService.UpdateAsync(_options.DefaultCurrency.ToString());
        }
        #endregion

        #region Event handlers
        private void RatesUpdated(object sender, Rates.Events.RatesEventArgs e)
        {
            var rates = e.Rates;
            var rate = rates.FirstOrDefault(x => x.Uid.Equals(State.Currency.ToString()));
            if (rate != null)
                Rate = rate.SellRate > rate.BuyRate? rate.SellRate : rate.BuyRate;

            _buying = rate.BuyRate > rate.SellRate;
            
            OnCompleted();
        }

        private void RatesFailed(object sender, int e)
        {
            if (e == 0)
                OnFailed(Resources.AppResources.RatesWebErrorMessage);
            else if (e == 1)
                OnFailed(Resources.AppResources.RatesFormatErrorMessage);
        }
        #endregion
    }
}
