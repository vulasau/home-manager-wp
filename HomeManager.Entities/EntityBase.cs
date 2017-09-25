using HomeManager.Entities.Interfaces;
using System;
using System.ComponentModel;

namespace HomeManager.Entities
{
    public abstract class EntityBase : INotifyPropertyChanged, IEquatable<EntityBase>, IClonable<EntityBase>, IComparable<EntityBase>
    {
        public int Id { get; set; }

        public EntityBase()
        {

        }

        public abstract void Update(EntityBase entity);

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Equatable
        public virtual bool Equals(EntityBase other)
        {
            if (other == null)
                return false;

            return this.Id.Equals(other.Id);
        }
        #endregion

        #region Clonable
        public EntityBase Clone()
        {
            return (EntityBase)this.MemberwiseClone();
        }
        #endregion

        #region IComparable
        public virtual int CompareTo(EntityBase other)
        {
            if (other == null)
                return -1;
            return Id == other.Id ? 1 : 0;
        }
        #endregion
    }
}
