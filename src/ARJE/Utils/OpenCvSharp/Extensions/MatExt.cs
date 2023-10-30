using System;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.OpenCvSharp.Extensions
{
    public static class MatExt
    {
        public static int GetByteCount(this Matrix matrix)
        {
            return (int)matrix.DataEnd - (int)matrix.DataStart;
        }

        public static unsafe Span<byte> AsBytes(this Matrix matrix)
        {
            void* start = matrix.DataStart.ToPointer();
            int byteCount = matrix.GetByteCount();
            return new Span<byte>(start, byteCount);
        }
    }
}
