using System;
using System.IO;
using ARJE.Utils.Python.Environment;

namespace ARJE.Utils.Python.Launcher
{
    public readonly struct PythonAppInfo<TEnv>
        where TEnv : IPythonEnvironmentInfo
    {
        public PythonAppInfo(TEnv environmentInfo, DirectoryInfo directory, string startupScriptName)
        {
            ArgumentNullException.ThrowIfNull(environmentInfo);
            ArgumentNullException.ThrowIfNull(directory);
            ArgumentNullException.ThrowIfNull(startupScriptName);

            this.EnvironmentInfo = environmentInfo;
            this.Directory = directory;
            this.StartupScriptName = startupScriptName;
        }

        public TEnv EnvironmentInfo { get; }

        public DirectoryInfo Directory { get; }

        public string StartupScriptName { get; }
    }
}
