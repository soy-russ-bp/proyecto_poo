using System;
using System.Collections.Generic;

namespace ARJE.Utils.Python.Proxy
{
    public interface IIdMapper
    {
        public IReadOnlySet<Type> AllowedTypes { get; }

        public int GetTypeId(Type type);
    }
}
