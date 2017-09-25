using HomeManager.Rates.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using HomeManager.PhoneMemoryAccess.Interfaces;
using System.Collections.ObjectModel;
using HomeManager.Rates.Events;
using Microsoft.Phone.Net.NetworkInformation;
using HomeManager.Entities.Enums;
using HomeManager.Rates.Entities;
using System.Threading.Tasks;
using System.Net;

namespace HomeManager.Rates
{
    public class RatesService: IRatesService
    {
        #region Dependencies
        private IPhoneMemoryService _memoryService;
        private IRatesWebClient _client;
        private IRatesHtmlParser _parser;
        #endregion

        #region Private Fields
        private const string _lastUpdateKey = "lastUpdate";
        private const string _lastCurrencyKey = "lastCurrency";
        private const string _fileName = "Rates.xml";

        private bool _loading;
        private string _lastCurrency;
        private DateTime _lastUpdate;
        private IEnumerable<string> _supportedCurrencies;
        private ObservableCollection<CurrencyRate> _rates;
        #endregion

        public RatesService(IPhoneMemoryService memoryService, RatesHtmlParser parser, RatesWebClient client)
        {
            _supportedCurrencies = Enum.GetNames(typeof(CurrencyName));
            _rates = new ObservableCollection<CurrencyRate>();

            _memoryService = memoryService;
            _parser = parser;
            _client = client;
            _client.HtmlLoaded += HtmlLoaded;
        }

        #region Public methods
        public void UpdateAsync(string currencyUid)
        {
            if (UpToDate(currencyUid))
            {
                OnUpdated(_rates);
            }
            else
            {
                if (InternetAvailable() && !_loading)
                    LoadHtmlAsync(currencyUid);
                else
                    OnUpdated(_rates);
            }
        }
        #endregion

        #region Save/Load
        public void Load()
        {
            _rates = _memoryService.LoadCollection<CurrencyRate>(_fileName);
            _lastUpdate = _memoryService.LoadValue<DateTime>(_lastUpdateKey);
            _lastCurrency = _memoryService.LoadValue<string>(_lastCurrencyKey);

            OnLoaded();
        }

        public void Save()
        {
            _memoryService.SaveCollection<CurrencyRate>(_rates, _fileName);
            _memoryService.SaveValue(_lastUpdateKey, _lastUpdate);
            _memoryService.SaveValue(_lastCurrencyKey, _lastCurrency);
        }

        public async void LoadAsync()
        {
            await Task.Run(() => Load());
        }

        public async void SaveAsync()
        {
            await Task.Run(() => Save());
        }

        public void Clear()
        {
            _rates.Clear();
            _lastCurrency = string.Empty;
            _lastUpdate = default(DateTime);
            _memoryService.DeleteFile(_fileName);
        }
        #endregion

        #region Private methods
        private void LoadHtmlAsync(string currencyUid)
        {
            try
            {
                _loading = true;
                _lastCurrency = currencyUid;
                _lastUpdate = DateTime.Now;
                _client.LoadHtmlAsync(currencyUid);
            }
            catch (WebException ex)
            {
                OnFailed(0);
            }
        }

        private bool UpToDate(string currencyUid)
        {
            return CheckDate(_lastUpdate)
                && CompareUids(_lastCurrency, currencyUid);
        }

        private bool InternetAvailable()
        {
            return NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None;
        }

        private bool CompareUids(string last, string current)
        {
            if (string.IsNullOrEmpty(last) || string.IsNullOrEmpty(current))
                return false;

            return string.Equals(last, current, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool CheckDate(DateTime date)
        {
            if (date == default(DateTime))
                return false;

            return (DateTime.Now.Year == date.Year
                && DateTime.Now.DayOfYear == date.DayOfYear);

        }
        #endregion

        #region Web client event handlers
        private void HtmlLoaded(object sender, string e)
        {
            try
            {
                var parsed = _parser.Parse(e);
                var filtered = parsed.Where(rate => !CompareUids(rate.Uid, _lastCurrency)
                && _supportedCurrencies.Any(cy => CompareUids(rate.Uid, cy)));

                _rates = new ObservableCollection<CurrencyRate>(filtered);
                _loading = false;

                OnUpdated(_rates);
            }
            catch(FormatException ex)
            {
                OnFailed(1);
            }
        }
        #endregion

        #region Events
        public event EventHandler<RatesEventArgs> Updated;
        public event EventHandler Loaded;
        public event EventHandler<int> Failed;

        protected void OnUpdated(IEnumerable<CurrencyRate> rates)
        {
            if (Updated != null)
                Updated(this, new RatesEventArgs(rates));
        }

        protected void OnLoaded()
        {
            if (Loaded != null)
                Loaded(this, new EventArgs());
        }

        protected void OnFailed(int code)
        {
            if (Failed != null)
                Failed(this, code);
        }
        #endregion
    }
}
