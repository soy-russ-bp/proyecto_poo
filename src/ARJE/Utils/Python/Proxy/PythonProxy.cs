using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using ARJE.Utils.IO.Pipes;
using ARJE.Utils.Python.Proxy.Packets;

namespace ARJE.Utils.Python.Proxy
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public sealed class PythonProxy : IDisposable
    {
        public PythonProxy(string pipeName, IIdMapper idMapper, int bufferCapacity = 0)
        {
            this.Pipe = new HybridPipeServer(pipeName);
            this.BufferCapacity = bufferCapacity;
        }

        public string PipeIdentifier => this.Pipe.Identifier;

        private HybridPipeServer Pipe { get; }

        private int BufferCapacity { get; }

        private PacketReader? Reader { get; set; }

        private PacketWriter? Writer { get; set; }

        public void Start()
        {
            Stream pipeStream = this.Pipe.WaitForConnection();
            this.CreateReaderAndWriter(pipeStream);
        }

        public async Task StartAsync()
        {
            Stream pipeStream = await this.Pipe.WaitForConnectionAsync();
            this.CreateReaderAndWriter(pipeStream);
        }

        public void Dispose()
        {
            this.Pipe.Dispose();
            this.Reader?.Dispose();
        }

        public TObject Receive<TObject, TPacket>()
            where TPacket : IInboundProxyPacket<TObject>, INoArgsPacket, new()
        {
            var packet = new TPacket();
            return this.Receive<TObject, TPacket>(packet);
        }

        public TObject Receive<TObject, TPacket>(TPacket packet)
            where TPacket : IInboundProxyPacket<TObject>
        {
            if (this.Reader == null)
            {
                ThrowNotStarted();
            }

            return this.Reader.ReadObject<TObject, TPacket>(packet);
        }

        public void Send<TPacket>(TPacket packet)
            where TPacket : IOutboundProxyPacket
        {
            if (this.Writer == null)
            {
                ThrowNotStarted();
            }

            this.Writer.WriteObject(packet);
        }

        [DoesNotReturn]
        private static void ThrowNotStarted()
        {
            throw new InvalidOperationException("Proxy not started.");
        }

        private void CreateReaderAndWriter(Stream pipeStream)
        {
            this.Reader = new PacketReader(pipeStream, this.BufferCapacity);
            this.Writer = new PacketWriter(pipeStream);
        }
    }
}
