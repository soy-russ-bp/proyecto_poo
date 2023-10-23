using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using ARJE.Utils.AI;

namespace ARJE.Utils.Python.Proxy.Packets.Inbound
{
    public class InboundDetectionCollectionPacket : IInboundProxyPacket<InboundDetectionCollectionPacket, ReadOnlyCollection<Detection>>
    {
        private InboundDetectionCollectionPacket()
        {
        }

        static ReadOnlyCollection<Detection> IInboundProxyPacket<InboundDetectionCollectionPacket, ReadOnlyCollection<Detection>>.ReceiveObject(BinaryReader packetReader)
        {
            byte detectionCount = packetReader.ReadByte();
            var detections = new List<Detection>(detectionCount);
            for (int i = 0; i < detectionCount; i++)
            {
                Detection detection = ReadDetection(packetReader);
                detections.Add(detection);
            }

            return detections.AsReadOnly();
        }

        private static Detection ReadDetection(BinaryReader packetReader)
        {
            string detectionName = packetReader.ReadString();
            LandmarkCollection landmarks = ReadLandmarks(packetReader);
            return new Detection(detectionName, landmarks);
        }

        private static LandmarkCollection ReadLandmarks(BinaryReader packetReader)
        {
            IList<Vector3> landmarks = ReadLandmarksPositions(packetReader);
            IList<LandmarkConnection> connections = ReadLandmarksConnections(packetReader);
            return new LandmarkCollection(landmarks, connections);
        }

        private static IList<Vector3> ReadLandmarksPositions(BinaryReader packetReader)
        {
            int landmarkCount = packetReader.ReadInt32();
            var landmarks = new List<Vector3>(landmarkCount);
            for (int landmarkI = 0; landmarkI < landmarkCount; landmarkI++)
            {
                float x = packetReader.ReadSingle();
                float y = packetReader.ReadSingle();
                float z = packetReader.ReadSingle();
                var landmark = new Vector3(x, y, z);
                landmarks.Add(landmark);
            }

            return landmarks;
        }

        private static IList<LandmarkConnection> ReadLandmarksConnections(BinaryReader packetReader)
        {
            return Array.Empty<LandmarkConnection>();

            // TODO ...
            /*
            byte connectionCount = packetReader.ReadByte();
            var connections = new List<LandmarkConnection>(connectionCount);
            for (int connectionI = 0; connectionI < connectionCount; connectionI++)
            {
                byte startIndex = packetReader.ReadByte();
                byte endIndex = packetReader.ReadByte();
                var connection = new LandmarkConnection(startIndex, endIndex);
                connections.Add(connection);
            }

            //return connections;
            */
        }
    }
}
