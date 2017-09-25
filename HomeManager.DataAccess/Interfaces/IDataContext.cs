using HomeManager.Entities;
using System;

namespace HomeManager.DataAccess.Interfaces
{
    public interface IDataContext
    {
        IRepository<Operation> Operations { get; set; }
        IRepository<Conversion> Conversions { get; set; }
        IRepository<OperationCategory> Categories { get; set; }
        IRepository<OperationCurrency> Cash { get; set; }
        IRepository<CashAccount> Accounts { get; set; }

        event EventHandler Loaded;
        //event EventHandler Saved;

        void Save();
        void Load();
        void Clear();

        //void LoadAsync();
        //void SaveAsync();
    }
}
