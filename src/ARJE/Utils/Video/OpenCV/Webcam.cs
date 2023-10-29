using System;
using Emgu.CV;
using Matrix = Emgu.CV.Mat;

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
            this.VideoCapturer.Start();
            this.VideoCapturer.ImageGrabbed += this.FrameGrabbedNotify;
        }

        public void PauseGrab()
        {
            this.VideoCapturer.Pause();
            this.VideoCapturer.ImageGrabbed -= this.FrameGrabbedNotify;
        }

        public void StopGrab()
        {
            this.Dispose();
        }

        public override void Dispose()
        {
            this.VideoCapturer.ImageGrabbed -= this.FrameGrabbedNotify;
            this.VideoCapturer.Stop();
            this.FrameBuffer.Dispose();
            base.Dispose();
        }

        private void FrameGrabbedNotify(object? sender, EventArgs e)
        {
            this.VideoCapturer.Retrieve(this.FrameBuffer);
            this.FlipIfRequired(this.FrameBuffer);
            this.OnFrameGrabbed?.Invoke(this.FrameBuffer);
        }
    }
}
