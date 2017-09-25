using System;

namespace HomeManager.ViewModels.Protection
{
    public class LogInViewModel : PasswordBase
    {
        public LogInViewModel()
            : base()
        {
            Answer = string.Empty;
            Password = string.Empty;
        }

        #region UI event handlers
        public void OnEnter()
        {
            bool success = string.Equals(Password.GetHashCode().ToString(), _options.Password);
            OnEntered(success);
        }

        public void OnRestore()
        {
            bool success = string.Equals(Answer, _options.Answer, StringComparison.InvariantCultureIgnoreCase);
            Password = _options.Password;
            OnRestored(success);
        }
        #endregion

        #region Events
        public event EventHandler<bool> Entered;
        public event EventHandler<bool> Restored;

        private void OnEntered(bool success)
        {
            if (Entered != null)
                Entered(this, success);
        }

        private void OnRestored(bool succes)
        {
            if (Restored != null)
                Restored(this, succes);
        }
        #endregion
    }
}
