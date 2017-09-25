using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;
using HomeManager.Services.Interfaces;
using System;
using System.Linq;

namespace HomeManager.Services
{
    public class DefaultCategoriesInitializer
    {
        #region Dependencies
        IDataContext _dataContext;
        IIconsService _iconsService;
        #endregion

        #region Private fields
        private bool _dataLoaded;
        private bool _iconsLoaded;
        #endregion

        public DefaultCategoriesInitializer(IDataContext dataContext, IIconsService iconsService)
        {
            _dataContext = dataContext;
            _iconsService = iconsService;

            _dataContext.Loaded += DataContextLoaded;
            _iconsService.Loaded += IconsLoaded;
        }

        #region Helper methods
        private void Initialize()
        {
            InitializeIncomeCategories();
            InitializeExpenseCategories();
            _dataLoaded = false;
        }

        private void InitializeIncomeCategories()
        {
            if (_dataContext.Categories.Collection.Any(x => x.Type == Entities.Enums.OperationType.Income))
                return;

            var salaryIcon = _iconsService.Icons.First(x => x.Name.Equals("Briefcase"));

            var salaryCategory = new OperationCategory()
            {
                Name = Resources.AppResources.SalaryCategoryName,
                Type = Entities.Enums.OperationType.Income,
                IconSource = salaryIcon
            };

            _dataContext.Categories.Add(salaryCategory);
        }

        private void InitializeExpenseCategories()
        {
            if (_dataContext.Categories.Collection.Any(x => x.Type == Entities.Enums.OperationType.Expense))
                return;

            var foodIcon = _iconsService.Icons.First(x => x.Name.Equals("ForkAndKnife"));
            var transportIcon = _iconsService.Icons.First(x => x.Name.Equals("Taxi"));

            var foodCategory = new OperationCategory()
            {
                Name = Resources.AppResources.FoodCategoryName,
                Type = Entities.Enums.OperationType.Expense,
                IconSource = foodIcon
            };

            var transportCategory = new OperationCategory()
            {
                Name = Resources.AppResources.TransportCategoryName,
                Type = Entities.Enums.OperationType.Expense,
                IconSource = transportIcon
            };

            _dataContext.Categories.Add(foodCategory);
            _dataContext.Categories.Add(transportCategory);
        }
        #endregion

        #region Event handlers
        private void DataContextLoaded(object sender, EventArgs e)
        {
            _dataLoaded = true;

            if(_iconsLoaded)
                Initialize();
        }

        private void IconsLoaded(object sender, EventArgs e)
        {
            _iconsLoaded = true;

            if(_dataLoaded)
                Initialize();
        }
        #endregion
    }
}
