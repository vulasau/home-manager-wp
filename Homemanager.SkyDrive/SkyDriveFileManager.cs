using Homemanager.SkyDrive.Interfaces;
using HomeManager.PhoneMemoryAccess.Interfaces;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Homemanager.SkyDrive
{
    public class SkyDriveFileManager: ISkyDriveFileManager
    {
        #region Dependencies
        private IPhoneMemoryService _memoryService;
        #endregion

        #region Private fields
        private string _folderId;

        private LiveConnectClient _client;
        private LiveConnectSession _session;
        #endregion

        #region Constructors
        public SkyDriveFileManager(IPhoneMemoryService memoryService)
        {
            _memoryService = memoryService;
        }
        #endregion

        #region Public methods
        public async void Initialize(string folderName, LiveConnectSession session)
        {
            _session = session;
            _client = new LiveConnectClient(_session);

            try
            {
                _folderId = await GetSkyDriveFolderId(folderName);
                if (string.IsNullOrEmpty(_folderId))
                    _folderId = await CreateSkydriveFolder(folderName);
                OnInitializationCompleted(true);
            }
            catch
            {
                OnInitializationCompleted(false);
            }
        }

        public async void UploadFile(string fileName)
        {
            try
            {
                var stream = _memoryService.ReadFile(fileName);
                await _client.UploadAsync(_folderId, fileName, stream, OverwriteOption.Overwrite);
                OnUploadCompleted(fileName, true);
            }
            catch (Exception)
            {
                OnUploadCompleted(fileName, false);
            }
        }

        public async void DownloadFile(string fileName)
        {
            string fileId = await GetFileId(fileName);
            if (!string.IsNullOrEmpty(fileId))
            {
                Uri source = await GetFileSourcePath(fileId);
                GetFileContent(source, fileName);
            }
            else
            {
                OnDownloadCompleted(fileName, true);
            }
        }
        #endregion

        #region Private methods
        private async Task<string> GetSkyDriveFolderId(string folderName)
        {
            var result = await _client.GetAsync("me/skydrive/files/");
            var data = (List<object>)result.Result["data"];
            foreach (IDictionary<string, object> content in data)
            {
                if (content["name"].ToString().Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                    return content["id"].ToString();
            }
            return string.Empty;
        }

        private async Task<string> CreateSkydriveFolder(string folderName)
        {
            var folderData = new Dictionary<string, object>();
            folderData.Add("name", folderName);
            var operationResult = await _client.PostAsync("me/skydrive", folderData);
            dynamic result = operationResult.Result;
            return string.Format("{0}", result.id);
        }

        private async Task<string> GetFileId(string fileName)
        {
            var fileResult = await _client.GetAsync(_folderId + "/files");
            var fileData = (List<object>)fileResult.Result["data"];
            foreach (IDictionary<string, object> content in fileData)
            {
                if (content["name"].ToString().Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
                    return content["id"].ToString();
            }
            return string.Empty;
        }

        private async Task<Uri> GetFileSourcePath(string fileId)
        {
            var res = await _client.GetAsync(fileId);
            var sourcePath = res.Result["source"].ToString();
            return new Uri(sourcePath);
        }

        private void GetFileContent(Uri source, string fileName)
        {
            var webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDownloadCompleted;
            webClient.DownloadStringAsync(source, fileName);
        }
        #endregion

        #region WebClient event handlers
        void OnDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            bool success = e.Error == null;
            _memoryService.WriteFile(e.UserState.ToString(), e.Result);
            OnDownloadCompleted(e.UserState.ToString(), success);
        }
        #endregion

        #region Events
        public event EventHandler<bool> InitializationCompleted;
        public event OperationEventHandler UploadCompleted;
        public event OperationEventHandler DownloadCompleted;

        private void OnInitializationCompleted(bool success)
        {
            if (InitializationCompleted != null)
                InitializationCompleted(this, success);
        }

        private void OnUploadCompleted(string fileName, bool success)
        {
            if (UploadCompleted != null)
                UploadCompleted(fileName, success);
        }

        private void OnDownloadCompleted(string fileName, bool success)
        {
            if (DownloadCompleted != null)
                DownloadCompleted(fileName, success);
        }
        #endregion
    }
}
