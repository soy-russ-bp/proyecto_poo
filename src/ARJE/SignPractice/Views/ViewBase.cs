using ARJE.SignPractice.ViewModels;
using Avalonia.Controls;

namespace ARJE.SignPractice.Views
{
    public abstract class ViewBase : UserControl
    {
        protected ViewBase()
        {
            this.OnInitializeComponent();
            this.DataContextChanged += this.OnDataContextChanged;
        }

        protected abstract void OnInitializeComponent();

        private void OnDataContextChanged(object? sender, System.EventArgs e)
        {
            if (this.DataContext == null)
            {
                return;
            }

            ((IViewModelInit)this.DataContext).Init(this);
        }
    }
}
