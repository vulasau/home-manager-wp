using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using HomeManager.ViewModels.Protection;
using Microsoft.Phone.Shell;
using HomeManager.Resources;

namespace HomeManager.Views.Protection
{
    public partial class LogInPage : PhoneApplicationPage
    {
        private LogInViewModel _viewModel;

        public LogInPage()
        {
            _viewModel = new LogInViewModel();
            _viewModel.Entered += OnEntered;
            _viewModel.Restored += OnRestored;
            
            InitializeComponent();
            DataContext = _viewModel;
        }

        #region UI event handlers
        private void OnEnterClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OnEnter();
        }

        private void OnRestoreClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OnRestore();
        }
        #endregion

        #region Event handlers
        private void OnRestored(object sender, bool e)
        {
            if (e)
            {
                PhoneApplicationService.Current.State["changing"] = true;
                NavigationService.Navigate(new Uri("/Views/Protection/PasswordPage.xaml", UriKind.Relative));
            }
        }

        private void OnEntered(object sender, bool e)
        {
            if (!e)
                MessageBox.Show(string.Format(AppResources.WrongPasswordMessage));  
            else
                NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }
        #endregion

        #region navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!_viewModel.Protected)
            {
                var uri = new Uri("/Views/MainPage.xaml", UriKind.Relative);
                NavigationService.Navigate(uri);
            }
        }
        #endregion
    }
}