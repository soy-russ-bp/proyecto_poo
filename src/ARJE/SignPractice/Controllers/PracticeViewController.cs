using System;
using System.Threading.Tasks;
using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.AI;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.MVC.Views;
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
            Task.Run(this.LoadModel);
        }

        public override void Run(IViewDisplay viewDisplay)
        {
            this.Model.VideoSource.StartGrab(this.Model.GrabConfig);
            base.Run(viewDisplay);
        }

        public override void Dispose()
        {
            this.Model.VideoSource.StopGrab();
            this.Model.VideoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            base.Dispose();
        }

        private Task LoadModel()
        {
            do
            {
                if (this.Model.DetectionModel.IsProxyConnected)
                {
                    this.Model.DetectionModel.LoadModel();
                    return Task.CompletedTask;
                }

                Task.Delay(TimeSpan.FromSeconds(0.1));
            }
            while (true);
        }

        private void OnBackBtnClick()
        {
            HomeViewController.Instance.GoToHome();
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            if (this.Model.DetectionModel.Ready)
            {
                string? detectedSign = this.Model.DetectionModel.ProcessFrame(frame, out IDetection? detection);
                if (detectedSign != null)
                {
                    this.View.SignText = detectedSign;
                    DetectionDrawer.Draw(frame, detection!);
                }
            }

            Bitmap uiFrame = frame.ToAvaloniaBitmap(buffer: this.Model.FrameEncodeBuffer);
            this.View.SetFrameAndDisposeLast(uiFrame);
        }
    }
}
