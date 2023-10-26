using System;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.Video.OpenCV
{
    public sealed class AsyncWebcam : OpenCVCamera, IAsyncVideoSource<Matrix>
    {
        public AsyncWebcam(int camIndex = 0, bool flipHorizontally = false)
            : base(camIndex, flipHorizontally)
        {
        }

        public event IAsyncVideoSource<Matrix>.FrameGrabbedHandler? OnFrameGrabbed;

        public void Start()
        {
            this.VideoCapturer.Start();
            this.VideoCapturer.ImageGrabbed += this.FrameGrabbedHandler;
        }

        public void Pause()
        {
            this.VideoCapturer.Pause();
        }

        public void Stop()
        {
            this.Dispose();
        }

        public override void Dispose()
        {
            this.VideoCapturer.ImageGrabbed -= this.FrameGrabbedHandler;
            this.VideoCapturer.Stop();
            base.Dispose();
        }

        private void FrameGrabbedHandler(object? sender, EventArgs e)
        {
            this.VideoCapturer.Retrieve(this.Buffer);
            this.FlipIfRequired(this.Buffer);
            this.OnFrameGrabbed?.Invoke(this.Buffer);
        }
    }
}
