using System.IO;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.Python.Proxy.Packets.Outbound
{
    public sealed class OutboundMatrixPacket : IOutboundProxyPacket<OutboundMatrixPacket, Matrix>
    {
        private OutboundMatrixPacket()
        {
        }

        static int IOutboundProxyPacket<OutboundMatrixPacket, Matrix>.ComputeLength(Matrix matrix)
        {
            return (sizeof(int) * 2) + matrix.GetRawData().Length;
        }

        static void IOutboundProxyPacket<OutboundMatrixPacket, Matrix>.SendObject(BinaryWriter writer, Matrix matrix)
        {
            writer.Write(matrix.Width);
            writer.Write(matrix.Height);
            writer.Write(matrix.GetRawData());
        }
    }
}
