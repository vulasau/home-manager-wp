using HomeManager.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;

namespace HomeManager.Infrastructure
{
    public class SingletoneContainer: IContainer
    {
        private Dictionary<Type, object> _components;

        public SingletoneContainer()
        {
            _components = new Dictionary<Type, object>();
        }

        public void RegisterInstance<T>(T instance)
        {
            Type type = typeof(T);
            if(!_components.ContainsKey(type)){
                _components.Add(type, instance);
            }
        }

        public T GetInstance<T>()
        {
            Type type = typeof(T);
            if (!_components.ContainsKey(type))
                throw new ArgumentNullException(string.Format("Object of type: {0} was not registered.", type.Name));
            return (T)_components[type];
        }
    }
}
