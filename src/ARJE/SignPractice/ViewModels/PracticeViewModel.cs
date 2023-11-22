using System;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using ARJE.Utils.Video.OpenCv;
using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace ARJE.SignPractice.ViewModels
{
    public sealed class PracticeViewModel : ViewModelBase, IDisposable
    {
        private readonly VectorOfByte frameEncodeBuffer = new();
        private readonly Webcam webcam;
        private Bitmap? frame;

        public PracticeViewModel(Webcam webcam)
        {
            this.webcam = webcam;
            webcam.StartGrab();
            webcam.OnFrameGrabbed += this.OnFrameGrabbed;
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

        private void OnFrameGrabbed(Mat frame)
        {
            this.Frame = frame.ToAvaloniaBitmap(buffer: this.frameEncodeBuffer);
        }

        public void Dispose()
        {
            this.webcam.OnFrameGrabbed -= this.OnFrameGrabbed;
            this.webcam.StopGrab();
        }
    }
}
