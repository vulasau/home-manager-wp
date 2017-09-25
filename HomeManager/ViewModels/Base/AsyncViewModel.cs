using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager.ViewModels.Base
{
    public abstract class AsyncViewModel : ViewModelBase
    {
        #region Public fields
        private bool _inProgress;
        public bool InProgress
        {
            get { return _inProgress; }
            private set
            {
                _inProgress = value;
                OnPropertyChanged("InProgress");
            }
        }

        private string _progressMessage;
        public string ProgressMessage
        {
            get { return _progressMessage; }
            private set
            {
                _progressMessage = value;
                OnPropertyChanged("ProgressMessage");
            }
        }
        #endregion

        protected AsyncViewModel()
            : base()
        {

        }

        protected virtual void OnStarted(string message)
        {
            InProgress = true;
            ProgressMessage = message;
        }

        #region Events
        public delegate void MessageEventHandler(string message, string caption);

        public event MessageEventHandler Completed;
        public event MessageEventHandler Failed;

        protected virtual void OnCompleted(string message = null)
        {
            InProgress = false;
            ProgressMessage = string.Empty;

            if (Completed != null)
                Completed(message, Resources.AppResources.OkCaption);
        }

        protected virtual void OnFailed(string message)
        {
            InProgress = false;
            ProgressMessage = string.Empty;

            if (Failed != null)
                Failed(message, Resources.AppResources.ErrorCaption);
        }
        #endregion
    }
}
