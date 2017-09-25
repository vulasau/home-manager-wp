using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomeManager.ViewModels;
using HomeManager.Entities;
using HomeManager.Resources;

namespace HomeManager.Views
{
    public partial class CategoryPage : PhoneApplicationPage
    {
        CategoryViewModel _viewModel;
        private bool _initialized;

        public CategoryPage()
        {
            InitializeComponent();
        }

        #region Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_initialized)
                return;
            else _initialized = true;

            var category = (OperationCategory)PhoneApplicationService.Current.State["scategory"];
            _viewModel = new CategoryViewModel(category);
            _viewModel.Removing += OnRemoving;

            this.DataContext = _viewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (PhoneApplicationService.Current.State.ContainsKey("scategory"))
                PhoneApplicationService.Current.State.Remove("scategory");
        }
        #endregion

        #region App bar event handlers
        private void OnSaveClick(object sender, EventArgs e)
        {
            _viewModel.OnSave();
            NavigationService.GoBack();
        }

        private void OnRemoveClick(object sender, EventArgs e)
        {
            if (_viewModel.TryRemove())
                NavigationService.GoBack();
        }
        #endregion

        #region ViewModel event handlers
        private void OnRemoving(OperationCategory category, int operationsCount)
        {
            var result = MessageBox.Show(string.Format(
                AppResources.RemoveCategoryMessage, category.Name, operationsCount),
                AppResources.AttentionCaption, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                _viewModel.KeepRemoving(category);
                NavigationService.GoBack();
            }
        }
        #endregion
    }
}