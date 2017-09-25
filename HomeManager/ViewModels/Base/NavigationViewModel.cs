namespace HomeManager.ViewModels.Base
{
    public class NavigationViewModel: ViewModelBase
    {
        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged("SelectedTabIndex");
                SelectedTabChanged();
            }
        }

        public NavigationViewModel()
            : base()
        {

        }

        protected virtual void SelectedTabChanged()
        {

        }
    }
}
