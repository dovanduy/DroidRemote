using DroidRemote.Core.States;
using DroidRemote.Windows.MvvmExt;
using DroidRemote.Windows.Properties;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NanoMvvm;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DroidRemote.Windows.VM
{
    internal class RemoteViewerVM : ImageStreamingViewModel
    {
        public ICommand ViewLoadedCommand => new DelegateCommand(ViewDidLoad);
        private ImageSource _deviceScreenImageSource;

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

            using (screenImageStream)
            {
                _deviceScreenImageSource = new BitmapImage();
                var bitmapScreenSource = (_deviceScreenImageSource as BitmapImage);
                bitmapScreenSource.BeginInit();
                bitmapScreenSource.StreamSource = screenImageStream;
                bitmapScreenSource.EndInit();
            }

            ImageDisplayView.ImageControl.Source = _deviceScreenImageSource;

            //We're ready!
            await progressController.CloseAsync();
        }
    }
}