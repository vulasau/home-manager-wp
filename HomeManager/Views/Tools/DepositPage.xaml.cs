using Microsoft.Phone.Controls;
using HomeManager.ViewModels.Tools;

namespace HomeManager.Views.Tools
{
    public partial class DepositPage : PhoneApplicationPage
    {
        public DepositPage()
        {
            InitializeComponent();
            DataContext = new DepositViewModel();
        }
    }
}