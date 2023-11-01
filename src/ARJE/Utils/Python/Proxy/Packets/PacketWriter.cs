using System;
using System.IO;
using System.Text;

namespace ARJE.Utils.Python.Proxy.Packets
{
    public sealed class PacketWriter : IDisposable
    {
        public PacketWriter(Stream pipeStream)
            : this(new BinaryWriter(pipeStream, Encoding.UTF8, leaveOpen: true))
        {
        }

        private PacketWriter(BinaryWriter pipeWriter)
        {
            ArgumentNullException.ThrowIfNull(pipeWriter);

            this.PipeWriter = pipeWriter;
        }

        private BinaryWriter PipeWriter { get; }

        public void Dispose()
        {
            this.PipeWriter.Dispose();
        }

        public void WriteObject<TPacket>(TPacket packet)
            where TPacket : IOutboundProxyPacket
        {
            int id = 2000; // TODO: Get id
            int length = packet.Length;
            this.PipeWriter.Write(id);
            this.PipeWriter.Write(length);

            // TODO: Check stream pos
            packet.WriteObject(this.PipeWriter);
        }
    }
}
