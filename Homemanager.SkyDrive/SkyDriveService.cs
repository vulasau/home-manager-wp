using Homemanager.SkyDrive.Interfaces;
using HomeManager.DataAccess.Interfaces;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Homemanager.SkyDrive
{
    public class SkyDriveService: ISkyDriveService
    {
        #region Dependencies
        private IDataContext _dataContext;
        private ISkyDriveFileManager _fileManager;
        #endregion

        #region Private fields
        private const string _folderName = "HomeManager";

        private Dictionary<string, bool> _pendingOperations;
        #endregion

        public SkyDriveService(IDataContext dataContext, ISkyDriveFileManager fileManager)
        {
            _dataContext = dataContext;
            _fileManager = fileManager;
            _fileManager.InitializationCompleted += OnInitialized;
            _fileManager.UploadCompleted += OnUploaded;
            _fileManager.DownloadCompleted += OnDownloaded;

            _pendingOperations = new Dictionary<string, bool>();
        }

        #region Public methods
        public void Initialize(LiveConnectSession session)
        {
            _fileManager.Initialize(_folderName, session);
        }

        public void Upload()
        {
            _pendingOperations.Clear();

            _fileManager.UploadFile(_dataContext.Operations.FileName);
            _fileManager.UploadFile(_dataContext.Conversions.FileName);
            _fileManager.UploadFile(_dataContext.Categories.FileName);
            _fileManager.UploadFile(_dataContext.Cash.FileName);
            _fileManager.UploadFile(_dataContext.Accounts.FileName);
        }

        public void Download()
        {
            _pendingOperations.Clear();

            _fileManager.DownloadFile(_dataContext.Operations.FileName);
            _fileManager.DownloadFile(_dataContext.Conversions.FileName);
            _fileManager.DownloadFile(_dataContext.Categories.FileName);
            _fileManager.DownloadFile(_dataContext.Cash.FileName);
            _fileManager.DownloadFile(_dataContext.Accounts.FileName);
        }
        #endregion

        #region File manager event handlers
        private void OnInitialized(object sender, bool e)
        {
            OnInitialized(e);
        }

        private void OnUploaded(string fileName, bool success)
        {
            _pendingOperations.Add(fileName, success);

            if (_pendingOperations.Count == 5)
                OnUploaded(_pendingOperations.All(x => x.Value == true));
        }

        private void OnDownloaded(string fileName, bool success)
        {
            _pendingOperations.Add(fileName, success);

            if (_pendingOperations.Count == 5)
                OnDownloaded(_pendingOperations.All(x => x.Value == true));
        }
        #endregion

        #region Events
        public event EventHandler<bool> Uploaded;
        public event EventHandler<bool> Downloaded;
        public event EventHandler<bool> Initialized;

        protected void OnInitialized(bool success)
        {
            if (Initialized != null)
                Initialized(this, success);
        }

        protected void OnUploaded(bool success)
        {
            if (Uploaded != null)
                Uploaded(this, success);
        }

        protected void OnDownloaded(bool success)
        {
            if (Downloaded != null)
                Downloaded(this, success);
        }
        #endregion
    }
}
