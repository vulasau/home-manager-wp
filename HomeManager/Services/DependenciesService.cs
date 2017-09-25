using HomeManager.DataAccess.Interfaces;
using HomeManager.Services.Interfaces;
using System;

namespace HomeManager.Services
{
    public class DependenciesService
    {
        #region Dependencies
        IDataContext _dataContext;
        IIconsService _iconsService;
        #endregion

        #region Private fields
        private bool _dataLoaded;
        private bool _iconsLoaded;
        #endregion

        public DependenciesService(IDataContext dataContext, IIconsService iconsService)
        {
            _dataContext = dataContext;
            _iconsService = iconsService;

            _dataContext.Loaded += DataContextLoaded;
            _iconsService.Loaded += IconsLoaded;
        }

        private void Update()
        {
            foreach (var category in _dataContext.Categories.Collection)
            {
                category.IconSource = _iconsService.Get(category.IconSource.Id);
            }

            foreach (var operation in _dataContext.Operations.Collection)
            {
                operation.Category = _dataContext.Categories.Get(operation.Category.Id);
            }

            _dataLoaded = false;
        }

        #region Event handlers
        private void DataContextLoaded(object sender, EventArgs e)
        {
            _dataLoaded = true;

            if(_iconsLoaded)
                Update();
        }

        private void IconsLoaded(object sender, EventArgs e)
        {
            _iconsLoaded = true;

            if(_dataLoaded)
                Update();
        }
        #endregion
    }
}
