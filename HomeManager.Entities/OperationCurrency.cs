using HomeManager.Entities.Enums;

namespace HomeManager.Entities
{
    public class OperationCurrency: EntityBase
    {
        public CurrencyName Name { get; set; }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged("Balance");
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

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Balance);
        }

        public new OperationCurrency Clone()
        {
            return (OperationCurrency)this.MemberwiseClone();
        }

        public override void Update(EntityBase entity)
        {
            var current = entity as OperationCurrency;
            Name = current.Name;
            Balance = current.Balance;
            Account = current.Account;
        }

        public override bool Equals(EntityBase other)
        {
            if (other is OperationCurrency)
                return ((OperationCurrency)other).Name == Name
                    && ((OperationCurrency)other).Account.Equals(Account);
            return false;
        }

        public override int CompareTo(EntityBase other)
        {
            if (other == null || !(other is OperationCurrency))
                return -1;
            return Name == ((OperationCurrency)other).Name
                && Account.Equals(((OperationCurrency)other).Name) ? 1 : 0;
        }
    }
}
