using HomeManager.ViewModels.Base;
using System.ComponentModel;

namespace HomeManager.ViewModels.Protection
{
    public class PasswordViewModel : PasswordBase
    {
        #region Publci fields
        private string _confirmation;
        public string Confirmation
        {
            get { return _confirmation; }
            set
            {
                _confirmation = value;
                OnPropertyChanged("Confirmation");
            }
        }

        public bool Ready
        {
            get
            {
                if (Protected)
                    return string.Equals(Password, Confirmation)
                        && !string.IsNullOrEmpty(Question)
                        && !string.IsNullOrEmpty(Answer);
                else
                    return true;
            }
        }
        #endregion

        public PasswordViewModel()
            : base()
        {
            if (Protected)
                Confirmation = _options.Password;

            this.PropertyChanged += OnStateChanged;
        }

        public PasswordViewModel(bool changing)
            : base()
        {
            if (changing)
            {
                Protected = true;
                Password = string.Empty;
                Confirmation = string.Empty;
                Question = string.Empty;
                Answer = string.Empty;
            }

            this.PropertyChanged += OnStateChanged;
        }

        #region UI event handlers
        public void OnSave()
        {
            _options.Protected = Protected;

            if (Protected)
            {
                _options.Password = Password.GetHashCode().ToString();
                _options.Question = Question;
                _options.Answer = Answer;
            }

            _options.Save();
        }
        #endregion

        #region Property changed event handlers
        private void OnStateChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Ready");
        }
        #endregion
    }
}
