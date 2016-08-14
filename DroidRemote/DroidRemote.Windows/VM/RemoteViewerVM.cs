using DroidRemote.Core.States;
using DroidRemote.Windows.Properties;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NanoMvvm;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DroidRemote.Windows.VM
{
    internal class RemoteViewerVM : ViewModelBase
    {
        public ICommand ViewLoadedCommand => new DelegateCommand(ViewDidLoad);

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
            //We're ready!
            await progressController.CloseAsync();
        }
    }
}