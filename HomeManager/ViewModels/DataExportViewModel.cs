using HomeManager.Core;
using HomeManager.Entities;
using HomeManager.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace HomeManager.ViewModels
{
    public class DataExportViewModel: AsyncViewModel
    {
        #region Dependencies
        private readonly IDataExportService _dataExportService;
        #endregion

        #region Public fields
        private bool _includeExpenses;
        public bool IncludeExpenses
        {
            get { return _includeExpenses; }
            set
            {
                _includeExpenses = value;
                OnPropertyChanged("IncludeExpenses", "Ready");
            }
        }

        private bool _includeIncomes;
        public bool IncludeIncomes
        {
            get { return _includeIncomes; }
            set
            {
                _includeIncomes = value;
                OnPropertyChanged("IncludeIncomes", "Ready");
            }
        }

        private bool _includeConversions;
        public bool IncludeConversions
        {
            get { return _includeConversions; }
            set
            {
                _includeConversions = value;
                OnPropertyChanged("IncludeConversions", "Ready");
            }
        }

        public bool HasConversions { get { return _dataContext.Conversions.Collection.Any(); } }
        public bool HasExpenses { get { return _dataContext.Operations.Collection.Any(x => x.Type == Entities.Enums.OperationType.Expense); } }
        public bool HasIncomes { get { return _dataContext.Operations.Collection.Any(x => x.Type == Entities.Enums.OperationType.Income); } }
        public bool Ready { get { return IncludeExpenses || IncludeIncomes || IncludeConversions; } }
        #endregion

        public DataExportViewModel()
            : base()
        {
            _dataExportService = App.Container.GetInstance<IDataExportService>();

            IncludeExpenses = HasExpenses;
            IncludeIncomes = HasIncomes;
            IncludeConversions = HasConversions;
        }

        #region UI event handlers
        public void OnExport()
        {
            OnStarted(Resources.AppResources.UploadProgressMessage);

            var files = new List<string>();

            if (IncludeExpenses)
            {
                var expenses = _dataContext.Operations.Collection.Where(x => x.Type == Entities.Enums.OperationType.Expense).OrderByDescending(x => x.Date);
                _dataExportService.Export<Operation>(expenses, "Expenses.csv");
                files.Add("Expenses.csv");
            }
            if (IncludeIncomes)
            {
                var incomes = _dataContext.Operations.Collection.Where(x => x.Type == Entities.Enums.OperationType.Income).OrderByDescending(x => x.Date);
                _dataExportService.Export<Operation>(incomes, "Incomes.csv");
                files.Add("Incomes.csv");
            }
            if (IncludeConversions)
            {
                _dataExportService.Export<Conversion>(_dataContext.Conversions.Collection, "Conversions.csv");
                files.Add("Conversions.csv");
            }

            OnCompleted(string.Format(Resources.AppResources.DataExportCompleteMessage, string.Join(", ", files)));
        }
        #endregion
    }
}
