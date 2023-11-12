using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ARJE.Utils.Collections.Extensions;

namespace ARJE.Utils.Python.Proxy
{
    public sealed class AutoIdMapper : IIdMapper
    {
        public AutoIdMapper(IReadOnlySet<Type> types)
        {
            this.AllowedTypes = types;
            this.IdsMap = CreateIdsMap(types);
        }

        public IReadOnlySet<Type> AllowedTypes { get; }

        private ReadOnlyDictionary<Type, int> IdsMap { get; }

        public int GetTypeId(Type type)
        {
            if (this.IdsMap.TryGetValue(type, out int id))
            {
                return id;
            }

            throw new InvalidOperationException();
        }

        private static ReadOnlyDictionary<Type, int> CreateIdsMap(IReadOnlySet<Type> types)
        {
            return types.Enumerated().ToDictionary(type => type.Item, type => type.Index).AsReadOnly();
        }
    }
}
