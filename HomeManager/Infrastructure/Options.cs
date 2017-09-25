using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Infrastructure.Enums;
using HomeManager.PhoneMemoryAccess.Interfaces;
using System.ComponentModel;

namespace HomeManager.Infrastructure
{
    public class Options: INotifyPropertyChanged
    {
        #region Dependencies
        private IPhoneMemoryService _memoryService;
        #endregion

        #region Const fields
        private const string _defaultCurrencyKey = "defaultCurrency";
        private const string _defaultSortOrderKey = "defaultSortOrder";
        private const string _defaultOperationsFilterKey = "operationsFilter";
        private const string _loadRatesKey = "loadRates";
        private const string _skyDriveAutoUploadKey = "skyDriveAutoUpload";
        private const string _protectedKey = "protected";
        private const string _passwordKey = "password";
        private const string _questionKey = "question";
        private const string _answerKey = "answer";
        #endregion

        #region Public fields
        private CashAccount _account;
        public CashAccount Account
        {
            get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged("Account");
            }
        }

        private CurrencyName _defaultCurrency;
        public CurrencyName DefaultCurrency
        {
            get { return _defaultCurrency; }
            set
            {
                _defaultCurrency = value;
                OnPropertyChanged("DefaultCurrency");
            }
        }

        private CategorySortOrder _defaultSortOrder;
        public CategorySortOrder DefaultSortOrder
        {
            get { return _defaultSortOrder; }
            set
            {
                _defaultSortOrder = value;
                OnPropertyChanged("DefaultSortOrder");
            }
        }

        private OperationFilter _defaultOperationFiler;
        public OperationFilter DefaultOperationsFilter
        {
            get { return _defaultOperationFiler; }
            set
            {
                _defaultOperationFiler = value;
                OnPropertyChanged("DefaultOperationsFilter");
            }
        }

        private bool _loadRates;
        public bool LoadRates
        {
            get { return _loadRates; }
            set
            {
                _loadRates = value;
                OnPropertyChanged("LoadRates");
            }
        }

        private bool _skyDriveAutoUpload;
        public bool SkyDriveAutoUpload
        {
            get { return _skyDriveAutoUpload; }
            set
            {
                _skyDriveAutoUpload = value;
                OnPropertyChanged("SkyDriveAutoUpload");
            }
        }

        //Protection fields
        private bool _protected;
        public bool Protected
        {
            get { return _protected; }
            set
            {
                _protected = value;
                OnPropertyChanged("Protected");
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _question;
        public string Question
        {
            get { return _question; }
            set
            {
                _question = value;
                OnPropertyChanged("Question");
            }
        }

        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                OnPropertyChanged("Answer");
            }
        }
        #endregion

        public Options(IPhoneMemoryService memoryService)
        {
            _memoryService = memoryService;

            DefaultCurrency = CurrencyName.BYR;
            DefaultSortOrder = CategorySortOrder.Usage;
            DefaultOperationsFilter = OperationFilter.Month;
            Account = null;
        }

        #region Save/Load
        public void Save()
        {
            _memoryService.SaveValue(_defaultCurrencyKey, DefaultCurrency);
            _memoryService.SaveValue(_defaultSortOrderKey, DefaultSortOrder);
            _memoryService.SaveValue(_defaultOperationsFilterKey, DefaultOperationsFilter);
            _memoryService.SaveValue(_loadRatesKey, LoadRates);
            _memoryService.SaveValue(_skyDriveAutoUploadKey, SkyDriveAutoUpload);

            _memoryService.SaveValue(_protectedKey, _protected);
            _memoryService.SaveValue(_passwordKey, _password);
            _memoryService.SaveValue(_questionKey, _question);
            _memoryService.SaveValue(_answerKey, _answer);
        }

        public void Load()
        {
            DefaultCurrency = _memoryService.LoadValue<CurrencyName>(_defaultCurrencyKey);
            DefaultSortOrder = _memoryService.LoadValue<CategorySortOrder>(_defaultSortOrderKey);
            DefaultOperationsFilter = _memoryService.LoadValue<OperationFilter>(_defaultOperationsFilterKey);
            LoadRates = _memoryService.LoadValue<bool>(_loadRatesKey);
            SkyDriveAutoUpload = _memoryService.LoadValue<bool>(_skyDriveAutoUploadKey);

            Protected = _memoryService.LoadValue<bool>(_protectedKey);
            Password = _memoryService.LoadValue<string>(_passwordKey);
            Question = _memoryService.LoadValue<string>(_questionKey);
            Answer = _memoryService.LoadValue<string>(_answerKey);
        }
        #endregion

        #region Property changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
