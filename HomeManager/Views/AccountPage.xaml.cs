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
    public partial class AccountPage : PhoneApplicationPage
    {
        private AccountViewModel _viewModel;
        private bool _initialized;

        public AccountPage()
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

            var account = (CashAccount)PhoneApplicationService.Current.State["saccount"];
            _viewModel = new AccountViewModel(account);
            _viewModel.Removing += OnRemoving;

            this.DataContext = _viewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (PhoneApplicationService.Current.State.ContainsKey("saccount"))
                PhoneApplicationService.Current.State.Remove("saccount");
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
        private void OnRemoving(CashAccount account, int operationsCount)
        {
            var result = MessageBox.Show(string.Format(
                AppResources.RemoveAccountMessage, account.Name, operationsCount),
                AppResources.AttentionCaption, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                _viewModel.KeepRemoving(account);
                NavigationService.GoBack();
            }
        }
        #endregion
    }
}