using System.IO;
using ARJE.Utils.IO;

namespace ARJE.Utils.Python.Proxy.Packets
{
    public interface IInboundProxyPacket<TPacket, TData>
        where TPacket : IInboundProxyPacket<TPacket, TData>
    {
        public static TData Receive<TBuffer>(IIdMapper idManager, BinaryReader pipeReader, TBuffer buffer)
            where TBuffer : IReadableBinaryBuffer, IFillableByReader
        {
            int id = pipeReader.ReadInt32();
            // TODO: Check id match
            int length = pipeReader.ReadInt32();
            buffer.Fill(pipeReader, length);
            return TPacket.ReceiveObject(buffer.Reader);
        }

        protected static abstract TData ReceiveObject(BinaryReader packetReader);
    }
}
