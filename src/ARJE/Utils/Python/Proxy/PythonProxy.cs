using System;
using System.IO.Pipes;
using ARJE.Utils.Python.Proxy.Packets;

namespace ARJE.Utils.Python.Proxy
{
    public sealed class PythonProxy : IDisposable
    {
        public PythonProxy(string pipeName, IIdMapper idMapper, int bufferCapacity = 0)
        {
            this.Pipe = new NamedPipeClientStream(pipeName);
            this.Reader = new PacketReader(this.Pipe, bufferCapacity);
            this.Writer = new PacketWriter(this.Pipe);
        }

        private NamedPipeClientStream Pipe { get; }

        private PacketReader Reader { get; }

        private PacketWriter Writer { get; }

        public PythonProxy Start()
        {
            this.Pipe.Connect();
            return this;
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
