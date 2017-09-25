using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;
using HomeManager.Infrastructure;
using HomeManager.Resources;
using System;

namespace HomeManager.Services
{
    public class AccountsInitializer
    {
        private IDataContext _dataContext;

        public AccountsInitializer(IDataContext datacontext)
        {
            _dataContext = datacontext;
            _dataContext.Loaded += Loaded;
        }

        private void Loaded(object sender, EventArgs e)
        {
            var baseAccount = _dataContext.Accounts.Get(0);
            
            if (baseAccount == null)
            {
                baseAccount = new CashAccount(AppResources.BaseAccountName);
                _dataContext.Accounts.Collection.Add(baseAccount);
            }

            foreach (var cur in _dataContext.Cash.Collection)
            {
                cur.Account = cur.Account != null ? _dataContext.Accounts.Get(cur.Account.Id) : baseAccount;
            }

            foreach (var op in _dataContext.Operations.Collection)
            {
                op.Account = op.Account != null ? _dataContext.Accounts.Get(op.Account.Id) : baseAccount; 
            }

            foreach (var conv in _dataContext.Conversions.Collection)
            {
                conv.Account = conv.Account != null ? _dataContext.Accounts.Get(conv.Account.Id) : baseAccount;
            }
        }
    }
}
