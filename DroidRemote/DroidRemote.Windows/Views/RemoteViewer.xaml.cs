using DroidRemote.Windows.MvvmExt;
using NanoMvvm;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DroidRemote.Windows.Views
{
    /// <summary>
    /// Interaction logic for RemoteViewer.xaml
    /// </summary>
    public partial class RemoteViewer : IImageDisplayView
    {
        public RemoteViewer()
        {
            InitializeComponent();
            (DataContext as ViewModelBase).View = this as IView;
            (DataContext as ImageStreamingViewModel).ImageDisplayView = this as IImageDisplayView;
        }

        public Window WindowHandle => this;

        public Image ImageControl => deviceImage;

        //MaterialButton Events

        public void ExitToAppClicked(object sender, EventArgs e)
        {

        }

    }
}