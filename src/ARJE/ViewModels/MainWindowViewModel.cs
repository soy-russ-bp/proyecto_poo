using Arje.Views;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Arje.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase content;
        PracticeViewModel practiceVM;

        public MainWindowViewModel()
        {
            content = new HomeViewModel();
        }

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }
        public void SetFrame(Bitmap bm)
        {
            practiceVM.SetFrame(bm);
        }
        public void GoToPractice()
        {
            practiceVM = new PracticeViewModel();
            Content = practiceVM;
        }
        public void GoToCreate()
        {
            Content = new CreationViewModel();
        }
        public void GoToImport()
        {
            Content = new ImportViewModel();
        }
        public void GoToHome()
        {
            Content = new HomeViewModel();
        }
        public void GoToSelect()
        {
            Content = new SelectionViewModel();
        }

    }
}