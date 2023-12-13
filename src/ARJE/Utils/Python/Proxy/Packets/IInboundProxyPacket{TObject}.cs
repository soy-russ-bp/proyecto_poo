using System.IO;

namespace ARJE.Utils.Python.Proxy.Packets
{
    public interface IInboundProxyPacket<TObject>
    {
        public abstract TObject ReadObject(BinaryReader reader);
    }
}
