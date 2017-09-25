using System;

namespace HomeManager.Rates.Interfaces
{
    public interface IRatesWebClient
    {
        void LoadHtmlAsync(string currencyUid);
        event EventHandler<string> HtmlLoaded;
    }
}
