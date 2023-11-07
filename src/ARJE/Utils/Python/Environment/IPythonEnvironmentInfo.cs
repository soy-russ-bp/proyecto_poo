namespace ARJE.Utils.Python.Environment
{
    public interface IPythonEnvironmentInfo
    {
        public string? ActivationPath { get; }

        public string ExecutablePath { get; }
    }
}
