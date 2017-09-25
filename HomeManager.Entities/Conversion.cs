using HomeManager.Entities.Enums;
using System;

namespace HomeManager.Entities
{
    public class Conversion : EntityBase
    {
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        private ConversionType _type;
        public ConversionType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }
        
        private CurrencyName _from;
        public CurrencyName From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        private CurrencyName _to;
        public CurrencyName To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged("To");
            }
        }

        private double _amount;
        public double Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private double _rate;
        public double Rate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                OnPropertyChanged("Rate");
            }
        }

        private CashAccount _account;
        public CashAccount Account
        {
            get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged("Account");
            }
        }

        public Conversion()
        {
            Date = DateTime.Now;
        }

        public override void Update(EntityBase entity)
        {
            var current = entity as Conversion;
            Date = current.Date;
            Type = current.Type;
            From = current.From;
            To = current.To;
            Amount = current.Amount;
            Rate = current.Rate;
            Account = current.Account;
        }

        public override string ToString()
        {
            var pattern = "{0} {1} {2} {3} {4} {5}";
            if (Type == ConversionType.Buy)
                return string.Format(pattern, Date.ToShortDateString(), Rate * Amount, From, "->", To, Amount);
            else
                return string.Format(pattern, Date.ToShortDateString(), Amount, From, "->", To, Rate * Amount);
        }
    }
}
