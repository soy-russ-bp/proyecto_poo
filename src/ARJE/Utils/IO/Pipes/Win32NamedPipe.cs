#if OS_WINDOWS
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace ARJE.Utils.IO.Pipes
{
    public sealed class Win32NamedPipe : INamedPipe
    {
        public Win32NamedPipe(string pipeName)
        {
            ArgumentNullException.ThrowIfNull(pipeName);

            this.NameOrPath = pipeName;
            this.PipeStream = new NamedPipeClientStream(pipeName);
        }

        public string NameOrPath { get; }

        public bool Connected { get; private set; }

        private NamedPipeClientStream PipeStream { get; }

        public static INamedPipe Create(string pipeName)
        {
            return new Win32NamedPipe(pipeName);
        }

        public Stream Connect()
        {
            PipeUtils.AssertNotConnected(this);

            this.PipeStream.Connect();
            this.Connected = true;
            return this.PipeStream;
        }

        public Task<Stream> ConnectAsync()
        {
            return Task.Run(this.Connect);
        }

        public void Dispose()
        {
            this.PipeStream.Dispose();
        }
    }
}
#endif
