using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Entities.Extensions;
using HomeManager.Extensions;
using HomeManager.Statistics.Entities;
using HomeManager.Statistics.Interfaces;
using HomeManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HomeManager.ViewModels
{
    public class MainViewModel : NavigationViewModel
    {
        #region Dependencies
        private IStatisticsManager _statistics;
        #endregion

        #region Navigation
        protected override void SelectedTabChanged()
        {
            base.SelectedTabChanged();
            if (SelectedTabIndex == 3)
                OnPropertyChanged("MonthExpense", "MonthIncome", "Cash", "Limits", "Budget");
        }
        #endregion

        #region Balance fields
        public IEnumerable<CashInfo> MonthExpense
        {
            get { return _statistics.TotalMonth(_dataContext.Operations.Collection, OperationType.Expense, SelectedAccount); }
        }

        public IEnumerable<CashInfo> MonthIncome
        {
            get { return _statistics.TotalMonth(_dataContext.Operations.Collection, OperationType.Income, SelectedAccount); }
        }
        #endregion

        #region Search
        private string _expenseSearch;
        public string ExpenseSearch
        {
            get { return _expenseSearch; }
            set
            {
                _expenseSearch = value;
                OnPropertyChanged("Expenses");
            }
        }

        private string _incomeSearch;
        public string IncomeSearch
        {
            get { return _incomeSearch; }
            set
            {
                _incomeSearch = value;
                OnPropertyChanged("Incomes");
            }
        }
        #endregion

        #region Public fields
        public IEnumerable<Operation> Expenses { get { return _dataContext.Operations.Collection.Filter(SelectedAccount, OperationType.Expense, _options.DefaultOperationsFilter, ExpenseSearch); } }
        public IEnumerable<Operation> Incomes { get { return _dataContext.Operations.Collection.Filter(SelectedAccount, OperationType.Income, _options.DefaultOperationsFilter, IncomeSearch); } }
        public IEnumerable<Conversion> Conversions { get { return _dataContext.Conversions.Collection.ByAccount(SelectedAccount); } }
        public IEnumerable<OperationCurrency> Cash { get { return _dataContext.Cash.Collection.ByAccount(SelectedAccount); } }
        public IEnumerable<CashAccount> Accounts { get { return _dataContext.Accounts.Collection; } }
        public IEnumerable<LimitInfo> Limits { get { return _statistics.GetCriticalLimits(_dataContext.Operations.Collection, _options.DefaultCurrency, _options.Account); } }

        public QuickAccessViewModel<OperationCategory, double> QuickExpense { get; private set; }
        public QuickAccessViewModel<OperationCategory, double> QuickIncome { get; private set; }

        public Dictionary<CurrencyName, double> Budget { get { return _statistics.GetMonthlyBudget(DateTime.Now, _dataContext.Operations.Collection.ByAccount(_options.Account)); } }

        public Operation SelectedExpense { get; set; }
        public Operation SelectedIncome { get; set; }
        public Conversion SelectedConversion { get; set; }

        public string Date { get { return DateTime.Now.ToShortDateString(); } }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            _statistics = App.Container.GetInstance<IStatisticsManager>();
            _dataContext.Loaded += DataContextChanged;
            _dataContext.Operations.Collection.CollectionChanged += OperationsChanged;
            _dataContext.Accounts.Collection.CollectionChanged += AccountsChanged;
            _dataContext.Conversions.Collection.CollectionChanged += ConversionsChanged;
            _options.PropertyChanged += OnOptionsChanged;

            this.PropertyChanged += OnStateChanged;

            SelectedAccount.PropertyChanged += OnAccountLimitChanged;

            InitializeQuickAccess();
        }
        #endregion

        #region UI event handlers
        void OnQuickExpenseAdd(object sender, Base.Events.QuickAccessEventArgs<OperationCategory, double> e)
        {
            _dataContext.Operations.Add(new Operation()
            {
                Account = SelectedAccount,
                Date = DateTime.Now,
                Type = OperationType.Expense,
                Category = e.Selected,
                Currency = _options.DefaultCurrency,
                Amount = e.Value,
                Description = string.Empty
            });

            OnPropertyChanged("Expenses, Cash");
        }

        void OnQuickIncomeAdd(object sender, Base.Events.QuickAccessEventArgs<OperationCategory, double> e)
        {
            _dataContext.Operations.Add(new Operation()
            {
                Account = SelectedAccount,
                Date = DateTime.Now,
                Type = OperationType.Income,
                Category = e.Selected,
                Currency = _options.DefaultCurrency,
                Amount = e.Value,
                Description = string.Empty
            });

            OnPropertyChanged("Incomes, Cash");
        }

        public void OnRemoveConversion()
        {
            _dataContext.Conversions.Remove(SelectedConversion);
            OnPropertyChanged("Cash");
        }

        public void OnRemoveExpense()
        {
            _dataContext.Operations.Remove(SelectedExpense);
        }

        public void OnRemoveIncome()
        {
            _dataContext.Operations.Remove(SelectedIncome);
        }

        private void OperationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Expenses", "Incomes");
        }

        private void AccountsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Accounts");
        }

        private void ConversionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Conversions");
        }
        #endregion

        #region State event handlers
        private void DataContextChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Expenses", "Incomes", "Conversions", "Cash");
            InitializeQuickAccess();
        }

        private void OnOptionsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DefaultOperationsFilter", StringComparison.InvariantCultureIgnoreCase))
                OnPropertyChanged("Expenses", "Incomes");
        }

        private void OnStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedAccount"))
                OnPropertyChanged("Expenses", "Incomes", "Cash", "MonthIncome", "MonthExpense", "Conversions", "CurrentMonthBudget", "PreviousMonthBudget", "Budget");
        }

        private void OnAccountLimitChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Limit") || e.PropertyName.Equals("Limited"))
                OnPropertyChanged("Limits");
        }
        #endregion

        #region Private methods
        private void InitializeQuickAccess()
        {
            if (_options.DefaultSortOrder == Infrastructure.Enums.CategorySortOrder.Name)
            {
                QuickExpense = new QuickAccessViewModel<OperationCategory, double>(_dataContext.Categories.Collection, x => x.Type == OperationType.Expense, y => y.Name);
                QuickIncome = new QuickAccessViewModel<OperationCategory, double>(_dataContext.Categories.Collection, x => x.Type == OperationType.Income, y => y.Name);
            }
            else
            {
                QuickExpense = new QuickAccessViewModel<OperationCategory, double>(_dataContext.Categories.Collection, x => x.Type == OperationType.Expense, y => y.Usage);
                QuickIncome = new QuickAccessViewModel<OperationCategory, double>(_dataContext.Categories.Collection, x => x.Type == OperationType.Income, y => y.Usage);
            }
            QuickExpense.AddInvoked += OnQuickExpenseAdd;
            QuickIncome.AddInvoked += OnQuickIncomeAdd;

            OnPropertyChanged("QuickExpense", "QuickIncome");
        }
        #endregion
    }
}
