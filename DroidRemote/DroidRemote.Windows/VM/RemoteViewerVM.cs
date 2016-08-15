using DroidRemote.Core.States;
using DroidRemote.Windows.MvvmExt;
using DroidRemote.Windows.Properties;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NanoMvvm;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DroidRemote.Windows.VM
{
    internal class RemoteViewerVM : ImageStreamingViewModel
    {
        public ICommand ViewLoadedCommand => new DelegateCommand(ViewDidLoad);
        private BitmapImage _deviceScreenImageSource;

        private readonly RemoteViewerState _remoteViewerState = new RemoteViewerState(Settings.Default.adbExecutablePath);

        private async void ViewDidLoad(object obj)
        {
            await PrepareConnection();
        }

        private async Task PrepareConnection()
        {
            //Get the connection ready
            var progressController = await (View as MetroWindow).ShowProgressAsync("Connecting", "Establishing a connection");
            progressController.SetIndeterminate();
            await _remoteViewerState.PrepareServer();
            progressController.SetMessage("Preparing device");
            var connectionStatus = _remoteViewerState.ConnectToDevice();
            if (!connectionStatus)
            {
                //Could not connect to device
                await progressController.CloseAsync();
                await (View as MetroWindow).ShowMessageAsync("Error", "The device could not be found");
                return;
            }

            progressController.SetTitle("Preparing");
            progressController.SetMessage("Receiving screen");
            //Get first image and display
            var screenImageStream = await _remoteViewerState.GetScreenImageStreamViaProcess();

            /*
            //Dump to file
            using (var firstImageStream = File.OpenWrite("firstimage.png"))
            {
                screenImageStream.Position = 0; //Reset position
                screenImageStream.CopyTo(firstImageStream);
            }
            */

            _deviceScreenImageSource = new BitmapImage();

            using (screenImageStream)
            {
                _deviceScreenImageSource.BeginInit();
                _deviceScreenImageSource.StreamSource = screenImageStream;
                _deviceScreenImageSource.EndInit();

                ImageDisplayView.ImageControl.Source = _deviceScreenImageSource;
            }

            //We're ready!
            await progressController.CloseAsync();
        }

        private static void SaveBitmapImageToFile(BitmapImage image, string fileName)
        {
            using (var ofs = File.OpenWrite(fileName))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)image));
                encoder.Save(ofs);
            }
        }
    }
}