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
        }

        public event IAsyncVideoSource<Matrix>.FrameGrabbedHandler? OnFrameGrabbed;

        public GrabState GrabState { get; private set; } = GrabState.Stopped;

        protected override VideoCapture VideoCapturer { get; }

        private Matrix FrameBuffer { get; } = new();

        private AutoResetEvent PauseEvent { get; } = new(initialState: false);

        private Task? GrabTask { get; set; }

        public Matrix Read()
        {
            this.VideoCapturer.Read(this.FrameBuffer);
            this.FlipIfRequired(this.FrameBuffer);
            return this.FrameBuffer;
        }

        public void StartGrab()
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
                this.GrabTask = Task.Factory.StartNew(this.GrabFramesTask, TaskCreationOptions.LongRunning);
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

        public override void Dispose()
        {
            this.StopGrab();
            this.FrameBuffer.Dispose();
            base.Dispose();
        }

        private Task GrabFramesTask()
        {
            while (this.GrabState is GrabState.Running or GrabState.Paused)
            {
                if (this.GrabState == GrabState.Paused)
                {
                    this.PauseEvent.WaitOne();
                    continue;
                }

                bool grabbed = !this.VideoCapturer.IsDisposed && this.VideoCapturer.Grab();

                if (grabbed)
                {
                    this.NotifyFrameGrabbed();
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

        private void NotifyFrameGrabbed()
        {
            this.VideoCapturer.Retrieve(this.FrameBuffer);
            this.FlipIfRequired(this.FrameBuffer);
            this.OnFrameGrabbed?.Invoke(this.FrameBuffer);
        }
    }
}
