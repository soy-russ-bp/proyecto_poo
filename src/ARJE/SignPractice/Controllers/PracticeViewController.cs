using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Controllers;

namespace ARJE.SignPractice.Controllers
{
    public sealed class PracticeViewController : ViewControllerBase<PracticeView, PracticeDataModel, PracticeViewModel>
    {
        public PracticeViewController(PracticeDataModel dataModel)
            : base(dataModel, new PracticeViewModel(dataModel))
        {
            dataModel.VideoSource.OnFrameGrabbed += this.ViewModel.OnFrameGrabbed;
            dataModel.VideoSource.StartGrab(dataModel.GrabConfig);
        }

        public override void Dispose()
        {
            this.DataModel.VideoSource.StopGrab();
            this.DataModel.VideoSource.OnFrameGrabbed -= this.ViewModel.OnFrameGrabbed;
            base.Dispose();
            // this.DataModel.CustomModel.Clear();
        }
    }
}
