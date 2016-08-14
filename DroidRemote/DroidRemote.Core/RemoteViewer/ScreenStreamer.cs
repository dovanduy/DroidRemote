using SharpAdbClient;
using System.Diagnostics;
using System.IO;

namespace DroidRemote.Core.RemoteViewer
{
    internal class ScreenStreamer
    {
        public DeviceData CurrentDevice { get; }
        public RemoteAndroidHelper DeviceHelper { get; }
        public string AdbExecutablePath { get; }

        public ScreenStreamer(DeviceData targetDevice, string adbExecutablePath)
        {
            CurrentDevice = targetDevice;
            DeviceHelper = new RemoteAndroidHelper(CurrentDevice);
            AdbExecutablePath = adbExecutablePath;
        }

        public MemoryStream GetImageStreamScreenshotViaProcess()
        {
            var streamImageProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = AdbExecutablePath,
                    Arguments = "exec-out screencap -p",
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                },
            };
            var receiveImageStream = new MemoryStream();
            streamImageProcess.Start();
            streamImageProcess.StandardOutput.BaseStream.CopyTo(receiveImageStream);
            streamImageProcess.WaitForExit();
            return receiveImageStream;
        }
    }
}