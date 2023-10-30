using System;
using System.IO;
using OpenCvSharp;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.Python.Proxy.Packets.Outbound
{
    public readonly struct OutboundMatrixPacket : IOutboundProxyPacket
    {
        public OutboundMatrixPacket(Matrix matrix)
        {
            ArgumentNullException.ThrowIfNull(matrix);

            this.Matrix = matrix;
        }

        public int Length => (sizeof(int) * 2) + this.GetMatrixRawData().Length;

        private byte[] GetMatrixRawData()
        {
            var newimage = this.Matrix.Reshape(1);
            newimage.GetArray(out byte[] bytes);
            return bytes;
            // TODO
            //this.Matrix.GetArray(out Vec3b[] matrixData);
            //return matrixData.;
        }

        private Matrix Matrix { get; }

        public static implicit operator OutboundMatrixPacket(Matrix matrix) => new(matrix);

        public void WriteObject(BinaryWriter writer)
        {
            writer.Write(this.Matrix.Width);
            writer.Write(this.Matrix.Height);
            writer.Write(this.GetMatrixRawData());
        }
    }
}
