using HomeManager.Entities.Enums;

namespace HomeManager.Entities
{
    public class OperationCategory: EntityBase
    {
        public int Usage { get; set; }

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

        private Icon _iconSource;
        public Icon IconSource
        {
            get { return _iconSource; }
            set
            {
                _iconSource = value;
                OnPropertyChanged("IconSource");
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

        private bool _budgetCategory;
        public bool BudgetCategory
        {
            get { return _budgetCategory; }
            set
            {
                _budgetCategory = value;
                OnPropertyChanged("BudgetCategory");
            }
        }

        public new OperationCategory Clone()
        {
            return (OperationCategory)this.MemberwiseClone();
        }

        public override void Update(EntityBase entity)
        {
            var current = entity as OperationCategory;
            Type = current.Type;
            Name = current.Name;
            IconSource = current.IconSource;
            Limit = current.Limit;
            Limited = current.Limited;
            BudgetCategory = current.BudgetCategory;
        }
    }
}
