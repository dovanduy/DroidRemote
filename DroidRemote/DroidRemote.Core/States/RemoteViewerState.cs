using SharpAdbClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroidRemote.Core.States
{
    public class RemoteViewerState
    {
        public DeviceData ConnectedDevice;
        public string AdbExecutablePath;

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
            return true;
        }

        public List<DeviceData> GetAvailableDevices()
        {
            var devices = AdbClient.Instance.GetDevices();
            //return devices.Select(device=>device.Serial).ToList();
            return devices;
        }
    }
}