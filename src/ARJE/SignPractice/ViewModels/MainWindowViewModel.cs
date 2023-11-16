using ReactiveUI;

namespace ARJE.SignPractice.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;

        public MainWindowViewModel()
        {
            this.content = new HomeViewModel();
        }

        public ViewModelBase Content
        {
            get => this.content;
            private set => this.RaiseAndSetIfChanged(ref this.content, value);
        }

        public void GoToPractice()
        {
            this.Content = new PracticeViewModel();
        }
        public void GoToCreate()
        {
            this.Content = new PracticeViewModel();
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
        public void Exit()
        {
            //Close App
        }

    }
}