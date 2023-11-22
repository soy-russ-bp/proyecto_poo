using ARJE.Utils.Video;
using OpenCvSharp;

namespace ARJE.Utils.OpenCvSharp
{
    public sealed class Webcam : OpenCvVideoSource
    {
        public Webcam(int camIndex = 0, FlipType outputFlipType = FlipType.None)
            : base(CreateVideoCapturer(camIndex), outputFlipType)
        {
        }

        private static VideoCapture CreateVideoCapturer(int camIndex)
        {
            return new VideoCapture(camIndex);
        }
    }
}
