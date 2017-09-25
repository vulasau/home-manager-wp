using HomeManager.Entities.Enums;
using System;

namespace HomeManager.Entities
{
    public class Operation : EntityBase
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

        private OperationType _type;
        public OperationType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        private OperationCategory _category;
        public OperationCategory Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        private CurrencyName _currency;
        public CurrencyName Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                OnPropertyChanged("Currency");
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

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
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

        public Operation()
        {
            Date = DateTime.Now;
        }

        public new Operation Clone()
        {
            return (Operation)MemberwiseClone();
        }

        public override void Update(EntityBase entity)
        {
            var current = entity as Operation;
            Date = current.Date;
            Type = current.Type;
            Category = current.Category;
            Currency = current.Currency;
            Amount = current.Amount;
            Description = current.Description;
            Account = current.Account;
        }
    }
}
