using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomeManager.ViewModels;
using HomeManager.Entities;

namespace HomeManager.Views
{
    public partial class ConversionPage : PhoneApplicationPage
    {
        private ConversionViewModel _viewModel;
        private bool _initialized;

        public ConversionPage()
        {
            InitializeComponent();
        }

        #region Application bar event handlers
        private void OnSaveClick(object sender, EventArgs e)
        {
            _viewModel.OnSave();
            NavigationService.GoBack();
        }

        private void OnRemoveClick(object sender, EventArgs e)
        {
            _viewModel.OnRemove();
            NavigationService.GoBack();
        }
        #endregion

        #region Navigation
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_initialized)
                return;
            else
                _initialized = true;

            if (PhoneApplicationService.Current.State.ContainsKey("sconversion"))
            {
                var conversion = (Conversion)PhoneApplicationService.Current.State["sconversion"];
                _viewModel = new ConversionViewModel(conversion);
            }
            else
            {
                _viewModel = new ConversionViewModel();
            }

            this.DataContext = _viewModel;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("sconversion"))
                PhoneApplicationService.Current.State.Remove("sconversion");
        }
        #endregion 
    }
}