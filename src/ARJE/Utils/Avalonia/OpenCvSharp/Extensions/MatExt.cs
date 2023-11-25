using System.IO;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.OpenCvSharp.Extensions;
using Avalonia.Media.Imaging;
using OpenCvSharp.Internal.Vectors;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.Avalonia.OpenCvSharp.Extensions
{
    public static class MatExt
    {
        public static Bitmap ToAvaloniaBitmap(this Matrix matrix, string extension = ".jpg", VectorOfByte? buffer = null)
        {
            buffer ??= new VectorOfByte();

            Cv2Utils.ImEncode(extension, matrix, buffer);
            using UnmanagedMemoryStream stream = buffer.ToUnmanagedMemoryStream();
            return new Bitmap(stream);
        }
    }
}
