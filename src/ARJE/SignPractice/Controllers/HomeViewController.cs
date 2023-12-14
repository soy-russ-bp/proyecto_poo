using System;
using System.Threading.Tasks;
using ARJE.Shared.Models;
using ARJE.Shared.Proxy;
using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.MVC.Views;
using Avalonia.Threading;

namespace ARJE.SignPractice.Controllers
{
    public sealed class HomeViewController : ViewControllerBase<HomeView, HomeDataModel>
    {
        private readonly IViewDisplay viewDisplay;

        private CustomModel? customModel;

        public HomeViewController(IViewDisplay viewDisplay)
            : base(HomeDataModel.Default)
        {
            Instance = this;
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
            await this.Model.HandsModel.StartAsync(PythonProxyApp.AppInfo);
        }

        public void GoToHome()
        {
            this.Run(this.viewDisplay);
        }

        public void GoToPractice()
        {
            ArgumentNullException.ThrowIfNull(this.CustomModel);

            var dataModel = new PracticeDataModel(this.Model.VideoSource, this.CustomModel);
            new PracticeViewController(dataModel).Run(this.viewDisplay);
        }

        public void GoToSelect()
        {
            var dataModel = new SelectDataModel(this.Model.ConfigCollection, this.CustomModel?.TrainingConfig);
            var controller = new SelectViewController(dataModel);
            controller.Run(this.viewDisplay);
            controller.OnConfigSelected += this.OnConfigSelected;
        }

        public void GoToImport()
        {
            var dataModel = new ImportDataModel(this.Model.ConfigCollection.SaveDirectory);
            new ImportViewController(dataModel).Run(this.viewDisplay);
        }

        private void OnConfigSelected(IModelTrainingConfig<IModelConfig> config)
        {
            this.CustomModel = new CustomModel(this.Model.HandsModel, this.Model.ConfigCollection, config);
        }

        private void UpdatePracticeEnabledState()
        {
            this.View.PracticeBtnEnabled = this.CustomModel != null;
        }
    }
}
