using Homemanager.SkyDrive;
using Homemanager.SkyDrive.Interfaces;
using HomeManager.Cash;
using HomeManager.Cash.Interfaces;
using HomeManager.Core;
using HomeManager.DataAccess;
using HomeManager.DataAccess.Interfaces;
using HomeManager.DataExport;
using HomeManager.Infrastructure.Interfaces;
using HomeManager.PhoneMemoryAccess;
using HomeManager.PhoneMemoryAccess.Interfaces;
using HomeManager.Rates;
using HomeManager.Rates.Interfaces;
using HomeManager.Services;
using HomeManager.Services.Interfaces;
using HomeManager.Statistics;
using HomeManager.Statistics.Interfaces;
using HomeManager.Tools;

namespace HomeManager.Infrastructure
{
    public class BootStrapper
    {
        private IContainer _container;

        public BootStrapper()
        {
            _container = App.Container;
        }

        public void Initialize()
        {
            var memoryService = new PhoneMemoryService();
            var iconsService = new IconsService();
            var statisticsManager = new StatisticsManager();

            var options = new Options(memoryService);
            var dataContext = new DataContext(memoryService);

            var skyDriveFileManager = new SkyDriveFileManager(memoryService);
            var skyDriveService = new SkyDriveService(dataContext, skyDriveFileManager);

            var csvDataExportService = new CsvDataExportService(memoryService);

            var cashService = new CashService(dataContext);
            var tileService = new TileService(dataContext);
            var categoryUsageService = new CategoryUsageService(dataContext.Operations);
            var dependenciesService = new DependenciesService(dataContext, iconsService);
            var defaultCategoriesInitializer = new DefaultCategoriesInitializer(dataContext, iconsService);
            var accountInitializer = new AccountsInitializer(dataContext);

            var ratesHtmlParser = new RatesHtmlParser();
            var ratesWebClient = new RatesWebClient();
            var ratesService = new RatesService(memoryService, ratesHtmlParser, ratesWebClient);
            var depositCalculator = new DepositCalculator();
            var creaditCalculator = new CreditCalculator();

            _container.RegisterInstance<IPhoneMemoryService>(memoryService);
            _container.RegisterInstance<IIconsService>(iconsService);
            _container.RegisterInstance<IStatisticsManager>(statisticsManager);

            _container.RegisterInstance<Options>(options);         
            _container.RegisterInstance<IDataContext>(dataContext);

            _container.RegisterInstance<ISkyDriveService>(skyDriveService);
            _container.RegisterInstance<IDataExportService>(csvDataExportService);
            _container.RegisterInstance<ICashService>(cashService);
            _container.RegisterInstance<IRatesService>(ratesService);
            _container.RegisterInstance<DepositCalculator>(depositCalculator);
            _container.RegisterInstance<CreditCalculator>(creaditCalculator);
        }
    }
}
