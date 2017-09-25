using System.Windows;
using Microsoft.Phone.Controls;
using HomeManager.ViewModels;

namespace HomeManager.Views
{
    public partial class RatesPage : PhoneApplicationPage
    {
        bool _initialized;
        private RatesViewModel _viewModel;

        public RatesPage()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!_initialized)
            {
                _initialized = true;
                _viewModel = new RatesViewModel();
                _viewModel.Failed += OnFailed;
                DataContext = _viewModel;
            }
        }

        private void OnFailed(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }
    }
}