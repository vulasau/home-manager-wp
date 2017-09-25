using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;
using HomeManager.PhoneMemoryAccess.Interfaces;
using System;
using System.Threading.Tasks;

namespace HomeManager.DataAccess
{
    public class DataContext: IDataContext
    {
        private IPhoneMemoryService _memoryService;

        public IRepository<Operation> Operations { get; set; }
        public IRepository<Conversion> Conversions { get; set; }
        public IRepository<OperationCategory> Categories { get; set; }
        public IRepository<OperationCurrency> Cash { get; set; }
        public IRepository<CashAccount> Accounts { get; set; }

        public DataContext(IPhoneMemoryService memoryService)
        {
            _memoryService = memoryService;

            Operations = new Repository<Operation>("Operations.xml", _memoryService);
            Conversions = new Repository<Conversion>("Conversions.xml", _memoryService);
            Categories = new Repository<OperationCategory>("Categories.xml", _memoryService);
            Cash = new Repository<OperationCurrency>("Cash.xml", _memoryService);
            Accounts = new Repository<CashAccount>("Accounts.xml", _memoryService);
        }

        #region Save/Load
        public void Save()
        {
            SaveAll();
        }

        public void Load()
        {
            LoadAll();
        }

        public void Clear()
        {
            _memoryService.Clear();
            _memoryService.DeleteFile(Operations.FileName);
            _memoryService.DeleteFile(Conversions.FileName);
            _memoryService.DeleteFile(Categories.FileName);
            _memoryService.DeleteFile(Cash.FileName);

            Operations.Clear();
            Conversions.Clear();
            Categories.Clear();
            Cash.Clear();
        }
        #endregion

        #region Async methods
        public async void SaveAsync()
        {
            await Task.Run(() => SaveAll());
        }

        public async void LoadAsync()
        {
            await Task.Run(() => LoadAll());
        }
        #endregion

        #region Private methods
        private void SaveAll()
        {
            Accounts.Save();
            Operations.Save();
            Conversions.Save();
            Categories.Save();
            Cash.Save();

            OnSaved();
        }

        private void LoadAll()
        {
            Accounts.Load();
            Operations.Load();
            Conversions.Load();
            Categories.Load();
            Cash.Load();

            OnLoaded();
        }
        #endregion

        #region Events
        public event EventHandler Loaded;
        public event EventHandler Saved;

        protected virtual void OnLoaded()
        {
            if (Loaded != null)
                Loaded(this, new EventArgs());
        }

        protected virtual void OnSaved()
        {
            if (Saved != null)
                Saved(this, new EventArgs());
        }
        #endregion
    }
}
