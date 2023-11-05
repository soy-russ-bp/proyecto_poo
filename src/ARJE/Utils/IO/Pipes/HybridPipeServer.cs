using System;
using System.IO;
using System.IO.Pipes;

namespace ARJE.Utils.IO.Pipes
{
    public sealed class HybridPipeServer : IPipeServer
    {
        public HybridPipeServer(string pipeName)
        {
            ArgumentNullException.ThrowIfNull(pipeName);

            this.Identifier = GetIdentifier(pipeName);
            this.PipeStream = new NamedPipeServerStream(
                pipeName,
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.CurrentUserOnly);
        }

        public string Identifier { get; }

        public bool IsConnected => this.PipeStream.IsConnected;

        private NamedPipeServerStream PipeStream { get; }

        public Stream WaitForConnection()
        {
            this.PipeStream.WaitForConnection();
            return this.PipeStream;
        }

        public void Dispose()
        {
            this.PipeStream.Dispose();
        }

        private static string GetIdentifier(string pipeName)
        {
#if OS_LINUX || OS_MAC
            return Path.Join("tmp", $"CoreFxPipe_{pipeName}.fifo");
#else
            return pipeName;
#endif
        }
    }
}
