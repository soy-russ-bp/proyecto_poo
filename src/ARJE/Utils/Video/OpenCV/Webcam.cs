using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.Video.OpenCV
{
    public sealed class Webcam : OpenCVCamera, ISyncVideoSource<Matrix>, IAsyncVideoSource<Matrix>
    {
        public Webcam(int camIndex = 0, FlipType outputFlipType = FlipType.None)
        {
            this.VideoCapturer = new VideoCapture(camIndex);
            this.OutputFlipType = outputFlipType;
            this.FrameBuffer = new();
        }

        public event IAsyncVideoSource<Matrix>.FrameGrabbedHandler? OnFrameGrabbed;

        protected override VideoCapture VideoCapturer { get; }

        private Matrix FrameBuffer { get; }

        public Matrix Read()
        {
            this.VideoCapturer.Read(this.FrameBuffer);
            this.FlipIfRequired(this.FrameBuffer);
            return this.FrameBuffer;
        }

        public void StartGrab()
        {
            Task.Run(this.GrabFrames);
        }

        public void PauseGrab()
        {
            // TODO
        }

        public void StopGrab()
        {
            this.Dispose();
        }

        public override void Dispose()
        {
            // TODO
            this.FrameBuffer.Dispose();
            base.Dispose();
        }

        private Task GrabFrames()
        {
            // TODO
            while (true)
            {
                Thread.Sleep(10);
                this.FrameGrabbedNotify();
            }
        }

        private void FrameGrabbedNotify()
        {
            // TODO
            this.VideoCapturer.Read(this.FrameBuffer);
            this.FlipIfRequired(this.FrameBuffer);
            this.OnFrameGrabbed?.Invoke(this.FrameBuffer);
        }
    }
}
