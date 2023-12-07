using System;

namespace ARJE.SignPractice.DataModels
{
    public abstract record DataModelBase : IDisposable
    {
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
