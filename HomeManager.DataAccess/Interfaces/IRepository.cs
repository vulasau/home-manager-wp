using HomeManager.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HomeManager.DataAccess.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        string FileName { get; }
        ObservableCollection<T> Collection { get; }
        void Add(T item);
        T Get(int id);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> collection);
        void Update(T old, T current);
        void Clear();
        void Save();
        void Load();

        ObservableCollection<T> Where(Func<T, bool> filter);
        T FirstOrDefault(Func<T, bool> filter = null);

        event EventHandler<T> Added;
        event EventHandler<T> Removed;
    }
}
