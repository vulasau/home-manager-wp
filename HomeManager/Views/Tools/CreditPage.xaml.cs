using Microsoft.Phone.Controls;
using HomeManager.ViewModels.Tools;

namespace HomeManager.Views.Tools
{
    public partial class CreditPage : PhoneApplicationPage
    {
        public CreditPage()
        {
            InitializeComponent();
            DataContext = new CreditViewModel();
        }
    }
}