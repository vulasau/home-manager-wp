using HomeManager.ViewModels;
using Microsoft.Phone.Controls;

namespace HomeManager.Views
{
    public partial class BudgetPage : PhoneApplicationPage
    {
        private BudgetViewModel _viewModel;
        public BudgetPage()
        {
            InitializeComponent();
            _viewModel = new BudgetViewModel();
            DataContext = _viewModel;
        }
    }
}