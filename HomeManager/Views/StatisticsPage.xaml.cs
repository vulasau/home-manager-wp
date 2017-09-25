using System;
using Microsoft.Phone.Controls;
using HomeManager.ViewModels;

namespace HomeManager.Views
{
    public partial class StatisticsPage : PhoneApplicationPage
    {
        StatisticsViewModel _viewModel;

        public StatisticsPage()
        {
            InitializeComponent();
            _viewModel = new StatisticsViewModel();
            DataContext = _viewModel;
        }

        #region UI event handlers
        private void OnAllTimeClick(object sender, EventArgs e)
        {
            _viewModel.OnAllTime();
        }

        private void OnBackClick(object sender, EventArgs e)
        {
            _viewModel.OnBack();
        }

        private void OnCurrentClick(object sender, EventArgs e)
        {
            _viewModel.OnCurrent();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            _viewModel.OnNext();
        }
        #endregion
    }
}