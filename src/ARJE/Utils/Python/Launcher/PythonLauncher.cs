using System.Runtime.Versioning;
using ARJE.Utils.Diagnostics.CommandLine;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;
using ARJE.Utils.Text;

namespace ARJE.Utils.Python
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public class PythonLauncher<TEnv>
        where TEnv : IPythonEnvironmentInfo
    {
        public PythonLauncher(PythonAppInfo<TEnv> appInfo)
        {
            this.AppInfo = appInfo;
        }

        public PythonAppInfo<TEnv> AppInfo { get; }

        public void Run(string? windowName = null, params string[] args)
        {
            string argsJoin = ArgsUtils.Join(args);

            var commands = new string?[]
            {
                windowName != null ? CLI.GetTitleCommand(windowName) : null,
                this.AppInfo.EnvironmentInfo.ActivationPath,
                $"{this.AppInfo.EnvironmentInfo.ExecutablePath} -m {this.AppInfo.StartupScriptName} {argsJoin}",
            };

            CLI.Execute(this.AppInfo.Directory, commands);
        }
    }
}
