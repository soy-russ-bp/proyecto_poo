using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;

namespace ARJE.Utils.Python.Proxy.Packets.Inbound
{
    public readonly struct InboundHandDetectionCollectionPacket : IInboundProxyPacket<HandDetectionCollection>, INoArgsPacket
    {
        public HandDetectionCollection ReadObject(BinaryReader reader)
        {
            IList<HandDetection> detections = ReadDetections(reader);
            return new HandDetectionCollection(detections);
        }

        private static IList<HandDetection> ReadDetections(BinaryReader reader)
        {
            byte detectionCount = reader.ReadByte();
            if (detectionCount == 0)
            {
                return Array.Empty<HandDetection>();
            }

            return ReadDetections(reader, detectionCount);
        }

        private static IList<HandDetection> ReadDetections(BinaryReader reader, int detectionCount)
        {
            var detections = new HandDetection[detectionCount];
            for (int i = 0; i < detectionCount; i++)
            {
                detections[i] = ReadDetection(reader);
            }

            return detections;
        }

        private static HandDetection ReadDetection(BinaryReader reader)
        {
            LandmarkCollection landmarks = ReadLandmarks(reader);
            return new HandDetection(landmarks);
        }

        private static LandmarkCollection ReadLandmarks(BinaryReader reader)
        {
            IList<Vector3> landmarks = ReadLandmarksPositions(reader);
            IList<LandmarkConnection> connections = ReadLandmarksConnections(reader);
            return new LandmarkCollection(landmarks, connections);
        }

        private static IList<Vector3> ReadLandmarksPositions(BinaryReader reader)
        {
            int landmarkCount = reader.ReadInt32();
            var landmarks = new Vector3[landmarkCount];
            for (int landmarkI = 0; landmarkI < landmarkCount; landmarkI++)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                var landmark = new Vector3(x, y, z);
                landmarks[landmarkI] = landmark;
            }

            return landmarks;
        }

        private static IList<LandmarkConnection> ReadLandmarksConnections(BinaryReader reader)
        {
            // TODO
            return Array.Empty<LandmarkConnection>();
        }
    }
}
