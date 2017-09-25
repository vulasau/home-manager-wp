using HomeManager.Rates.Interfaces;
using System;
using System.Net;

namespace HomeManager.Rates
{
    public class RatesWebClient: IRatesWebClient
    {
        #region Private Fields
        private readonly string _dateFormat = "{0:yyyy-MM-dd}";
        private readonly string _urlFormat = @"http://www.xe.com/currencytables/?from={0}&date={1}";

        #endregion

        public RatesWebClient()
        {

        }

        public void LoadHtmlAsync(string currencyUid)
        {
            try
            {
                var date = string.Format(_dateFormat, DateTime.Now);
                var url = string.Format(_urlFormat, currencyUid, date);
                var uri = new Uri(url);

                var client = new WebClient();
                client.DownloadStringCompleted += DownloadCompleted;
                client.DownloadStringAsync(uri);
            }
            catch
            {
                throw new WebException();
            }
        }

        private void DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            OnHtmlLoaded(e.Result);
        }

        #region Events
        public event EventHandler<string> HtmlLoaded;

        protected void OnHtmlLoaded(string html)
        {
            if (HtmlLoaded != null)
                HtmlLoaded(this, html);
        }
        #endregion
    }
}
