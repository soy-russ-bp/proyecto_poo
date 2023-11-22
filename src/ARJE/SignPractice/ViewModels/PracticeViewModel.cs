using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using ARJE.Utils.Video;
using ARJE.Utils.Video.OpenCv;
using Avalonia.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace ARJE.SignPractice.ViewModels
{
    public class PracticeViewModel : ViewModelBase
    {
        private readonly VectorOfByte frameEncodeBuffer = new();
        private Bitmap? frame;

        public PracticeViewModel()
        {
            var cam = new Webcam(outputFlipType: FlipType.Horizontal);
            cam.StartGrab();
            cam.OnFrameGrabbed += this.OnFrameGrabbed;
        }

        private void OnFrameGrabbed(Mat frame)
        {
            this.Frame = frame.ToAvaloniaBitmap(buffer: this.frameEncodeBuffer);
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
    }
}
