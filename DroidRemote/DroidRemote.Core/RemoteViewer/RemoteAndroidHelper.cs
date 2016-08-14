using SharpAdbClient;
using SharpAdbClient.DeviceCommands;

namespace DroidRemote.Core.RemoteViewer
{
    internal class RemoteAndroidHelper
    {
        public DeviceData CurrentDevice { get; }

        public RemoteAndroidHelper(DeviceData device)
        {
            CurrentDevice = device;
        }

        public AndroidShellOutputReceiver ExecuteDeviceShellCommand(string command)
        {
            var outputReceiver = new AndroidShellOutputReceiver();
            CurrentDevice.ExecuteShellCommand(command, outputReceiver);
            return outputReceiver;
        }

        public void SendInputCommand(string command)
        {
            ExecuteDeviceShellCommand($"input {command}");
        }

        public void PerformTap(double x, double y)
        {
            SendInputCommand($"tap {x} {y}");
        }

        public void PerformSwipe(double x1, double y1, double x2, double y2, long duration)
        {
            SendInputCommand($"swipe {x1} {y1} {x2} {y2} {duration}");
        }

        public void PerformKeypress(int keyCode)
        {
            SendInputCommand($"keyevent {keyCode}");
        }

        public void PerformKeypress(char key)
        {
            SendInputCommand($"keyevent {key}");
        }

        public void PerformKeypress(string keyName)
        {
            SendInputCommand($"keyevent {keyName}");
        }

        public void PerformTextInput(string text)
        {
            SendInputCommand($"text {text}");
        }
    }
}