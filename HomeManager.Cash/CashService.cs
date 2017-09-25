using HomeManager.Cash.Entities;
using HomeManager.Cash.Interfaces;
using HomeManager.DataAccess.Interfaces;
using HomeManager.Entities;
using HomeManager.Entities.Enums;
using HomeManager.Entities.Extensions;
using System.Linq;

namespace HomeManager.Cash
{
    public class CashService : ICashService
    {
        private IRepository<OperationCurrency> _cash;
        private IRepository<Operation> _operations;
        private IRepository<Conversion> _conversions;

        public CashService(IDataContext dataContext)
        {
            _cash = dataContext.Cash;
            _operations = dataContext.Operations;
            _conversions = dataContext.Conversions;

            dataContext.Operations.Added += OnOperationAdded;
            dataContext.Operations.Removed += OnOperationRemoved;

            dataContext.Conversions.Added += OnConversionAdded;
            dataContext.Conversions.Removed += OnConversionRemoved;
        }

        public void ProcessOperation(Operation operation)
        {
            var currentCahs = GetCash(operation.Currency, operation.Account);
            if (currentCahs == null)
                currentCahs = InitializeCash(operation.Currency, operation.Account);

            if (operation.Type == OperationType.Expense)
                currentCahs.Balance -= operation.Amount;
            else
                currentCahs.Balance += operation.Amount;

            if (currentCahs.Balance == 0)
                _cash.Remove(currentCahs);
        }

        public void ProcessConversion(Conversion operation)
        {
            var cashFrom = GetCash(operation.From, operation.Account);
            if (cashFrom == null)
                cashFrom = InitializeCash(operation.From, operation.Account);

            var cashTo = GetCash(operation.To, operation.Account);
            if (cashTo == null)
                cashTo = InitializeCash(operation.To, operation.Account);

            var preview = PreviewConversion(operation);
            cashFrom.Balance -= preview.Minus;
            cashTo.Balance += preview.Plus;

            if (cashFrom.Balance == 0)
                _cash.Remove(cashFrom);

            if (cashTo.Balance == 0)
                _cash.Remove(cashTo);
        }

        public ConversionPreview PreviewConversion(Conversion operation)
        {
            var plus = operation.Type == ConversionType.Buy ? operation.Amount : operation.Amount * operation.Rate;
            var minus = operation.Type == ConversionType.Buy ? operation.Amount * operation.Rate : operation.Amount;
            return new ConversionPreview(plus, minus);
        }

        #region Private Methods
        private OperationCurrency GetCash(CurrencyName currency, CashAccount account)
        {
            return _cash.Collection.ByName(currency, account).SingleOrDefault();
        }

        private OperationCurrency InitializeCash(CurrencyName currency, CashAccount account)
        {
            var cash = new OperationCurrency()
            {
                Account = account,
                Name = currency,
                Balance = 0
            };
            _cash.Add(cash);

            return cash;
        }
        #endregion

        #region Event handlers
        private void OnOperationAdded(object sender, Operation e)
        {
            ProcessOperation(e);
        }

        private void OnOperationRemoved(object sender, Operation e)
        {
            e.Type = e.Type == OperationType.Expense ? OperationType.Income : OperationType.Expense;
            ProcessOperation(e);
        }

        private void OnConversionAdded(object sender, Conversion e)
        {
            ProcessConversion(e);
        }

        private void OnConversionRemoved(object sender, Conversion e)
        {
            var type = e.Type == ConversionType.Buy ? ConversionType.Sell : ConversionType.Buy;
            var from = e.To;
            var to = e.From;
            e.Type = type;
            e.From = from;
            e.To = to;
            ProcessConversion(e);
        }
        #endregion
    }
}
