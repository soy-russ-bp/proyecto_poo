using Emgu.CV;
using Emgu.CV.CvEnum;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.Video
{
    public sealed class Camera : IVideoSource<Matrix>
    {
        public Camera(int camIndex = 0, bool flipHorizontally = false)
        {
            this.FlipHorizontally = flipHorizontally;
            this.VideoCapture = new VideoCapture(camIndex);
        }

        public bool IsOpen => this.VideoCapture.IsOpened;

        public bool FlipHorizontally { get; set; }

        private VideoCapture VideoCapture { get; }

        public void Read(Matrix buffer)
        {
            this.VideoCapture.Read(buffer);
            if (this.FlipHorizontally)
            {
                CvInvoke.Flip(buffer, buffer, FlipType.Horizontal);
            }
        }

        public void Dispose()
        {
            this.VideoCapture.Dispose();
        }
    }
}
