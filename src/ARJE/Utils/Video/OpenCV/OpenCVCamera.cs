using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.Video.OpenCV
{
    public abstract class OpenCVCamera : IVideoSource<Matrix>
    {
        public OpenCVCamera(int camIndex = 0, bool flipHorizontally = false)
        {
            this.FlipHorizontally = flipHorizontally;
            this.VideoCapturer = new VideoCapture(camIndex);
        }

        public bool IsOpen => this.VideoCapturer.IsOpened;

        public bool FlipHorizontally { get; set; }

        protected VideoCapture VideoCapturer { get; }

        protected Matrix Buffer { get; private set; } = new();

        public virtual void Dispose()
        {
            this.VideoCapturer.Dispose();
            this.Buffer.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void FlipIfRequired(Matrix buffer)
        {
            if (this.FlipHorizontally)
            {
                CvInvoke.Flip(buffer, buffer, FlipType.Horizontal);
            }
        }
    }
}
