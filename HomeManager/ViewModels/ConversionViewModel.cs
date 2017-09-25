using HomeManager.Cash.Entities;
using HomeManager.Cash.Interfaces;
using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class ConversionViewModel: EntityViewModel<Conversion>
    {
        #region Dependencies
        ICashService _cashService;
        #endregion

        #region Public fields
        public bool Buying
        {
            get { return State.Type == ConversionType.Buy ? true : false; }
            set { State.Type = value ? ConversionType.Buy : ConversionType.Sell; }
                
        }

        public bool Selling
        {
            get { return State.Type == ConversionType.Sell ? true : false; }
            set { State.Type = value ? ConversionType.Sell : ConversionType.Buy; }
        }

        public ConversionPreview Preview
        {
            get
            {
                return _cashService.PreviewConversion(new Conversion()
                {
                    Date = State.Date,
                    Type = State.Type,
                    Amount = State.Amount,
                    Rate = State.Rate,
                    From = State.From,
                    To = State.To
                });
            }
        }

        public override bool Ready
        {
            get
            {
                return State.Amount > 0
                    && State.Rate > 0
                    && State.From != State.To;
            }
        }

        public IEnumerable<CurrencyName> Currencies
        {
            get { return Enum.GetValues(typeof(CurrencyName)).Cast<CurrencyName>(); }
        }
        #endregion

        #region Constructors
        public ConversionViewModel()
            : base()
        {

        }

        public ConversionViewModel(Conversion conversion)
            : base(conversion)
        {

        }
        #endregion

        #region UI event handlers
        public override void OnSave()
        {
            if (EditMode)
                _dataContext.Conversions.Update(_entity ,State);
            else
                _dataContext.Conversions.Add(State);
        }

        public override void OnRemove()
        {
            _dataContext.Conversions.Remove(_entity);
        }
        #endregion

        #region Helper methods
        protected override void InitializeState()
        {
            State = new Conversion();
            State.Account = SelectedAccount;
            State.From = _options.DefaultCurrency;
            State.To = _options.DefaultCurrency;
        }

        protected override void InitializeDependencies()
        {
            base.InitializeDependencies();

            _cashService = App.Container.GetInstance<ICashService>();
        }

        protected override void OnStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnStatePropertyChanged(sender, e);

            OnPropertyChanged("Preview");
        }
        #endregion
    }
}
