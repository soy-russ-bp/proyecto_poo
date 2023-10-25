using System;
using System.IO;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.Python.Proxy.Packets.Outbound
{
    public readonly struct OutboundMatrixPacket : IOutboundProxyPacket
    {
        public OutboundMatrixPacket(Matrix matrix)
        {
            ArgumentNullException.ThrowIfNull(matrix);

            this.Matrix = matrix;
        }

        public int Length => (sizeof(int) * 2) + this.Matrix.GetRawData().Length;

        private Matrix Matrix { get; }

        public static implicit operator OutboundMatrixPacket(Matrix matrix) => new(matrix);

        public void WriteObject(BinaryWriter writer)
        {
            writer.Write(this.Matrix.Width);
            writer.Write(this.Matrix.Height);
            writer.Write(this.Matrix.GetRawData());
        }
    }
}
