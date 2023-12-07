using System;
using ARJE.SignPractice.Views;

namespace ARJE.SignPractice.ViewModels
{
    public interface IViewModel<out TView> : IDisposable
        where TView : ViewBase
    {
        public TView View { get; }
    }
}
