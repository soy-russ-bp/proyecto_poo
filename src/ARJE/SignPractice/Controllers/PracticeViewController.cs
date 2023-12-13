using System;
using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.Controllers
{
    public sealed class PracticeViewController : ViewControllerBase<PracticeView, PracticeDataModel>
    {
        public PracticeViewController(PracticeDataModel dataModel)
            : base(dataModel)
        {
            this.View.OnBackBtnClick += this.OnBackBtnClick;
            dataModel.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            dataModel.VideoSource.StartGrab(dataModel.GrabConfig);
        }

        public override void Dispose()
        {
            this.Model.VideoSource.StopGrab();
            this.Model.VideoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            base.Dispose();
            // this.DataModel.CustomModel.Clear();
        }

        private void OnBackBtnClick()
        {
            HomeViewController.Instance.GoToHome();
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            (this.View.FrameSource as IDisposable)?.Dispose();
            this.View.FrameSource = frame.ToAvaloniaBitmap(buffer: this.Model.FrameEncodeBuffer);
        }
    }
}
