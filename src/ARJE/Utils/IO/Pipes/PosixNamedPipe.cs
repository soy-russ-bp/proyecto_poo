#if OS_MAC || DEBUG
using System;
using System.IO;
using System.Threading.Tasks;
using Mono.Unix.Native;
using Spectre.Console;

namespace ARJE.Utils.IO.Pipes
{
    public sealed class PosixNamedPipe : INamedPipe
    {
        private const FilePermissions FifoMode = FilePermissions.S_IRUSR | FilePermissions.S_IWUSR | FilePermissions.S_IFIFO;

        public PosixNamedPipe(string pipeName)
        {
            ArgumentNullException.ThrowIfNull(pipeName);

            this.NameOrPath = GetPipePath(pipeName);
            File.Delete(this.NameOrPath);
        }

        public string NameOrPath { get; }

        public bool Connected { get; private set; }

        private Stream? Stream { get; set; }

        public static INamedPipe Create(string pipeName)
        {
            return new PosixNamedPipe(pipeName);
        }

        public Stream Connect()
        {
            PipeUtils.AssertNotConnected(this);

            int result = Syscall.mkfifo(this.NameOrPath, FifoMode);
            AnsiConsole.WriteLine("mkfifo: " + result); // DEBUG (TODO)
            this.Stream = new FileStream(this.NameOrPath, FileMode.Open, FileAccess.Read, FileShare.Write, 0);
            this.Connected = true;
            return this.Stream;
        }

        public Task<Stream> ConnectAsync()
        {
            return Task.Run(this.Connect);
        }

        public void Dispose()
        {
            this.Stream?.Dispose();
            Syscall.unlink(this.NameOrPath);
            File.Delete(this.NameOrPath);
        }

        private static string GetPipePath(string pipeName)
        {
            string tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, pipeName + ".pipe");
        }
    }
}
#endif
