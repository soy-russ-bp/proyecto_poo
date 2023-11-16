using OpenCvSharp;

namespace ARJE.Utils.Video.OpenCv
{
    public sealed class Webcam : OpenCvCamera
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
