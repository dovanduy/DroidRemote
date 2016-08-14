using NanoMvvm;
using System.Windows;

namespace DroidRemote.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IView
    {
        public MainWindow()
        {
            InitializeComponent();
            (DataContext as ViewModelBase).View = this as IView;
        }

        public Window WindowHandle => this;
    }
}