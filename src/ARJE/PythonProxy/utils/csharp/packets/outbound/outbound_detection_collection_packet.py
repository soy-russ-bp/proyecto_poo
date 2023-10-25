from utils.ai.detection import Detection
from utils.ai.landmarks import Vector3, LandmarkCollection
from utils.csharp.packets.outbound_proxy_packet import OutboundProxyPacket
from utils.io import seven_bit_encoding
from utils.io.binary_writer import BinaryWriter


class OutboundDetectionCollectionPacket(OutboundProxyPacket[list[Detection]]):

    @property
    def length(self) -> int:
        detection_count = len(self.wrapped_object)
        if detection_count == 0:
            return 1

        length = 1
        detection: Detection
        for detection in self.wrapped_object:
            landmarks: LandmarkCollection = detection.landmarks
            length += seven_bit_encoding.get_encoded_len(detection.name) + 4 + (len(landmarks.positions) * (3 * 4))

        return length

    def send_object(self, pipe_writer: BinaryWriter):
        detection_count = len(self.wrapped_object)
        if detection_count == 0:
            pipe_writer.write_byte(0)
            return

        pipe_writer.write_byte(detection_count)

        detection: Detection
        for detection in self.wrapped_object:
            landmarks: LandmarkCollection = detection.landmarks
            pipe_writer.write_string(detection.name)
            pipe_writer.write_int(len(landmarks.positions))

            landmark_pos: Vector3
            for landmark_pos in detection.landmarks.positions:
                pipe_writer.write_float(landmark_pos.x)
                pipe_writer.write_float(landmark_pos.y)
                pipe_writer.write_float(landmark_pos.z)
