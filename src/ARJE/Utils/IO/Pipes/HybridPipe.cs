using System;
using System.IO;
using System.IO.Pipes;

namespace ARJE.Utils.IO.Pipes
{
    public sealed class HybridPipe : IPipeServer
    {
        private PipeState state;

        public HybridPipe(string pipeName)
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

        public PipeState State
        {
            get
            {
                if (!this.PipeStream.IsConnected)
                {
                    this.state = PipeState.NotConnected;
                }

                return this.state;
            }

            private set => this.state = value;
        }

        private NamedPipeServerStream PipeStream { get; }

        public Stream WaitForConnection()
        {
            PipeUtils.AssertNotConnectingOrConnected(this);

            this.State = PipeState.Connecting;
            this.PipeStream.WaitForConnection();
            this.State = PipeState.Connected;
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
