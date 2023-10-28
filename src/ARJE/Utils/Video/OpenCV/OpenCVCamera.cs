using System;
using Emgu.CV;
using CvFlipType = Emgu.CV.CvEnum.FlipType;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.Video.OpenCV
{
    public abstract class OpenCVCamera : IBaseVideoSource<Matrix>
    {
        public virtual bool IsOpen => this.VideoCapturer.IsOpened;

        public FlipType OutputFlipType { get; set; }

        protected abstract VideoCapture VideoCapturer { get; }

        public virtual void Dispose()
        {
            this.VideoCapturer.Dispose();
            GC.SuppressFinalize(this);
        }

        protected static CvFlipType CvFlipTypeConverter(FlipType flipType)
        {
            return flipType switch
            {
                FlipType.Vertical => CvFlipType.Vertical,
                FlipType.Horizontal => CvFlipType.Horizontal,
                FlipType.Both => CvFlipType.Both,
                _ => throw new ArgumentOutOfRangeException(nameof(flipType)),
            };
        }

        protected void FlipIfRequired(Matrix buffer)
        {
            if (this.OutputFlipType != FlipType.None)
            {
                CvFlipType flipType = CvFlipTypeConverter(this.OutputFlipType);
                CvInvoke.Flip(buffer, buffer, flipType);
            }
        }
    }
}
