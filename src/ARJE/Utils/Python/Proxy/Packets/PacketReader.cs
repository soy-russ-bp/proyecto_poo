using System;
using System.IO;
using System.IO.Pipes;
using ARJE.Utils.IO;

namespace ARJE.Utils.Python.Proxy.Packets
{
    public sealed class PacketReader : IDisposable
    {
        public PacketReader(PipeStream pipe, int bufferCapacity = 0)
            : this(new BinaryReader(pipe), bufferCapacity)
        {
        }

        private PacketReader(BinaryReader pipeReader, int bufferCapacity = 0)
        {
            ArgumentNullException.ThrowIfNull(pipeReader);

            this.PipeReader = pipeReader;
            this.Buffer = new ReusableBinaryBuffer(bufferCapacity);
        }

        private BinaryReader PipeReader { get; }

        private ReusableBinaryBuffer Buffer { get; }

        public void Dispose()
        {
            this.PipeReader.Dispose();
            this.Buffer.Dispose();
        }

        public TObject ReadObject<TObject, TPacket>(TPacket packet)
            where TPacket : IInboundProxyPacket<TObject>
        {
            int id = this.PipeReader.ReadInt32();
            // TODO: Check id match
            int length = this.PipeReader.ReadInt32();
            this.Buffer.Fill(this.PipeReader, length);
            return packet.ReadObject(this.Buffer.Reader);
        }
    }
}
