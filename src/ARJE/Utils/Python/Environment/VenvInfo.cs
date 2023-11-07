using System;
using System.IO;

namespace ARJE.Utils.Python.Environment
{
    public class VenvInfo : IPythonEnvironmentInfo
    {
        public VenvInfo(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);

            this.Name = name;
        }

        public string Name { get; }

        public string? ActivationPath => Path.Combine(this.Name, "Scripts", "activate");

        public string ExecutablePath => Path.Combine(this.Name, "Scripts", "python.exe");
    }
}
