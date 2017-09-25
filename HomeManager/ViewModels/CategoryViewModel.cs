using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Services.Interfaces;
using HomeManager.Statistics.Entities;
using HomeManager.Statistics.Interfaces;
using HomeManager.ViewModels.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class CategoryViewModel: EntityViewModel<OperationCategory>
    {
        #region Dependencies
        private IIconsService _iconsService;
        private IStatisticsManager _statisticsManager;
        #endregion

        #region Public fields
        public LimitInfo Limit { get { return _statisticsManager.GetLimitInfo(State, _dataContext.Operations.Collection, _options.DefaultCurrency, _options.Account); } }

        public CurrencyName DefaultCurrency { get { return _options.DefaultCurrency; } }

        public IEnumerable<CategoryInfo> Info { get { return _statisticsManager.GetCategoryInfo(_dataContext.Operations.Collection, State, SelectedAccount); } }

        public override bool Ready { get { return !string.IsNullOrEmpty(State.Name) && State.IconSource != null; } }

        public IEnumerable<Icon> Icons { get { return _iconsService.Icons; } }
        #endregion

        #region Constructors
        public CategoryViewModel(OperationCategory category)
            : base(category)
        {
            State.PropertyChanged += OnStateChanged;
        }
        #endregion

        #region State event handlers
        private void OnStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Limit") || e.PropertyName.Equals("Limited"))
            {
                OnPropertyChanged("Limit");
            }
        }
        #endregion

        #region UI event handlers
        public override void OnSave()
        {
            if (EditMode)
                _dataContext.Categories.Update(_entity, State);
            else
                _dataContext.Categories.Add(State);
        }

        public override void OnRemove()
        {
            
        }

        public bool TryRemove()
        {
            int operationsCount = _dataContext.Operations.Collection.Count(x => x.Category.Equals(_entity));
            if (operationsCount == 0)
            {
                _dataContext.Categories.Remove(_entity);
                return true;
            }
            else
            {
                Removing(_entity, operationsCount);
                return false;
            }
        }

        public void KeepRemoving(OperationCategory category)
        {
            _dataContext.Operations.RemoveRange(_dataContext.Operations.Collection.Where(x => x.Category.Equals(category)));
            _dataContext.Categories.Remove(category);
        }
        #endregion

        #region Helper Methods
        protected override void InitializeDependencies()
        {
            base.InitializeDependencies();

            _iconsService = App.Container.GetInstance<IIconsService>();
            _statisticsManager = App.Container.GetInstance<IStatisticsManager>();
        }

        protected override void InitializeState()
        {
            
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
