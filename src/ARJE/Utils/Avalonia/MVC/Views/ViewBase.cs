using Avalonia.Controls;

namespace ARJE.Utils.Avalonia.MVC.Views
{
    public abstract class ViewBase : UserControl
    {
        protected ViewBase()
        {
            this.OnInitializeComponent();
        }

        protected abstract void OnInitializeComponent();
    }
}
