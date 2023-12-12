using System;

namespace ARJE.Utils.Avalonia.MVC.Models
{
    public abstract record DataModelBase : IDataModel
    {
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
