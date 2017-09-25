using HomeManager.Rates.Entities;
using HomeManager.Rates.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeManager.Rates
{
    public class RatesHtmlParser: IRatesHtmlParser
    {
        public RatesHtmlParser()
        {

        }

        public ObservableCollection<CurrencyRate> Parse(string html)
        {
            try
            {
                return GetRates(html);
            }
            catch
            {
                throw new FormatException();
            }
        }

        #region Private Methods
        private ObservableCollection<CurrencyRate> GetRates(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var table = doc.GetElementbyId("historicalRateTbl");
            var tbody = table.ChildNodes.FindFirst("tbody");
            var rows = tbody.ChildNodes.Where(x => x.Name.Equals("tr")).ToList();
            
            return RatesFromNodes(rows);
        }

        private ObservableCollection<CurrencyRate> RatesFromNodes(IEnumerable<HtmlNode> nodes)
        {
            var rates = new ObservableCollection<CurrencyRate>();
            foreach (var node in nodes)
            {
                rates.Add(CurrencyFromNode(node));
            }
            return rates;
        }

        private CurrencyRate CurrencyFromNode(HtmlNode row)
        {
            var columns = row.ChildNodes.Where(x => x.Name.Equals("td"));
            var uid = columns.ElementAt(0).InnerText;
            var name = columns.ElementAt(1).InnerText;
            var buyRate = ParseDouble(columns.ElementAt(2).InnerText);
            var sellRate = ParseDouble(columns.ElementAt(3).InnerText);

            return new CurrencyRate()
            {
                Uid = uid,
                Name = name,
                BuyRate = buyRate,
                SellRate = sellRate
            };
        }

        private double ParseDouble(string text)
        {
            double value = Convert.ToDouble(text, System.Globalization.CultureInfo.InvariantCulture);
            return value > 1 ? Math.Round(value, 2) : Math.Round(value, CountRoundVlue(value));
        }

        private int CountRoundVlue(double value)
        {
            int i = 0;
            while (value <= 1)
            {
                value *= 10;
                i++;
            }
            return ++i;
        }
        #endregion
    }
}
