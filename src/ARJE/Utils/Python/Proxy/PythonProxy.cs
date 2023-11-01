using System;
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
            this.Pipe = PlatformNamedPipe.Create(pipeName);
            this.PipeName = this.Pipe.NameOrPath;
            this.Reader = new PacketReader(this.Pipe, bufferCapacity);
            this.Writer = new PacketWriter(this.Pipe);
        }

        public string PipeName { get; }

        private INamedPipe Pipe { get; }

        private PacketReader Reader { get; }

        private PacketWriter Writer { get; }

        public void Start()
        {
            this.Pipe.Connect();
        }

        public Task StartAsync()
        {
            return this.Pipe.ConnectAsync();
        }

        public void Dispose()
        {
            this.Pipe.Dispose();
            this.Reader.Dispose();
            this.Writer.Dispose();
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
            return this.Reader.ReadObject<TObject, TPacket>(packet);
        }

        public void Send<TPacket>(TPacket packet)
            where TPacket : IOutboundProxyPacket
        {
            this.Writer.WriteObject(packet);
        }
    }
}
