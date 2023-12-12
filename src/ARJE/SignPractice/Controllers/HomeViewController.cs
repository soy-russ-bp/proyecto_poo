using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.MVC.Models;
using ARJE.Utils.Avalonia.MVC.Views;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.Controllers
{
    public sealed class HomeViewController : ViewControllerBase<HomeView, NoDataModel>
    {
        private readonly IViewDisplay viewDisplay;

        private readonly IAsyncVideoSource<Matrix> videoSource =
            new Webcam(outputFlipType: FlipType.Horizontal);

        private readonly CustomModel customModel = null;

        public HomeViewController(IViewDisplay viewDisplay)
            : base(NoDataModel.None)
        {
            Instance = this;
            this.viewDisplay = viewDisplay;
            this.View.OnPracticeBtnClick += this.GoToPractice;
            this.View.OnSelectBtnClick += this.GoToSelect;
            this.View.OnImportBtnClick += this.GoToImport;
        }

        public static HomeViewController Instance { get; private set; } = null!;

        public void Run()
        {
            this.GoToHome();
        }

        public void GoToHome()
        {
            this.Run(this.viewDisplay);
        }

        public void GoToPractice()
        {
            var dataModel = new PracticeDataModel(this.videoSource, this.customModel);
            new PracticeViewController(dataModel).Run(this.viewDisplay);
        }

        public void GoToSelect()
        {
            new SelectViewController().Run(this.viewDisplay);
        }

        public void GoToImport()
        {
            new ImportViewController().Run(this.viewDisplay);
        }
    }
}
