using DroidRemote.Windows.Properties;
using System.Windows;

namespace DroidRemote.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Auto-upgrade
            if (Settings.Default.upgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.upgradeRequired = false;
            }
            base.OnStartup(e);
        }
    }
}