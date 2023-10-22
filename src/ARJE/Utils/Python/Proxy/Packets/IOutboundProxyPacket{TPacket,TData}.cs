using System;
using System.IO;

namespace ARJE.Utils.Python.Proxy.Packets
{
    public interface IOutboundProxyPacket<TPacket, TData>
        where TPacket : IOutboundProxyPacket<TPacket, TData>
    {
        public static void Send(IIdMapper idMapper, BinaryWriter pipeWriter, TData data)
        {
            ArgumentNullException.ThrowIfNull(idMapper);
            ArgumentNullException.ThrowIfNull(pipeWriter);
            ArgumentNullException.ThrowIfNull(data);

            int id = idMapper.GetTypeId(typeof(TData));
            int length = TPacket.ComputeLength(data);
            pipeWriter.Write(id);
            pipeWriter.Write(length);

            // TODO: Check stream pos
            TPacket.SendObject(pipeWriter, data);
        }

        protected static abstract int ComputeLength(TData data);

        protected static abstract void SendObject(BinaryWriter writer, TData data);
    }
}
