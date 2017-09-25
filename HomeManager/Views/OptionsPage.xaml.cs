using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomeManager.Entities;
using HomeManager.ViewModels;
using HomeManager.Resources;

namespace HomeManager.Views
{
    public partial class OptionsPage : PhoneApplicationPage
    {
        private bool _initialized;
        private OptionsViewModel _viewModel;

        public OptionsPage()
        {
            InitializeComponent();
        }

        #region UI event handlers
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!_initialized)
            {
                _initialized = true;
                _viewModel = new OptionsViewModel();
                _viewModel.Removing += OnRemoving;
                DataContext = _viewModel;
            }
        }

        private void OnProtectionClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Protection/PasswordPage.xaml", UriKind.Relative));
        }

        private void OnDataExportClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/DataExportPage.xaml", UriKind.Relative));
        }

        private void OnSkyDriveClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SkyDrivePage.xaml", UriKind.Relative));
        }

        private void OnClearDatabaseClick(object sender, RoutedEventArgs e)
        {
            var message = AppResources.ClearDatabaseMessage;
            var caption = AppResources.ClearDatabaseCaption;
            var result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                _viewModel.ClearDatabase();
        }

        //Expense
        private void OnAddExpenseClick(object sender, RoutedEventArgs e)
        {
            _viewModel.QuickExpense.OnAdd();
        }

        private void OnEditExpenseClick(object sender, EventArgs e)
        {

            PhoneApplicationService.Current.State["scategory"] = _viewModel.SelectedExpenseCategory;
            NavigationService.Navigate(new Uri("/Views/CategoryPage.xaml", UriKind.Relative));
        }

        private void OnRemoveExpenseClick(object sender, EventArgs e)
        {
            _viewModel.RemoveExpenseCategory();
        }

        //Income
        private void OnAddIcomeClick(object sender, RoutedEventArgs e)
        {
            _viewModel.QuickIncome.OnAdd();
        }

        private void OnEditIncomeClick(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["scategory"] = _viewModel.SelectedIncomeCategory;
            NavigationService.Navigate(new Uri("/Views/CategoryPage.xaml", UriKind.Relative));
        }

        private void OnRemoveIncomeClick(object sender, EventArgs e)
        {
            _viewModel.RemoveIncomeCategory();
        }
        #endregion

        #region ViewModel event handlers
        private void OnRemoving(OperationCategory category, int operationsCount)
        {
            var result = MessageBox.Show(string.Format(
                AppResources.RemoveCategoryMessage, category.Name, operationsCount), 
                AppResources.AttentionCaption, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                _viewModel.KeepRemoving(category);
        }
        #endregion

        #region Filters
        private void ExpenseCategoriesFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = (e.Item as OperationCategory).Type == Entities.Enums.OperationType.Expense;
        }

        private void IncomeCategoriesFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = (e.Item as OperationCategory).Type == Entities.Enums.OperationType.Income;
        }
        #endregion
    }
}