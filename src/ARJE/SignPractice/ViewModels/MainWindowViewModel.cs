using System;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using ReactiveUI;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly IAsyncVideoSource<Matrix> videoSource =
            new Webcam(outputFlipType: FlipType.Horizontal);

        private ViewModelBase content;

        private PracticeViewModel? practiceVM;

        public MainWindowViewModel()
        {
            this.content = new HomeViewModel();
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

        public void GoToPractice()
        {
            this.practiceVM = new PracticeViewModel(this.videoSource);
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
