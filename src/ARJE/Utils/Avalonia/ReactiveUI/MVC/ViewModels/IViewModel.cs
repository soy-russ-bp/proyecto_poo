using ARJE.Utils.Avalonia.ReactiveUI.MVC.Views;

namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels
{
    public interface IViewModel<out TView>
        where TView : ViewBase
    {
        public TView View { get; }
    }
}
