using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;
using ARJE.Utils.Json.Converters;
using Newtonsoft.Json;

namespace ARJE.Utils.AI
{
    [JsonConverter(typeof(ObjectAsArrayConverter<LandmarkCollection, Vector3>))]
    public class LandmarkCollection : IObjectAsArray<Vector3>
    {
        public LandmarkCollection(IList<Vector3> positions, IList<LandmarkConnection> connections)
        {
            this.Positions = positions.AsReadOnly();
            this.Connections = connections.AsReadOnly();
        }

        public ReadOnlyCollection<Vector3> Positions { get; }

        public ReadOnlyCollection<LandmarkConnection> Connections { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IList<Vector3> IObjectAsArray<Vector3>.ObjectAsArray => this.Positions;

        static IObjectAsArray<Vector3> IObjectAsArray<Vector3>.Create(IList<Vector3> objectAsArray)
        {
            return new LandmarkCollection(objectAsArray, Array.Empty<LandmarkConnection>());
        }
    }
}
