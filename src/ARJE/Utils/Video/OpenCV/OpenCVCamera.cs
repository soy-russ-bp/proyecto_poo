using System;
using OpenCvSharp;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.Video.OpenCV
{
    public abstract class OpenCVCamera : IBaseVideoSource<Matrix>
    {
        public virtual bool IsOpen => this.VideoCapturer.IsOpened();

        public FlipType OutputFlipType { get; set; }

        protected abstract VideoCapture VideoCapturer { get; }

        public virtual void Dispose()
        {
            this.VideoCapturer.Dispose();
            GC.SuppressFinalize(this);
        }

        protected static FlipMode CvFlipTypeConverter(FlipType flipType)
        {
            return flipType switch
            {
                FlipType.Vertical => FlipMode.X,
                FlipType.Horizontal => FlipMode.Y,
                FlipType.Both => FlipMode.XY,
                _ => throw new ArgumentOutOfRangeException(nameof(flipType)),
            };
        }

        protected void FlipIfRequired(Matrix buffer)
        {
            if (this.OutputFlipType != FlipType.None)
            {
                FlipMode flipMode = CvFlipTypeConverter(this.OutputFlipType);
                Cv2.Flip(buffer, buffer, flipMode);
            }
        }
    }
}
