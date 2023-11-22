using System;
using ARJE.Utils.Video;
using ARJE.Utils.Video.OpenCv;
using ReactiveUI;

namespace ARJE.SignPractice.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;
        private PracticeViewModel? practiceVM;
        private readonly Webcam webcam = new(outputFlipType: FlipType.Horizontal);

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
            this.practiceVM = new PracticeViewModel(webcam);
            this.Content = this.practiceVM;
        }
        public void GoToCreate()
        {
            this.Content = new CreationViewModel();
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
