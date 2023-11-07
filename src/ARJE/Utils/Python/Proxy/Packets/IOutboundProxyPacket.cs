using System.IO;

namespace ARJE.Utils.Python.Proxy.Packets
{
    public interface IOutboundProxyPacket
    {
        public abstract int Length { get; }

        public abstract void WriteObject(BinaryWriter writer);
    }
}
