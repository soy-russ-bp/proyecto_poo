using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using ARJE.Utils.Video;
using Avalonia.Threading;
using OpenCvSharp.Internal.Vectors;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    public sealed class PracticeViewModel : ViewModelBase<PracticeDataModel, PracticeView>
    {
        private readonly AsyncGrabConfig grabConfig = new(
            SynchronizationContext: new AvaloniaSynchronizationContext());

        private readonly VectorOfByte frameEncodeBuffer = new();

        // private readonly CustomModel customModel;

        private Bitmap? frame;

        public PracticeViewModel(PracticeDataModel dataModel)
            : base(dataModel)
        {
            dataModel.VideoSource.StartGrab(this.grabConfig);
            dataModel.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;
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

        public override void Dispose()
        {
            this.DataModel.VideoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            this.DataModel.VideoSource.StopGrab();
            //this.DataModel.CustomModel.Clear();
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            this.Frame = frame.ToAvaloniaBitmap(buffer: this.frameEncodeBuffer);
            //this.customModel.ProcessFrame(frame);
        }
    }
}
