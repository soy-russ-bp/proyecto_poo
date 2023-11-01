#if OS_WINDOWS
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace ARJE.Utils.IO.Pipes
{
    public sealed class Win32NamedPipe : INamedPipe
    {
        public Win32NamedPipe(string pipeName)
        {
            this.NameOrPath = pipeName;
            this.PipeStream = new NamedPipeClientStream(pipeName);
        }

        public string NameOrPath { get; }

        public Stream Stream => this.PipeStream;

        private NamedPipeClientStream PipeStream { get; }

        public static INamedPipe Create(string pipeName)
        {
            return new Win32NamedPipe(pipeName);
        }

        public void Connect()
        {
            this.PipeStream.Connect();
        }

        public Task ConnectAsync()
        {
            return this.PipeStream.ConnectAsync();
        }

        public void Dispose()
        {
            this.PipeStream.Dispose();
        }
    }
}
#endif
