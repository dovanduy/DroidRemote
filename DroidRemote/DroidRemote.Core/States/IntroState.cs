using System;
using System.Diagnostics;

namespace DroidRemote.Core.States
{
    public class IntroState
    {
        public void VisitIridiumIonSite()
        {
            Process.Start("https://iridiumion.xyz");
        }

        public void VisitProductHome()
        {
            Process.Start("https://iridiumion.xyz/projects/droidmanager");
        }
    }
}