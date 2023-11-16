using System;
using System.Threading;
using System.Threading.Tasks;
using ARJE.Utils.Threading;
using OpenCvSharp;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.Video.OpenCV
{
    public sealed class VideoCaptureGrabber : IAsyncVideoSource<Matrix>
    {
        public VideoCaptureGrabber(VideoCapture videoCapturer, FlipType outputFlipType = FlipType.None, Matrix? frameBuffer = null)
        {
            ArgumentNullException.ThrowIfNull(videoCapturer);

            this.VideoCapturer = videoCapturer;
            this.OutputFlipType = outputFlipType;
            this.DisposeFrameBuffer = frameBuffer is null;
            this.FrameBuffer = frameBuffer ?? new();
        }

        public event IAsyncVideoSource<Matrix>.FrameGrabbedHandler? OnFrameGrabbed;

        public VideoCapture VideoCapturer { get; }

        public bool IsOpen => this.VideoCapturer.IsOpened();

        public GrabState GrabState { get; private set; } = GrabState.Stopped;

        public FlipType OutputFlipType { get; set; }

        private bool DisposeFrameBuffer { get; }

        private Matrix FrameBuffer { get; }

        private AutoResetEvent PauseEvent { get; } = new(initialState: false);

        private Task? GrabTask { get; set; }

        public void StartGrab(AsyncGrabConfig grabConfig)
        {
            if (this.GrabState == GrabState.Paused)
            {
                this.GrabState = GrabState.Running;
                this.PauseEvent.Set();
                return;
            }

            if (this.GrabState.IsStop())
            {
                this.GrabState = GrabState.Running;
                this.GrabTask = Task.Factory.StartNew(() => this.GrabFramesTask(grabConfig), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public void PauseGrab()
        {
            if (this.GrabState == GrabState.Running)
            {
                this.GrabState = GrabState.Paused;
            }
        }

        public void StopGrab()
        {
            switch (this.GrabState)
            {
                case GrabState.Paused:
                    this.GrabState = GrabState.Stopping;
                    this.PauseEvent.Set();
                    break;
                case GrabState.Running:
                    this.GrabState = GrabState.Stopping;
                    break;
            }

            if (this.GrabTask != null)
            {
                this.GrabTask.Wait(100);
                this.GrabTask = null;
            }
        }

        public void Dispose()
        {
            this.StopGrab();
            if (this.DisposeFrameBuffer)
            {
                this.FrameBuffer.Dispose();
            }
        }

        private Task GrabFramesTask(AsyncGrabConfig grabConfig)
        {
            var fpsSync = new FpsSync(grabConfig.FpsCap);
            while (this.GrabState is GrabState.Running or GrabState.Paused)
            {
                if (this.GrabState == GrabState.Paused)
                {
                    this.PauseEvent.WaitOne();
                    continue;
                }

                if (!fpsSync.ShouldGrab())
                {
                    continue;
                }

                bool grabbed = !this.VideoCapturer.IsDisposed && this.VideoCapturer.Grab();

                if (grabbed)
                {
                    fpsSync.NotifyGrabbed();
                    this.NotifyFrameGrabbed(grabConfig.SynchronizationContext);
                }
                else
                {
                    this.GrabState = GrabState.Stopping;
                    break;
                }
            }

            this.GrabState = GrabState.Stopped;
            return Task.CompletedTask;
        }

        private void NotifyFrameGrabbed(SynchronizationContext? synchronizationContext)
        {
            this.VideoCapturer.Retrieve(this.FrameBuffer);
            this.FlipIfRequired(this.FrameBuffer);
            synchronizationContext.SendInCtxOrCurrent(this, webcam => webcam.OnFrameGrabbed?.Invoke(webcam.FrameBuffer));
        }

        private void FlipIfRequired(Matrix buffer)
        {
            if (FlipConverter.TryConvertToFlipMode(this.OutputFlipType, out FlipMode flipMode))
            {
                Cv2.Flip(buffer, buffer, flipMode);
            }
        }
    }
}
