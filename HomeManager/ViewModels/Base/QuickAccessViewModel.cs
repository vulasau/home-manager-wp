using HomeManager.Entities;
using HomeManager.ViewModels.Base.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace HomeManager.ViewModels.Base
{
    public class QuickAccessViewModel<TItem, TValue>: ViewModelBase where TItem : Entities.EntityBase
    {
        #region Private fields
        private IEnumerable<TItem> _items;
        private Func<TItem, bool> _filter;
        private Func<TItem, object> _ordering;
        #endregion

        #region Public Fileds
        private TItem _selectedItem;
        public TItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        private TValue _value;
        public TValue Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public bool Available { get { return Items.Any(); } }

        public IEnumerable<TItem> Items { get { return OrderItems(_items); } }
        #endregion

        public QuickAccessViewModel(IEnumerable<TItem> items, Func<TItem, bool> filter = null, Func<TItem, object> ordering = null)
        {
            _items = items;
            _filter = filter != null ? filter : x => true;
            _ordering = ordering != null ? ordering : x => x.Id;
            
            if (items is ObservableCollection<TItem>)
                ((ObservableCollection<TItem>)items).CollectionChanged += OnCollectionChanged;
            if(CategoryUsageSorting())
                _dataContext.Operations.Collection.CollectionChanged += OperationsCollectionChaged;

            UpdateSelectedItem();
        }

        #region UI event handlers
        public void OnAdd()
        {
            OnAddInvoked();

            Value = default(TValue);
        }
        #endregion

        #region Private methods
        private void UpdateSelectedItem()
        {
            if (SelectedItem == null)
                SelectedItem = Items.FirstOrDefault();
        }

        private bool CategoryUsageSorting()
        {
            return typeof(TItem) == typeof(OperationCategory) 
                && _options.DefaultSortOrder == Infrastructure.Enums.CategorySortOrder.Usage;
        }

        private IEnumerable<TItem> OrderItems(IEnumerable<TItem> items)
        {
            if (CategoryUsageSorting())
                return items.Where(_filter).OrderByDescending(_ordering);
            return items.Where(_filter).OrderBy(_ordering);
        }
        #endregion

        #region Events
        public delegate void QuickAccessEventHandler(object sender, QuickAccessEventArgs<TItem, TValue> e);
        public event QuickAccessEventHandler AddInvoked;

        protected void OnAddInvoked()
        {
            if (AddInvoked != null)
                AddInvoked(this, new QuickAccessEventArgs<TItem, TValue>(SelectedItem, Value));
        }
        #endregion

        #region Collection changed
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Items", "Available");
            UpdateSelectedItem();
        }

        private void OperationsCollectionChaged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Items");
        }
        #endregion
    }
}
