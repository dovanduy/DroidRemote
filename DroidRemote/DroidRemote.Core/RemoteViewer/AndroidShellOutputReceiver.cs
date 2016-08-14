using SharpAdbClient;
using System.IO;

namespace DroidRemote.Core.RemoteViewer
{
    internal class AndroidShellOutputReceiver : IShellOutputReceiver
    {
        public Stream ShellOutputStream;
        public bool ParsesErrors => false;

        private StreamWriter _outputStreamWriter;

        public AndroidShellOutputReceiver()
        {
            ShellOutputStream = new MemoryStream();
            _outputStreamWriter = new StreamWriter(ShellOutputStream) { AutoFlush = true };
        }

        public void AddOutput(string line)
        {
            _outputStreamWriter.WriteLine(line);
        }

        public void Flush()
        {
            _outputStreamWriter.Flush();
        }
    }
}