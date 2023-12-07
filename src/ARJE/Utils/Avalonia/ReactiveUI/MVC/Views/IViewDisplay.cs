using System;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;

namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.Views
{
    public interface IViewDisplay
    {
        public void SetContent(IViewModel<ViewBase> content, IDisposable controller);
    }
}
