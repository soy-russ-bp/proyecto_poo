using System.Runtime.Versioning;
using ARJE.SignPractice.Controllers;
using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using ReactiveUI;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal sealed class MainWindowViewModel : ReactiveObject, IViewDisplay
    {
        private readonly IAsyncVideoSource<Matrix> videoSource =
            new Webcam(outputFlipType: FlipType.Horizontal);

        private readonly CustomModel customModel = null;

        private IViewModel<ViewBase>? content;

        public MainWindowViewModel()
        {
            this.GoToHome();
        }

        public IViewModel<ViewBase>? Content
        {
            get => this.content;
            private set
            {
                this.content?.Dispose();
                this.content = value;
                this.RaisePropertyChanged();
            }
        }

        public void GoToHome()
        {
            new HomeViewController().Run(this);
        }

        public void GoToPractice()
        {
            var dataModel = new PracticeDataModel(this.videoSource, this.customModel);
            new PracticeViewController(dataModel).Run(this);
        }

        public void GoToImport()
        {
            new ImportViewController().Run(this);
        }

        public void GoToSelect()
        {
            new SelectViewController().Run(this);
        }

        void IViewDisplay.SetContent(IViewModel<ViewBase> content)
        {
            this.Content = content;
        }
    }
}
