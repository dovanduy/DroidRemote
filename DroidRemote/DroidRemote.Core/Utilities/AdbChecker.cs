using SharpAdbClient;

namespace DroidRemote.Core.Utilities
{
    public class AdbChecker
    {
        public static bool VerifyAdbExecutable(string path)
        {
            try
            {
                AdbServer.Instance.StartServer(path, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}