using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomeManager.ViewModels;
using HomeManager.Entities.Enums;

namespace HomeManager.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _initialized;
        private MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        #region UI Event Handlers
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!_initialized)
            {
                _initialized = true;
                _viewModel = new MainViewModel();
                DataContext = _viewModel;
            }
        }

        private void OnQuickAddExpenseClick(object sender, RoutedEventArgs e)
        {
            _viewModel.QuickExpense.OnAdd();
        }

        private void OnQuickAddIncomeClick(object sender, RoutedEventArgs e)
        {
            _viewModel.QuickIncome.OnAdd();
        }
        #endregion

        #region Expenses application bar event handlers
        private void OnAddExpenseClick(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["otype"] = OperationType.Expense;
            NavigationService.Navigate(new Uri("/Views/OperationPage.xaml", UriKind.Relative));
        }

        private void OnEditExpenseClick(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["soperation"] = _viewModel.SelectedExpense;
            NavigationService.Navigate(new Uri("/Views/OperationPage.xaml", UriKind.Relative));
        }

        private void OnRemoveExpenseClick(object sender, EventArgs e)
        {
            _viewModel.OnRemoveExpense();
        }
        #endregion

        #region Incomes application bar event handlers
        private void OnAddIncomeClick(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["otype"] = OperationType.Income;
            NavigationService.Navigate(new Uri("/Views/OperationPage.xaml", UriKind.Relative));
        }

        private void OnEditIncomeClick(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["soperation"] = _viewModel.SelectedIncome;
            NavigationService.Navigate(new Uri("/Views/OperationPage.xaml", UriKind.Relative));
        }

        private void OnRemoveIncomeClick(object sender, EventArgs e)
        {
            _viewModel.OnRemoveIncome();
        }
        #endregion

        #region Conversions application bar event handlers
        private void OnAddConversionClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ConversionPage.xaml", UriKind.Relative));
        }

        private void OnEditConversionClick(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["sconversion"] = _viewModel.SelectedConversion;
            NavigationService.Navigate(new Uri("/Views/ConversionPage.xaml", UriKind.Relative));
        }

        private void OnRemoveConversionClick(object sender, EventArgs e)
        {
            _viewModel.OnRemoveConversion();
        }

        private void OnRatesClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/RatesPage.xaml", UriKind.Relative));
        }
        #endregion

        #region Application bar event handlers
        private void OnStatisticsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/StatisticsPage.xaml", UriKind.Relative));
        }

        private void OnBudgetClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/BudgetPage.xaml", UriKind.Relative));
        }

        private void OnAccountsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AccountsPage.xaml", UriKind.Relative));
        }

        private void OnToolsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Tools/ToolsPage.xaml", UriKind.Relative));
        }

        private void OnOptionsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/OptionsPage.xaml", UriKind.Relative));
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }
        #endregion

        #region Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Closing the application ignoring LogIn page navigation
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();

            base.OnNavigatedTo(e);
        }
        #endregion
    }
}