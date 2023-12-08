using System;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Views;

namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels
{
    public interface IViewModelInit
    {
        public event Action? OnViewInit;

        public void Init(ViewBase view);
    }
}
