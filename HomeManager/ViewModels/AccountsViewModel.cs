using HomeManager.Entities;
using HomeManager.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class AccountsViewModel: ViewModelBase
    {
        #region Public fields
        private string _newAccountName;
        public string NewAccountName
        {
            get { return _newAccountName; }
            set
            {
                _newAccountName = value.TrimEnd(' ').TrimStart(' ');
                OnPropertyChanged("NewAccountName");
            }
        }

        public bool CanRemove { get { return SelectedAccount != null && SelectedAccount.Id != 0; } }
        public ObservableCollection<CashAccount> Accounts { get { return _dataContext.Accounts.Collection; } }
        #endregion

        public AccountsViewModel()
            : base()
        {
            this.PropertyChanged += OnStateChanged;
        }

        #region Ui event handlers
        public void OnAdd()
        {
            bool exist = _dataContext.Accounts.Collection.Any(x => x.Name.Equals(NewAccountName));
            if (!exist)
            {
                _dataContext.Accounts.Add(new CashAccount(NewAccountName));
                NewAccountName = string.Empty;
            }
            else
            {
                OnAddFailed();
            }
        }

        public void OnRemove()
        {
            if (CanRemove)
            {
                int operations = _dataContext.Operations.Collection.Count(x => x.Account.Equals(SelectedAccount));
                if (operations == 0)
                    _dataContext.Accounts.Remove(SelectedAccount);
                else
                    OnRemoving(SelectedAccount, operations);
            }
        }

        public void KeepRemoving(CashAccount account)
        {
            _dataContext.Operations.RemoveRange(_dataContext.Operations.Collection.Where(x => x.Account.Equals(account)));
            _dataContext.Accounts.Remove(account);
        }
        #endregion

        #region State event handlers
        private void OnStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedAccount"))
                OnPropertyChanged("CanRemove");
        }
        #endregion

        #region Events
        public delegate void RemoveAccountEventHandler(CashAccount account, int operationsCount);

        public event RemoveAccountEventHandler Removing;
        public event EventHandler AddFailed;

        protected void OnRemoving(CashAccount account, int operationsCount)
        {
            if (Removing != null)
                Removing(account, operationsCount);
        }

        protected void OnAddFailed()
        {
            if (AddFailed != null)
                AddFailed(this, null);
        }
        #endregion
    }
}
