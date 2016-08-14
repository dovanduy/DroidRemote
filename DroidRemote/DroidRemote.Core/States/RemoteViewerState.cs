using DroidRemote.Core.RemoteViewer;
using SharpAdbClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DroidRemote.Core.States
{
    public class RemoteViewerState
    {
        public DeviceData ConnectedDevice;
        public string AdbExecutablePath;

        private ScreenStreamer _screenStreamer;

        public RemoteViewerState(string adbExecutablePath)
        {
            AdbExecutablePath = adbExecutablePath;
        }

        public async Task PrepareServer()
        {
            //Get server ready
            await Task.Run(() => AdbServer.Instance.StartServer(AdbExecutablePath, true));
        }

        public bool ConnectToDevice()
        {
            var availableDevices = GetAvailableDevices();
            if (availableDevices.Count == 0)
            {
                return false;
            }
            ConnectedDevice = availableDevices[0]; //TODO: Use a better method later

            //Create utility objects
            _screenStreamer = new ScreenStreamer(ConnectedDevice, AdbExecutablePath);

            return true;
        }

        public List<DeviceData> GetAvailableDevices()
        {
            var devices = AdbClient.Instance.GetDevices();
            //return devices.Select(device=>device.Serial).ToList();
            return devices;
        }

        public async Task<MemoryStream> GetScreenImageStreamViaProcess()
        {
            var screenshotStream = await Task.Run(()=>_screenStreamer.GetImageStreamScreenshotViaProcess());
            return screenshotStream;
        }
    }
}