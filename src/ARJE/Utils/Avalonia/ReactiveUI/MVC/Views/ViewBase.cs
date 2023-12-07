using System;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;
using Avalonia.Controls;

namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.Views
{
    public abstract class ViewBase : UserControl
    {
        protected ViewBase()
        {
            this.OnInitializeComponent();
            this.DataContextChanged += this.OnDataContextChanged;
        }

        protected abstract void OnInitializeComponent();

        private void OnDataContextChanged(object? sender, EventArgs e)
        {
            if (this.DataContext == null)
            {
                return;
            }

            ((IViewModelInit)this.DataContext).Init(this);
        }
    }
}
