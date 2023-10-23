import numpy
import numpy.typing
from utils.ai.solutions.holistic_model import HolisticModel
from utils.csharp.csharp_proxy import CSharpProxy
from utils.csharp.packets.outbound.outbound_detection_collection_packet import OutboundDetectionCollectionPacket
from utils.io.binary_reader import BinaryReader


print(" - App start - ")


with HolisticModel() as model:
    with CSharpProxy("SignTrainer").start() as proxy:
        print(" - Proxy start - ")
        while True:
            packet: bytes = proxy.receive_packet(201)
            with BinaryReader.from_buffer(packet, proxy.byte_order) as packetReader:
                width: int = packetReader.read_int()
                height: int = packetReader.read_int()
                pixels = packetReader.read_all_bytes()
                flat_array = numpy.frombuffer(pixels, numpy.uint8)
                matrix = flat_array.reshape((height, width, 3))

            results = model.process(matrix)
            proxy.send_packet(202, OutboundDetectionCollectionPacket(results))
