using System;
using System.Runtime.Versioning;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using ARJE.Utils.Video;
using Avalonia.Threading;
using OpenCvSharp.Internal.Vectors;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal sealed class PracticeViewModel : ViewModelBase, IDisposable
    {
        private readonly IAsyncVideoSource<Matrix> videoSource;

        private readonly AsyncGrabConfig grabConfig = new(
            SynchronizationContext: new AvaloniaSynchronizationContext());

        private readonly VectorOfByte frameEncodeBuffer = new();

        // private readonly CustomModel customModel;

        private Bitmap? frame;

        public PracticeViewModel(IAsyncVideoSource<Matrix> videoSource, CustomModel customModel)
        {
            this.videoSource = videoSource;
            videoSource.StartGrab(this.grabConfig);
            videoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            //this.customModel = customModel;
        }

        public Bitmap? Frame
        {
            get => this.frame;
            private set
            {
                this.frame?.Dispose();
                this.RaiseAndSetIfChanged(ref this.frame, value);
            }
        }

        public void Dispose()
        {
            this.videoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            this.videoSource.StopGrab();
            //this.customModel.Clear();
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            this.Frame = frame.ToAvaloniaBitmap(buffer: this.frameEncodeBuffer);
            //this.customModel.ProcessFrame(frame);
        }
    }
}
