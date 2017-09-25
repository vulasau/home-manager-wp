using System.Windows;
using Microsoft.Phone.Controls;
using HomeManager.ViewModels.Protection;
using Microsoft.Phone.Shell;
using HomeManager.Resources;

namespace HomeManager.Views.Protection
{
    public partial class PasswordPage : PhoneApplicationPage
    {
        private PasswordViewModel _viewModel;

        public PasswordPage()
        {
            InitializeComponent();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OnSave();

            if (PhoneApplicationService.Current.State.ContainsKey("changing"))
            {
                PhoneApplicationService.Current.State.Remove("changing");
                NavigationService.Navigate(new System.Uri("/Views/MainPage.xaml", System.UriKind.Relative));
            }
            else
                NavigationService.GoBack();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("changing"))
            {
                var changing = (bool)PhoneApplicationService.Current.State["changing"];

                _viewModel = new PasswordViewModel(changing);
                MessageBox.Show(AppResources.ChangePasswordMessage);
            }
            else
                _viewModel = new PasswordViewModel();

            DataContext = _viewModel;
        }
    }
}