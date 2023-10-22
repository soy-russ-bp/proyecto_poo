using System;
using System.IO;
using System.IO.Pipes;
using ARJE.Utils.IO;
using ARJE.Utils.Python.Proxy.Packets;

namespace ARJE.Utils.Python.Proxy
{
    public sealed class PythonProxy : IDisposable
    {
        public PythonProxy(string pipeName, IIdMapper idMapper, int capacity = 0)
        {
            this.Pipe = new NamedPipeClientStream(pipeName);
            this.Reader = new BinaryReader(this.Pipe);
            this.Writer = new BinaryWriter(this.Pipe);
            this.IdMapper = idMapper;
            this.Buffer = new ReusableBinaryBuffer(capacity);
        }

        private NamedPipeClientStream Pipe { get; }

        private BinaryReader Reader { get; }

        private BinaryWriter Writer { get; }

        private IIdMapper IdMapper { get; }

        private ReusableBinaryBuffer Buffer { get; }

        public PythonProxy Start()
        {
            this.Pipe.Connect();
            return this;
        }

        public void Dispose()
        {
            this.Pipe.Dispose();
            this.Buffer.Dispose();
        }

        public void Send<TPacket, TData>(TData data)
            where TPacket : IOutboundProxyPacket<TPacket, TData>
        {
            IOutboundProxyPacket<TPacket, TData>.Send(this.IdMapper, this.Writer, data);
        }

        public TData Receive<TPacket, TData>()
            where TPacket : IInboundProxyPacket<TPacket, TData>
        {
            return IInboundProxyPacket<TPacket, TData>.Receive(this.IdMapper, this.Reader, this.Buffer);
        }
    }
}
