using HomeManager.ViewModels.Base;

namespace HomeManager.ViewModels.Protection
{
    public abstract class PasswordBase: ViewModelBase
    {
        #region Public fields
        private bool _protected;
        public bool Protected
        {
            get { return _protected; }
            set
            {
                _protected = value;
                OnPropertyChanged("Protected");
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _question;
        public string Question
        {
            get { return _question; }
            set
            {
                _question = value;
                OnPropertyChanged("Question");
            }
        }

        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                OnPropertyChanged("Answer");
            }
        }
        #endregion

        protected PasswordBase()
            : base()
        {
            Protected = _options.Protected;

            if (Protected)
            {
                Password = _options.Password;
                Question = _options.Question;
                Answer = _options.Answer;
            }
        }
    }
}
