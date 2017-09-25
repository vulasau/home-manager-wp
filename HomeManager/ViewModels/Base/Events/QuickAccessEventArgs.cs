using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager.ViewModels.Base.Events
{
    public class QuickAccessEventArgs<TItem, TValue>: EventArgs
    {
        public TItem Selected { get; private set; }
        public TValue Value { get; private set; }

        public QuickAccessEventArgs(TItem selected, TValue value)
        {
            Selected = selected;
            Value = value;
        }
    }
}
