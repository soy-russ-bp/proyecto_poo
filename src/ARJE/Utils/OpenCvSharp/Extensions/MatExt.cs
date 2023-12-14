using System;
using System.Diagnostics.Contracts;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.OpenCvSharp.Extensions
{
    public static class MatExt
    {
        [Pure]
        public static int GetByteCount(this Matrix matrix)
        {
            return (int)(matrix.DataEnd - matrix.DataStart);
        }

        [Pure]
        public static unsafe Span<byte> AsBytes(this Matrix matrix)
        {
            void* start = matrix.DataStart.ToPointer();
            int byteCount = matrix.GetByteCount();
            return new Span<byte>(start, byteCount);
        }
    }
}
