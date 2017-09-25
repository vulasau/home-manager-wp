using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;
using HomeManager.PhoneMemoryAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeManager.DataAccess
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private int _index;
        private IPhoneMemoryService _memoryService;

        public string FileName { get; private set; }
        public ObservableCollection<T> Collection { get; private set; }

        public Repository(string fileName, IPhoneMemoryService memoryService)
        {
            _index = 0;
            _memoryService = memoryService;
            
            FileName = fileName;
            Collection = new ObservableCollection<T>();
        }

        #region CRUD
        public void Add(T item)
        {
            if (Collection.Any())
                _index++;

            item.Id = _index;
            Collection.Add(item);
            OnAdded(item);
        }

        public T Get(int id)
        {
            return Collection.FirstOrDefault(x => x.Id == id);
        }

        public void Remove(T item)
        {
            Collection.Remove(item);
            OnRemoved(item);
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            for (int i = 0; i < collection.Count(); i++)
            {
                Remove(collection.ElementAt(i));
            }
        }

        public void Update(T old, T current)
        {
            OnRemoved(old);
            old.Update(current);
            OnAdded(current);
        }
        #endregion

        #region Save/Load
        public void Save()
        {
            _memoryService.SaveCollection<T>(Collection, FileName);
        }

        public void Load()
        {
            Collection = _memoryService.LoadCollection<T>(FileName);

            if (Collection.Any())
            {
                _index = Collection.OrderBy(x => x.Id).Last().Id;
            }
        }

        public void Clear()
        {
            Collection.Clear();
        }
        #endregion

        #region Filtering
        public ObservableCollection<T> Where(Func<T, bool> filter)
        {
            var collection = Collection.Where(filter);
            return new ObservableCollection<T>(collection);
        }

        public T FirstOrDefault(Func<T, bool> filter = null)
        {
            Func<T, bool> exp = filter != null ? filter : x => true;
            return Collection.FirstOrDefault(exp);
        }
        #endregion

        #region Events
        public event EventHandler<T> Added;
        public event EventHandler<T> Removed;

        protected virtual void OnAdded(T item)
        {
            var handler = Added;
            if (handler != null)
                handler(this, item);
        }

        protected virtual void OnRemoved(T item)
        {
            var handler = Removed;
            if (handler != null)
                handler(this, item);
        }  
        #endregion
    }
}
