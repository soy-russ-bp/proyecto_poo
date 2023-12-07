using System;

namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels
{
    public abstract record DataModelBase : IDisposable
    {
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
