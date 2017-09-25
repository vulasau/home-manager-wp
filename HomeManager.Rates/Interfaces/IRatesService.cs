using HomeManager.Rates.Events;
using System;

namespace HomeManager.Rates.Interfaces
{
    public interface IRatesService
    {
        void Load();
        void Save();
        void LoadAsync();
        void SaveAsync();
        void Clear();
        void UpdateAsync(string currencyUid);

        event EventHandler<RatesEventArgs> Updated;
        event EventHandler Loaded;
        event EventHandler<int> Failed;
    }
}
