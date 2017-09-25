using HomeManager.Rates.Entities;
using HomeManager.Rates.Interfaces;
using HomeManager.ViewModels.Base;
using System.Linq;
using System.Collections.Generic;

namespace HomeManager.ViewModels
{
    public class RatesViewModel: AsyncViewModel
    {
        #region Dependencies
        private IRatesService _ratesService;
        #endregion

        #region Public fields
        public string BaseCurrencyUid { get { return _options.DefaultCurrency.ToString(); } }
        public IEnumerable<CurrencyRate> Rates { get; private set; }
        public bool NoData { get; set; }
        #endregion

        public RatesViewModel()
            : base()
        {
            _ratesService = App.Container.GetInstance<IRatesService>();
            _ratesService.Updated += OnRatesloaded;
            _ratesService.Failed += OnLoadFailed;
            

            OnStarted(Resources.AppResources.RatesProgressMessage);
            _ratesService.UpdateAsync(_options.DefaultCurrency.ToString());
        }

        #region Rates service event handlers
        private void OnRatesloaded(object sender, Rates.Events.RatesEventArgs e)
        {
            Rates = e.Rates;
            NoData = !e.Rates.Any();
            OnPropertyChanged("Rates", "NoData");
            OnCompleted();
        }

        private void OnLoadFailed(object sender, int e)
        {
            if (e == 0)
                OnFailed(Resources.AppResources.RatesWebErrorMessage);
            else if (e == 1)
                OnFailed(Resources.AppResources.RatesFormatErrorMessage);
        }
        #endregion
    }
}
