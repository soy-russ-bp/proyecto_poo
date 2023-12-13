using System;
using System.Runtime.Versioning;
using ARJE.SignPractice.Controllers;
using ARJE.Utils.Avalonia.MVC.Views;
using ReactiveUI;

namespace ARJE.SignPractice
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal sealed class MainViewDisplay : ReactiveObject, IViewDisplay
    {
        private IDisposable? controller;

        private ViewBase? content;

        public MainViewDisplay()
        {
            new HomeViewController(this).Run();
        }

        public ViewBase? Content
        {
            get => this.content;
            private set
            {
                this.controller?.Dispose();
                this.content = value;
                this.RaisePropertyChanged();
            }
        }

        void IViewDisplay.SetContent(ViewBase content, IDisposable controller)
        {
            this.Content = content;
            this.controller = controller;
        }
    }
}
