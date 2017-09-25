using HomeManager.Entities;
using System.ComponentModel;

namespace HomeManager.ViewModels.Base
{
    public abstract class EntityViewModel<T> : ViewModelBase where T : EntityBase
    {
        #region Private fields
        protected T _entity;
        #endregion

        #region Public fields
        private T _state;
        public T State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                OnPropertyChanged("EditMode");
            }
        }

        public abstract bool Ready { get; }
        #endregion

        #region Constructors
        protected EntityViewModel()
            : base()
        {
            InitializeDependencies();
            InitializeState();
            State.PropertyChanged += OnStatePropertyChanged;
        }

        protected EntityViewModel(T entity)
            : base()
        {
            InitializeDependencies();
            InitializeState(entity);
            State.PropertyChanged += OnStatePropertyChanged;
            entity.PropertyChanged += OnStatePropertyChanged;
        }
        #endregion

        #region UI event handlers
        public abstract void OnSave();
        public abstract void OnRemove();
        #endregion

        #region Helper methods
        protected virtual void InitializeDependencies() { }

        private void InitializeState(T entity)
        {
            EditMode = true;
            State = (T)entity.Clone();

            _entity = entity;
        }

        protected abstract void InitializeState();

        protected virtual void OnStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Ready");
        }
        #endregion
    }
}
