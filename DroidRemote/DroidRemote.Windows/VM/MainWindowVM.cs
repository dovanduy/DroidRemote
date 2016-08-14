using DroidRemote.Core.States;
using DroidRemote.Core.Utilities;
using DroidRemote.Windows.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NanoMvvm;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DroidRemote.Windows.VM
{
    internal class MainWindowVM : ViewModelBase
    {
        public ICommand ConnectToDeviceCommand => new DelegateCommand(ConnectToDevice);
        public ICommand VisitIridiumIonSiteCommand => new DelegateCommand(VisitIridiumIonSite);
        public ICommand VisitProductHomeCommand => new DelegateCommand(VisitProductHome);

        private void VisitProductHome(object obj)
        {
            _introState.VisitProductHome();
        }

        private void VisitIridiumIonSite(object obj)
        {
            _introState.VisitIridiumIonSite();
        }

        private WindowService _windowService = new WindowService();
        private IntroState _introState = new IntroState();

        private async void ConnectToDevice(object obj)
        {
            if (!await VerifyAdbPath())
            {
                //Adb could not be verified.
                var tryAgain = await (View as MetroWindow).ShowMessageAsync("Error", "Oops! A valid ADB executable could not be found in that path. Verify and try again?");
                return;
            }

            //Launch the viewer
            (View as MetroWindow).Hide();
            _windowService.ShowWindowDialog<RemoteViewer>(View.WindowHandle);
            (View as MetroWindow).Show();
        }

        private async Task<bool> VerifyAdbPath()
        {
            if (!CheckAdbAvailability(null))
            {
                //Adb was not available. Ask now.
                string inputAdbPath = await (View as MetroWindow).ShowInputAsync("Please locate ADB", "Please enter the full path to or containing folder of the ADB executable. Alternatively, enter the path to the Android SDK. DroidManager uses ADB to communicate with your device.");
                //Verify ADB executable
                Tuple<bool, string> findResult;
                if (!String.IsNullOrWhiteSpace(inputAdbPath) && (findResult = await Task.Run(() => SmartFindAdb(inputAdbPath))).Item1 && await Task.Run(() => AdbChecker.VerifyAdbExecutable(findResult.Item2)))
                {
                    //Adb has been verified, save new settings
                    Properties.Settings.Default.adbExecutablePath = findResult.Item2;
                    Properties.Settings.Default.Save();
                    await (View as MetroWindow).ShowMessageAsync("Success", "We're all set! We found a working ADB executable!");
                    return true; //Success
                }
                else
                {
                    //await (View as MetroWindow).ShowMessageAsync("Error", "Sorry, we couldn't find a valid ADB executable in that path.");
                    return false; //Validation failure
                }
            }
            return true; //Adb is already available
        }

        /// <summary>
        /// Attempts to find the ADB executable even if the input string is not valid
        /// </summary>
        /// <param name="inputAdbPath"></param>
        /// <returns></returns>
        public static Tuple<bool, string> SmartFindAdb(string inputAdbPath)
        {
            string originalPath = inputAdbPath;
            if (CheckAdbAvailability(inputAdbPath))
            {
                return new Tuple<bool, string>(true, inputAdbPath); //It works right away! :D
            }
            else
            {
                //Some tweaking is required.

                //First try removing quotes!
                inputAdbPath = inputAdbPath.Replace("\"", ""); //Remove the double-quote character
                if (CheckAdbAvailability(inputAdbPath))
                {
                    return new Tuple<bool, string>(true, inputAdbPath); //Got it!
                }

                //Now try appending adb.exe
                inputAdbPath = Path.Combine(inputAdbPath, "adb.exe");
                if (CheckAdbAvailability(inputAdbPath))
                {
                    return new Tuple<bool, string>(true, inputAdbPath); //Got it!
                }

                //Now try platform-tools/adb.exe
                inputAdbPath = Path.Combine(Path.Combine(originalPath, "platform-tools"), "adb.exe");
                if (CheckAdbAvailability(inputAdbPath))
                {
                    return new Tuple<bool, string>(true, inputAdbPath); //Got it!
                }

                //Sorry, out of luck
            }

            return new Tuple<bool, string>(true, inputAdbPath); //Adb could not be found :(
        }

        public static bool CheckAdbAvailability(string testPath)
        {
            string adbPath = testPath ?? Properties.Settings.Default.adbExecutablePath;
            if (String.IsNullOrWhiteSpace(adbPath)) return false; //Setting is unset
            if (!File.Exists(adbPath)) return false; //Adb not found
            return true; //Adb is available
        }
    }
}