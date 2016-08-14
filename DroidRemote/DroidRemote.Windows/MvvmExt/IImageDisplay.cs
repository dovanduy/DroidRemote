using NanoMvvm;
using System.Windows.Controls;

namespace DroidRemote.Windows.MvvmExt
{
    public interface IImageDisplayView : IView
    {
        Image ImageControl { get; }
    }
}