using System;

namespace ARJE.Utils.Avalonia.MVC.Views
{
    public interface IViewDisplay
    {
        public void SetContent(ViewBase content, IDisposable controller);
    }
}
