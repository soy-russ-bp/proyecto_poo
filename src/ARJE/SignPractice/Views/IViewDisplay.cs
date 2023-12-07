using ARJE.SignPractice.ViewModels;

namespace ARJE.SignPractice.Views
{
    public interface IViewDisplay
    {
        public void SetContent(IViewModel<ViewBase> content);
    }
}
