using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace HomeManager.Views
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void OnEmailClick(object sender, RoutedEventArgs e)
        {
            var email = new EmailComposeTask();
            email.Subject = HomeManager.Resources.AppResources.ApplicationTitle;
            email.Body = HomeManager.Resources.AppResources.MailBody;
            email.To = HomeManager.Resources.AppResources.Mail;
            email.Show();
        }
    }
}