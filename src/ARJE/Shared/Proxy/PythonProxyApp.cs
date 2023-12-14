using ARJE.Utils.IO;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;

namespace ARJE.Shared.Proxy
{
    public static class PythonProxyApp
    {
        public static PythonAppInfo<VenvInfo> AppInfo { get; } = GetAppInfo();

        private static PythonAppInfo<VenvInfo> GetAppInfo()
        {
            DirectoryInfo proxyDir = GetProxyDirectory();
            PythonAppInfo<VenvInfo> appInfo = new(new VenvInfo(".venv"), proxyDir, "app");
            return appInfo;
        }

        private static DirectoryInfo GetProxyDirectory()
        {
            string searchPath = GetProxySearchPath();
            string dirPath = Path.Combine(searchPath, "PythonProxy");
            return new DirectoryInfo(dirPath);
        }

        private static string GetProxySearchPath()
        {
            string searchPath = AppContext.BaseDirectory;
#if DEBUG
            searchPath = PathUtils.GoUpToFolder(searchPath, "ARJE");
#endif
            return searchPath;
        }
    }
}
