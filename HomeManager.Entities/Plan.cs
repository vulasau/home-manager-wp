using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager.Entities
{
    public class Plan: EntityBase
    {
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

        private CashLimit _dayLimit;
        public CashLimit DayLimit
        {
            get { return _dayLimit; }
            set
            {
                _dayLimit = value;
                OnPropertyChanged("DayLimit");
            }
        }

        public ObservableCollection<PlaningGoal> Goals { get; set; }

        public Plan()
            : base()
        {

        }

        public override void Update(EntityBase entity)
        {
            var current = entity as Plan;
            Account = current.Account;
            Goals = current.Goals;
            DayLimit = current.DayLimit;
        }
    }
}
