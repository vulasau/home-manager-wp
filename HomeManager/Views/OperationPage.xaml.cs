using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomeManager.Entities;
using HomeManager.ViewModels;
using HomeManager.Entities.Enums;
using System.Windows;

namespace HomeManager.Views
{
    public partial class OperationPage : PhoneApplicationPage
    {
        private OperationViewModel _viewModel;
        private bool _initialized;

        public OperationPage()
        {
            InitializeComponent();
        }

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

        #region Application bar event handlers
        private void OnSaveClick(object sender, EventArgs e)
        {
            NavigationService.GoBack();
            _viewModel.OnSave();
        }

        private void OnRemoveClick(object sender, EventArgs e)
        {
            NavigationService.GoBack();
            _viewModel.OnRemove();
        }
        #endregion

        #region Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_initialized)
                return;
            else _initialized = true;

            if (PhoneApplicationService.Current.State.ContainsKey("otype"))
            {
                var type = (OperationType)PhoneApplicationService.Current.State["otype"];
                _viewModel = new OperationViewModel(type);
            }
            else
            {
                var operation = (Operation)PhoneApplicationService.Current.State["soperation"];
                _viewModel = new OperationViewModel(operation);
            }

            _viewModel.Completed += OnCompleted;
            _viewModel.Failed += OnFailed;

            this.DataContext = _viewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (PhoneApplicationService.Current.State.ContainsKey("otype"))
                PhoneApplicationService.Current.State.Remove("otype");

            if (PhoneApplicationService.Current.State.ContainsKey("soperation"))
                PhoneApplicationService.Current.State.Remove("soperation");
        }
        #endregion

        #region ViewModel event handlers
        private void OnCompleted(string message, string caption)
        {
            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        private void OnFailed(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }
        #endregion
    }
}