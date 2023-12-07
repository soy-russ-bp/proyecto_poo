using System.Collections.Generic;

namespace ARJE.Utils.Json.Converters
{
    public interface IObjectAsArray<T>
    {
        public IList<T> ObjectAsArray { get; }

        public static abstract IObjectAsArray<T> Create(IList<T> objectAsArray);
    }
}
