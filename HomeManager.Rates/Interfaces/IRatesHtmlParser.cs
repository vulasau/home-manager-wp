using HomeManager.Rates.Entities;
using System.Collections.ObjectModel;

namespace HomeManager.Rates.Interfaces
{
    public interface IRatesHtmlParser
    {
        ObservableCollection<CurrencyRate> Parse(string html);
    }
}
