using System;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    internal sealed class PracticeViewModel : ViewModelBase, IDisposable
    {
        private readonly VectorOfByte frameEncodeBuffer = new();
        private readonly IAsyncVideoSource<Matrix> videoSource;
        private Bitmap? frame;

        public PracticeViewModel(IAsyncVideoSource<Matrix> videoSource)
        {
            this.videoSource = videoSource;
            videoSource.StartGrab(AsyncGrabConfig.Default);
            videoSource.OnFrameGrabbed += this.OnFrameGrabbed;
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
        }

        private void OnFrameGrabbed(Mat frame)
        {
            this.Frame = frame.ToAvaloniaBitmap(buffer: this.frameEncodeBuffer);
        }
    }
}
