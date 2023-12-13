using System.Diagnostics.Contracts;
using System.IO;
using OpenCvSharp.Internal.Vectors;

namespace ARJE.Utils.OpenCvSharp.Extensions
{
    public static class VectorOfByteExt
    {
        [Pure]
        public static unsafe UnmanagedMemoryStream ToUnmanagedMemoryStream(this VectorOfByte vector)
        {
            return new UnmanagedMemoryStream((byte*)vector.ElemPtr, vector.Size);
        }
    }
}
