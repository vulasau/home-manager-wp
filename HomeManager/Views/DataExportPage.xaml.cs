using System.Windows;
using Microsoft.Phone.Controls;
using HomeManager.ViewModels;

namespace HomeManager.Views
{
    public partial class DataExportPage : PhoneApplicationPage
    {
        private DataExportViewModel _viewModel;

        public DataExportPage()
        {
            InitializeComponent();
            _viewModel = new DataExportViewModel();
            _viewModel.Completed += OnCompleted;

            DataContext = _viewModel;
        }

        private void OnCompleted(string message, string caption)
        {
            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        private void OnFailed(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        private void OnExportClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OnExport();
        }
    }
}