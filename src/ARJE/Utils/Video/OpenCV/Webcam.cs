using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.Video.OpenCV
{
    public sealed class Webcam : OpenCVCamera, ISyncVideoSource<Matrix>
    {
        public Webcam(int camIndex = 0, bool flipHorizontally = false)
            : base(camIndex, flipHorizontally)
        {
        }

        public Matrix Read()
        {
            this.VideoCapturer.Read(this.Buffer);
            this.FlipIfRequired(this.Buffer);
            return this.Buffer;
        }
    }
}
