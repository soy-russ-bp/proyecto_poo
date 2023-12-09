using ARJE.SignTrainer.App;
using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using ReactiveUI;
using System;
using System.IO;
using System.Runtime.Versioning;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly IAsyncVideoSource<Matrix> videoSource =
            new Webcam(outputFlipType: FlipType.Horizontal);

        private readonly CustomModel customModel = null;

        private ViewModelBase content;

        private PracticeViewModel? practiceVM;

        public static MainWindowViewModel Instance { get; private set; }

        public HandsModel HandsModel { get; } = new(new HandsModelConfig(1));
        public OnDiskModelTrainingConfigCollection ModelTrainingConfigCollection { get; }
        public MainWindowViewModel()
        {
            Keras.Keras.DisablePySysConsoleLog = true;
            Instance = this;
            HandsModel.Start(PythonProxyApp.AppInfo);
            this.content = new HomeViewModel();
            DirectoryInfo modelsDir = Directory.CreateDirectory("Models");
            ModelTrainingConfigCollection = new OnDiskModelTrainingConfigCollection(modelsDir);
        }

        public ViewModelBase Content
        {
            get => this.content;
            private set
            {
                (this.content as IDisposable)?.Dispose();
                this.RaiseAndSetIfChanged(ref this.content, value);
            }
        }

        public void GoToPractice(ModelTrainingConfig<HandsModelConfig> trainingConfig)
        {
            this.practiceVM = new PracticeViewModel(this.videoSource, this.HandsModel, trainingConfig, ModelTrainingConfigCollection);

            this.Content = this.practiceVM;
        }

        public void GoToImport()
        {
            this.Content = new ImportViewModel();
        }

        public void GoToHome()
        {
            this.Content = new HomeViewModel();
        }

        public void GoToSelect()
        {
            this.Content = new SelectionViewModel();
        }
    }
}
