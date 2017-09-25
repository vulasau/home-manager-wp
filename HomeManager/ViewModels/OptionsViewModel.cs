using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Extensions;
using HomeManager.Infrastructure.Enums;
using HomeManager.Rates.Interfaces;
using HomeManager.Services.Interfaces;
using HomeManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class OptionsViewModel: NavigationViewModel
    {
        #region Dependencies
        private IIconsService _iconsService;
        private IRatesService _ratesService;
        #endregion

        #region Public Fields
        public QuickAccessViewModel<Icon, string> QuickExpense { get; private set; }
        public QuickAccessViewModel<Icon, string> QuickIncome { get; private set; }

        public OperationCategory SelectedExpenseCategory { get; set; }
        public OperationCategory SelectedIncomeCategory { get; set; }

        public IEnumerable<Icon> Icons { get { return _iconsService.Icons; } }
        public IEnumerable<CurrencyName> Currencies { get { return Enum.GetValues(typeof(CurrencyName)).Cast<CurrencyName>(); } }
        public IEnumerable<CategorySortOrder> SortOrders { get { return Enum.GetValues(typeof(CategorySortOrder)).Cast<CategorySortOrder>(); } }
        public IEnumerable<OperationFilter> OperationFilters { get { return Enum.GetValues(typeof(OperationFilter)).Cast<OperationFilter>(); } }
        public IEnumerable<OperationCategory> Categories { get { return _dataContext.Categories.Collection.Order(_options.DefaultSortOrder); } }
        #endregion

        #region Options Fields
        public CurrencyName DefaultCurrency
        {
            get { return _options.DefaultCurrency; }
            set { _options.DefaultCurrency = value; }
        }
        public CategorySortOrder DefaultSortOrder
        {
            get { return _options.DefaultSortOrder; }
            set { _options.DefaultSortOrder = value; }
        }

        public OperationFilter DefaultOperationFilter
        {
            get { return _options.DefaultOperationsFilter; }
            set { _options.DefaultOperationsFilter = value; }
        }

        public bool LoadRates
        {
            get { return _options.LoadRates; }
            set { _options.LoadRates = value; }
        }
        #endregion

        #region Constructors
        public OptionsViewModel()
        {
            _iconsService = App.Container.GetInstance<IIconsService>();
            _ratesService = App.Container.GetInstance<IRatesService>();
            _dataContext.Categories.Collection.CollectionChanged += CategoriesCollectionChanged;
            _dataContext.Loaded += DataContextUpdated;

            InitializeQuickAccess();
        }
        #endregion

        #region UI event handlers
        void CategoriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Categories");
        }

        void OnAddExpenseCategory(object sender, Base.Events.QuickAccessEventArgs<Icon, string> e)
        {
            _dataContext.Categories.Add(new OperationCategory()
            {
                Name = e.Value,
                Type = OperationType.Expense,
                IconSource = e.Selected
            });
        }

        void OnAddIncomeCategory(object sender, Base.Events.QuickAccessEventArgs<Icon, string> e)
        {
            _dataContext.Categories.Add(new OperationCategory()
            {
                Name = e.Value,
                Type = OperationType.Income,
                IconSource = e.Selected
            });
        }

        public void RemoveExpenseCategory()
        {
            int operationsCount = _dataContext.Operations.Collection.Count(x => x.Category.Equals(SelectedExpenseCategory));
            if (operationsCount == 0)
                _dataContext.Categories.Remove(SelectedExpenseCategory);
            else
                Removing(SelectedExpenseCategory, operationsCount);
        }

        public void RemoveIncomeCategory()
        {
            int operationsCount = _dataContext.Operations.Collection.Count(x => x.Category.Equals(SelectedIncomeCategory));
            if (operationsCount == 0)
                _dataContext.Categories.Remove(SelectedIncomeCategory);
            else
                Removing(SelectedIncomeCategory, operationsCount);
        }

        public void KeepRemoving(OperationCategory category)
        {
            _dataContext.Operations.RemoveRange(_dataContext.Operations.Collection.Where(x => x.Category.Equals(category)));
            _dataContext.Categories.Remove(category);
        }

        public void ClearDatabase()
        {
            _dataContext.Clear();
            _ratesService.Clear();
            
        }
        #endregion

        #region Helper methods
        private void InitializeQuickAccess()
        {
            QuickExpense = new QuickAccessViewModel<Icon, string>(Icons);
            QuickExpense.AddInvoked += OnAddExpenseCategory;
            QuickIncome = new QuickAccessViewModel<Icon, string>(Icons);
            QuickIncome.AddInvoked += OnAddIncomeCategory;
        }
        #endregion

        #region DataContext event handlers
        private void DataContextUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged("Categories");
        }
        #endregion

        #region Events
        public delegate void RemoveCategoryEventHandler(OperationCategory category, int operationsCount);

        public event RemoveCategoryEventHandler Removing;

        protected void OnRemoving(OperationCategory category, int operationsCount)
        {
            if (Removing != null)
                Removing(category, operationsCount);
        }
        #endregion
    }
}
