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

        protected static bool TryConvertToFlipMode(FlipType flipType, out FlipMode flipMode)
        {
            const int NoValue = int.MinValue;
            flipMode = flipType switch
            {
                FlipType.Vertical => FlipMode.X,
                FlipType.Horizontal => FlipMode.Y,
                FlipType.Both => FlipMode.XY,
                _ => (FlipMode)NoValue,
            };

            return (int)flipMode != NoValue;
        }

        protected void FlipIfRequired(Matrix buffer)
        {
            if (TryConvertToFlipMode(this.OutputFlipType, out FlipMode flipMode))
            {
                Cv2.Flip(buffer, buffer, flipMode);
            }
        }
    }
}
