using Microsoft.Live;
using System;
using System.Threading.Tasks;

namespace Homemanager.SkyDrive.Interfaces
{
    public delegate void OperationEventHandler(string fileName, bool success);

    public interface ISkyDriveFileManager
    {
        void Initialize(string folderName, LiveConnectSession session);
        void UploadFile(string fileName);
        void DownloadFile(string fileName);

        event EventHandler<bool> InitializationCompleted;
        event OperationEventHandler UploadCompleted;
        event OperationEventHandler DownloadCompleted;
    }
}
