using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;
using HomeManager.Services.Interfaces;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.Linq;

namespace HomeManager.Services
{
    public class TileService: ITileService
    {
        private IRepository<OperationCurrency> _cash;

        public TileService(IDataContext dataContext)
        {
            dataContext.Conversions.Added += ConversionsChanged;
            dataContext.Conversions.Removed += ConversionsChanged;
            dataContext.Operations.Added += OperationsChanged;
            dataContext.Operations.Removed += OperationsChanged;

            _cash = dataContext.Cash;
        }

        #region Public methods
        public void UpdateTile()
        {
            var iconTileData = CreateIconTileData();
            var iconTile = ShellTile.ActiveTiles.FirstOrDefault();
            
            if(iconTile != null)
                iconTile.Update(iconTileData);
        }
        #endregion

        #region Private methods
        private ShellTileData CreateIconTileData()
        {
            var tileData = new IconicTileData();
            var content = BalanceToStringQueue();
            tileData.WideContent1 = content.Any() ? content.Dequeue() : string.Empty;
            tileData.WideContent2 = content.Any() ? content.Dequeue() : string.Empty;
            tileData.WideContent3 = content.Any() ? content.Dequeue() : string.Empty;

            return tileData;
        }

        private Queue<string> BalanceToStringQueue()
        {
            Queue<string> text = new Queue<string>();

            foreach (var currency in _cash.Collection)
            {
                if (currency.Balance > 0)
                    text.Enqueue(string.Format("{0}: {1}", currency.Name, currency.Balance));
            }

            return text;
        }
        #endregion

        #region Event Handlers
        private void OperationsChanged(object sender, Operation e)
        {
            UpdateTile();
        }

        private void ConversionsChanged(object sender, Conversion e)
        {
            UpdateTile();
        }
        #endregion
    }
}
