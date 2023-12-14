using System;
using System.IO;
using System.Threading.Tasks;
using ARJE.Shared.Models;
using ARJE.Shared.Proxy;
using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.MVC.Models;
using ARJE.Utils.Avalonia.MVC.Views;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using Avalonia.Threading;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.Controllers
{
    public sealed class HomeViewController : ViewControllerBase<HomeView, NoDataModel>
    {
        private readonly IViewDisplay viewDisplay;

        private readonly IAsyncVideoSource<Matrix> videoSource =
            new Webcam(outputFlipType: FlipType.Horizontal);

        private readonly DirectoryInfo modelsDir = Directory.CreateDirectory("Models");

        private readonly OnDiskModelTrainingConfigCollection configCollection;

        private readonly HandsModel handsModel = new(new HandsModelConfig(1));

        private CustomModel? customModel;

        public HomeViewController(IViewDisplay viewDisplay)
            : base(NoDataModel.None)
        {
            Instance = this;
            this.configCollection = new(this.modelsDir);
            this.viewDisplay = viewDisplay;
            this.UpdatePracticeEnabledState();
            this.View.OnPracticeBtnClick += this.GoToPractice;
            this.View.OnSelectBtnClick += this.GoToSelect;
            this.View.OnImportBtnClick += this.GoToImport;
        }

        public static HomeViewController Instance { get; private set; } = null!;

        private CustomModel? CustomModel
        {
            get => this.customModel;
            set
            {
                this.customModel = value;
                this.UpdatePracticeEnabledState();
            }
        }

        public async Task RunAsync()
        {
            Dispatcher.UIThread.Invoke(this.GoToHome);
            CustomModel.InitializePythonEngine();
            await this.handsModel.StartAsync(PythonProxyApp.AppInfo);
        }

        public void GoToHome()
        {
            this.Run(this.viewDisplay);
        }

        public void GoToPractice()
        {
            ArgumentNullException.ThrowIfNull(this.CustomModel);

            var dataModel = new PracticeDataModel(this.videoSource, this.CustomModel);
            new PracticeViewController(dataModel).Run(this.viewDisplay);
        }

        public void GoToSelect()
        {
            var dataModel = new SelectDataModel(this.configCollection);
            var controller = new SelectViewController(dataModel);
            controller.Run(this.viewDisplay);
            controller.OnConfigSelected += this.OnConfigSelected;
        }

        public void GoToImport()
        {
            new ImportViewController().Run(this.viewDisplay);
        }

        private void OnConfigSelected(IModelTrainingConfig<IModelConfig> config)
        {
            this.CustomModel = new CustomModel(this.handsModel, this.configCollection, config);
        }

        private void UpdatePracticeEnabledState()
        {
            this.View.PracticeBtnEnabled = this.CustomModel != null;
        }
    }
}
