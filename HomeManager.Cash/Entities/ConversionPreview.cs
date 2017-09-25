using System.ComponentModel;

namespace HomeManager.Cash.Entities
{
    public class ConversionPreview: INotifyPropertyChanged
    {
        private double _plus;
        public double Plus
        {
            get { return _plus; }
            set
            {
                _plus = value;
                OnPropertyChanged("Plus");
            }
        }

        private double _minus;
        public double Minus
        {
            get { return _minus; }
            set
            {
                _minus = value;
                OnPropertyChanged("Minus");
            }
        }

        public ConversionPreview(double plus, double minus)
        {
            Plus = plus;
            Minus = minus;
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
