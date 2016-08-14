using NanoMvvm;
using System.Windows;

namespace DroidRemote.Windows.Views
{
    /// <summary>
    /// Interaction logic for RemoteViewer.xaml
    /// </summary>
    public partial class RemoteViewer : IView
    {
        public RemoteViewer()
        {
            InitializeComponent();
            (DataContext as ViewModelBase).View = this as IView;
        }

        public Window WindowHandle => this;
    }
}