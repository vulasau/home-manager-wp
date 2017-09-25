using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;
using HomeManager.Infrastructure;
using System.ComponentModel;
using System.Windows;

namespace HomeManager.ViewModels.Base
{
    public abstract class ViewModelBase: INotifyPropertyChanged
    {
        #region Dependencies
        protected IDataContext _dataContext;
        protected Options _options;
        #endregion

        #region Account fileds
        public CashAccount SelectedAccount
        {
            get 
            {
                if (_options.Account == null)
                    _options.Account = _dataContext.Accounts.FirstOrDefault();
                return _options.Account;
            }
            set 
            {
                _options.Account = value;
                OnPropertyChanged("SelectedAccount");
            }
        }
        #endregion

        protected ViewModelBase()
        {
            _dataContext = App.Container.GetInstance<IDataContext>();
            _options = App.Container.GetInstance<Options>();
        }

        public virtual void Update()
        {

        }

        #region Property changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        protected void OnPropertyChanged(params string[] properties)
        {
            foreach (string property in properties)
                OnPropertyChanged(property);
        }
        #endregion
    }
}
