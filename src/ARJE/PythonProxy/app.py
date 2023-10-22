import numpy
import numpy.typing
from holistic_model import HolisticModel
from csharp_proxy import CSharpProxy
from detection_collection_packet import DetectionCollectionPacket
from io_utils.binary_reader import BinaryReader


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
            proxy.send_packet(202, DetectionCollectionPacket(results))
