using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.AI;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using Avalonia.Media.Imaging;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.Controllers
{
    public sealed class PracticeViewController : ViewControllerBase<PracticeView, PracticeDataModel>
    {
        public PracticeViewController(PracticeDataModel model)
            : base(model)
        {
            this.View.OnBackBtnClick += this.OnBackBtnClick;
            model.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            model.VideoSource.StartGrab(model.GrabConfig);
        }

        public override void Dispose()
        {
            this.Model.VideoSource.StopGrab();
            this.Model.VideoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            base.Dispose();
        }

        private void OnBackBtnClick()
        {
            HomeViewController.Instance.GoToHome();
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            string? detectedSign = this.Model.DetectionModel.ProcessFrame(frame, out IDetection? detection);
            if (detectedSign != null)
            {
                this.View.SignText = detectedSign;
                DetectionDrawer.Draw(frame, detection!);
            }

            Bitmap uiFrame = frame.ToAvaloniaBitmap(buffer: this.Model.FrameEncodeBuffer);
            this.View.SetFrameAndDisposeLast(uiFrame);
        }
    }
}
