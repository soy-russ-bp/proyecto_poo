using System.IO;
using System.Threading.Tasks;
using Mono.Unix.Native;
using Spectre.Console;

namespace ARJE.Utils.IO.Pipes
{
    public sealed class PosixNamedPipe : INamedPipe
    {
        public PosixNamedPipe(string pipeName)
        {
            AnsiConsole.WriteLine("PosixNamedPipe()");
            this.PipePath = GetPipePath(pipeName);
            int result = Syscall.mkfifo(this.PipePath, FilePermissions.S_IRUSR | FilePermissions.S_IWUSR | FilePermissions.S_IFIFO);
            AnsiConsole.WriteLine("mkfifo: " + result); // DEBUG (TODO)
            this.FileStream = File.Open(this.PipePath, FileMode.Open);
            AnsiConsole.WriteLine("File.Open");
        }

        public Stream Stream => this.FileStream;

        public FileStream FileStream { get; }

        private string PipePath { get; }

        public static INamedPipe Create(string pipeName)
        {
            return new PosixNamedPipe(pipeName);
        }

        private static string GetPipePath(string pipeName)
        {
            string tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, pipeName);
        }

        public void Connect()
        {
        }

        public Task ConnectAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.FileStream.Dispose();
            Syscall.unlink(this.PipePath);
            File.Delete(this.PipePath);
        }
    }
}
