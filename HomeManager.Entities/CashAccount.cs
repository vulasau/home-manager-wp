using System.ComponentModel;
namespace HomeManager.Entities
{
    public class CashAccount: EntityBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private double _limit;
        public double Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                OnPropertyChanged("Limit");
            }
        }

        private bool _limited;
        public bool Limited
        {
            get { return _limited; }
            set
            {
                _limited = value;
                OnPropertyChanged("Limited");
            }
        }

        public CashAccount()
        {
            Name = string.Empty;
        }

        public CashAccount(string name)
        {
            Name = name;
        }

        public override void Update(EntityBase entity)
        {
            var current = entity as CashAccount;
            Name = current.Name;
            Limit = current.Limit;
            Limited = current.Limited;
        }
    }
}
