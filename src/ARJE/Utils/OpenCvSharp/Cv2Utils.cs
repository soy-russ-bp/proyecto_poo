using System;
using ARJE.Utils.Collections;
using OpenCvSharp;
using OpenCvSharp.Internal;
using OpenCvSharp.Internal.Vectors;

namespace ARJE.Utils.OpenCvSharp
{
    public static class Cv2Utils
    {
        public static bool ImEncode(string extension, InputArray img, VectorOfByte buffer, int[]? @params = null)
        {
            extension = extension.Trim();

            ArgumentException.ThrowIfNullOrEmpty(extension);
            ArgumentNullException.ThrowIfNull(img);
            ArgumentNullException.ThrowIfNull(buffer);

            img.ThrowIfDisposed();
            buffer.ThrowIfDisposed();

            ArrayUtils.EmptyIfNull(ref @params);

            ExceptionStatus exception = NativeMethods.imgcodecs_imencode_vector(extension, img.CvPtr, buffer.CvPtr, @params, @params.Length, out int returnValue);
            NativeMethods.HandleException(exception);
            return returnValue != 0;
        }
    }
}
