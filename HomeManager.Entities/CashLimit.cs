
namespace HomeManager.Entities
{
    public class CashLimit: EntityBase
    {
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

        public override void Update(EntityBase entity)
        {
            var current = entity as CashLimit;
            Limit = current.Limit;
            Limited = current.Limited;
        }
    }
}
