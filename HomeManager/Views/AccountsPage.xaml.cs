using System;
using System.Windows;
using Microsoft.Phone.Controls;
using HomeManager.ViewModels;
using HomeManager.Entities;
using HomeManager.Resources;
using Microsoft.Phone.Shell;

namespace HomeManager.Views
{
    public partial class AccountsPage : PhoneApplicationPage
    {
        private AccountsViewModel _viewModel;

        public AccountsPage()
        {
            InitializeComponent();
            _viewModel = new AccountsViewModel();
            _viewModel.Removing += OnRemoving;
            _viewModel.AddFailed += OnAddFailed;
            DataContext = _viewModel;
        }

        #region Ui event handlers
        private void OnAddAccountClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OnAdd();
        }

        private void OnRemoveClick(object sender, EventArgs e)
        {
            _viewModel.OnRemove();
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["saccount"] = _viewModel.SelectedAccount;
            NavigationService.Navigate(new Uri("/Views/AccountPage.xaml", UriKind.Relative));
        }
        #endregion

        #region View model event handlers
        private void OnRemoving(CashAccount account, int operationsCount)
        {
            var result = MessageBox.Show(string.Format(
                AppResources.RemoveAccountMessage, account.Name, operationsCount),
                AppResources.AttentionCaption, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                _viewModel.KeepRemoving(account);
        }

        private void OnAddFailed(object sender, EventArgs e)
        {
            MessageBox.Show(AppResources.AccountNameErrorText);
        }
        #endregion
    }
}