using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace ARJE.Utils.AI
{
    public class LandmarkCollection
    {
        public LandmarkCollection(IList<Vector3> positions, IList<LandmarkConnection> connections)
        {
            this.Positions = positions.AsReadOnly();
            this.Connections = connections.AsReadOnly();
        }

        public ReadOnlyCollection<Vector3> Positions { get; }

        public ReadOnlyCollection<LandmarkConnection> Connections { get; }
    }
}
