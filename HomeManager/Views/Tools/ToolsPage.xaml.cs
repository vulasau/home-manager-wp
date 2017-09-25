using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace HomeManager.Views.Tools
{
    public partial class ToolsPage : PhoneApplicationPage
    {
        public ToolsPage()
        {
            InitializeComponent();
        }

        private void OnDepositCalculatorClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Tools/DepositPage.xaml", UriKind.Relative));
        }

        private void OnCreditCalculatorClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Tools/CreditPage.xaml", UriKind.Relative));
        }
    }
}