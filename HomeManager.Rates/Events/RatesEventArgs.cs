using HomeManager.Rates.Entities;
using System;
using System.Collections.Generic;

namespace HomeManager.Rates.Events
{
    public class RatesEventArgs: EventArgs
    {
        public IEnumerable<CurrencyRate> Rates { get; private set; }

        public RatesEventArgs(IEnumerable<CurrencyRate> collection)
            : base()
        {
            Rates = collection;
        }
    }
}
