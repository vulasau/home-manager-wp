using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomeManager.ViewModels;
using System.Windows.Navigation;

namespace HomeManager.Views
{
    public partial class SkyDrivePage : PhoneApplicationPage
    {
        private SkyDriveViewModel _viewModel;

        public SkyDrivePage()
        {
            InitializeComponent();
            _viewModel = new SkyDriveViewModel();
            _viewModel.Completed += OnCompleted;
            _viewModel.Failed += OnFailed;

            DataContext = _viewModel;
        }

        #region Ui event handlers
        private void LiveSessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e != null)
                _viewModel.OnSessionChanged(e.Session);
        }

        private void OnUploadClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OnUpload();
        }

        private void OnDownloadClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OnDownload();
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
        #endregion

        #region Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (_viewModel != null)
                _viewModel.Update();
        }
        #endregion
    }
}