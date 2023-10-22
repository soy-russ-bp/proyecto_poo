from detection import Detection
from io_utils import seven_bit_encoding
from outbound_proxy_packet import OutboundProxyPacket
from named_pipe import NamedPipe
from landmarks import Vector3, LandmarkCollection


TContent = list[Detection]


class DetectionCollectionPacket(OutboundProxyPacket[TContent]):

    def __init__(self, data: TContent):
        super().__init__(data)

    def compute_length(self) -> int:
        detection_count = len(self.data)
        if (detection_count == 0):
            return 1

        length = 1
        detection: Detection
        for detection in self.data:
            landmarks: LandmarkCollection = detection.landmarks
            length += seven_bit_encoding.get_encoded_len(detection.name) + 4 + (len(landmarks.positions) * (3 * 4))

        return length

    def send_object(self, pipe: NamedPipe):
        detection_count = len(self.data)
        if (detection_count == 0):
            pipe.writer.write_byte(0)
            return

        # pipe.reset_write_count()
        pipe.writer.write_byte(detection_count)

        detection: Detection
        for detection in self.data:
            landmarks: LandmarkCollection = detection.landmarks
            pipe.writer.write_string(detection.name)
            pipe.writer.write_int(len(landmarks.positions))

            landmarkPos: Vector3
            for landmarkPos in detection.landmarks.positions:
                pipe.writer.write_float(landmarkPos.x)
                pipe.writer.write_float(landmarkPos.y)
                pipe.writer.write_float(landmarkPos.z)

        # write_count: int = pipe.get_write_count()
        # print(f"{write_count} / {self.compute_length()}")
        # assert write_count == self.compute_length(), f"{write_count} != {self.compute_length()}"
