using System;
using ARJE.Utils.Video;
using OpenCvSharp;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.OpenCvSharp
{
    public abstract class OpenCvVideoSource : ISyncVideoSource<Matrix>, IAsyncVideoSource<Matrix>
    {
        public OpenCvVideoSource(VideoCapture videoCapturer, FlipType outputFlipType = FlipType.None)
        {
            this.VideoCapturer = videoCapturer;
            this.VideoCaptureGrabber = new VideoCaptureGrabber(videoCapturer, outputFlipType, this.FrameBuffer);
            this.VideoCaptureGrabber.OnFrameGrabbed += this.NotifyFrameGrabbed;
            this.OutputFlipType = outputFlipType;
        }

        public event IAsyncVideoSource<Matrix>.FrameGrabbedHandler? OnFrameGrabbed;

        public virtual bool IsOpen => this.VideoCapturer.IsOpened();

        public FlipType OutputFlipType
        {
            get => this.VideoCaptureGrabber.OutputFlipType;
            set => this.VideoCaptureGrabber.OutputFlipType = value;
        }

        public GrabState GrabState => this.VideoCaptureGrabber.GrabState;

        protected VideoCapture VideoCapturer { get; }

        protected Matrix FrameBuffer { get; } = new();

        protected VideoCaptureGrabber VideoCaptureGrabber { get; }

        public Matrix Read()
        {
            this.VideoCapturer.Read(this.FrameBuffer);
            this.FlipIfRequired(this.FrameBuffer);
            return this.FrameBuffer;
        }

        public void StartGrab(AsyncGrabConfig grabConfig)
        {
            this.VideoCaptureGrabber.StartGrab(grabConfig);
        }

        public void PauseGrab()
        {
            this.VideoCaptureGrabber.PauseGrab();
        }

        public void StopGrab()
        {
            this.VideoCaptureGrabber.StopGrab();
        }

        public virtual void Dispose()
        {
            this.VideoCaptureGrabber.Dispose();
            this.VideoCapturer.Dispose();
            this.FrameBuffer.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void FlipIfRequired(Matrix buffer)
        {
            if (FlipConverter.TryConvertToFlipMode(this.OutputFlipType, out FlipMode flipMode))
            {
                Cv2.Flip(buffer, buffer, flipMode);
            }
        }

        private void NotifyFrameGrabbed(Matrix frame)
        {
            this.OnFrameGrabbed?.Invoke(frame);
        }
    }
}
