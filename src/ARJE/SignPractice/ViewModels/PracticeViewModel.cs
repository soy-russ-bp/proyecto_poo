using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    public sealed class PracticeViewModel : ViewModelBase<PracticeDataModel, PracticeView>
    {
        private Bitmap? frame;

        public PracticeViewModel(PracticeDataModel dataModel)
            : base(dataModel)
        {
        }

        public Bitmap? Frame
        {
            get => this.frame;
            private set
            {
                if (this.frame == value)
                {
                    return;
                }

                this.frame?.Dispose();
                this.frame = value;
                this.RaisePropertyChanged();
            }
        }

        public void OnFrameGrabbed(Matrix frame)
        {
            this.Frame = frame.ToAvaloniaBitmap(buffer: this.DataModel.FrameEncodeBuffer);
            //this.customModel.ProcessFrame(frame);
        }
    }
}
