using Homemanager.SkyDrive.Interfaces;
using HomeManager.ViewModels.Base;
using Microsoft.Live;
using Microsoft.Phone.Net.NetworkInformation;

namespace HomeManager.ViewModels
{
    public class SkyDriveViewModel: AsyncViewModel
    {
        #region Dependencies
        private ISkyDriveService _skyDriveService;
        #endregion

        #region Public fields
        private bool _initialized;
        public bool Initialized
        {
            get { return _initialized; }
            set
            {
                _initialized = value;
                OnPropertyChanged("Initialized");
            }
        }

        public bool HasInternet { get { return NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None; } }
        #endregion

        public SkyDriveViewModel()
            : base()
        {
            _skyDriveService = App.Container.GetInstance<ISkyDriveService>();
            _skyDriveService.Initialized += OnInitialized;
            _skyDriveService.Uploaded += OnUploaded;
            _skyDriveService.Downloaded += OnDownloaded;
        }

        #region UI event handlers
        public void OnSessionChanged(LiveConnectSession session)
        {
            try
            {
                if (session != null)
                {
                    OnStarted(Resources.AppResources.InitializeProgressMessage);
                    _skyDriveService.Initialize(session);
                }
                else
                {
                    Initialized = false;
                }
            }
            catch
            {
                OnFailed(Resources.AppResources.InitializeErrorMessage);
            }
        }

        public void OnUpload()
        {
            OnStarted(Resources.AppResources.UploadProgressMessage);
            _dataContext.Save();
            _skyDriveService.Upload();
        }

        public void OnDownload()
        {
            OnStarted(Resources.AppResources.DownloadProgressMessage);
            _skyDriveService.Download();
        }

        public override void Update()
        {
            base.Update();
            OnPropertyChanged("HasInternet");
        }
        #endregion

        #region Service event handlers
        private void OnInitialized(object sender, bool e)
        {
            Initialized = e;
            OnCompleted();
        }

        private void OnUploaded(object sender, bool e)
        {
            string message = e ? Resources.AppResources.UploadCompleteMessage : Resources.AppResources.UploadErrorMessage;
            OnCompleted(message);
        }

        private void OnDownloaded(object sender, bool e)
        {
            _dataContext.Load();

            string message = e ? Resources.AppResources.DownloadCompleteMessage : Resources.AppResources.DownloadErrorMessage;
            OnCompleted(message);
        }
        #endregion
    }
}
