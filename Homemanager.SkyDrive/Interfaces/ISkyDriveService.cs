using Microsoft.Live;
using System;

namespace Homemanager.SkyDrive.Interfaces
{
    public interface ISkyDriveService
    {
        void Initialize(LiveConnectSession session);
        void Upload();
        void Download();

        event EventHandler<bool> Uploaded;
        event EventHandler<bool> Downloaded;
        event EventHandler<bool> Initialized;
    }
}
