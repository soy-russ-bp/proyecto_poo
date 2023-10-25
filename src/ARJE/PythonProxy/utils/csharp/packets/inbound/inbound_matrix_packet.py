import typing
import numpy.typing
from utils.csharp.packets.inbound_proxy_packet import InboundProxyPacket
from utils.io.binary_reader import BinaryReader


ObjectT = numpy.ndarray[typing.Any, numpy.dtype[numpy.uint8]]


class InboundMatrixPacket(InboundProxyPacket[ObjectT]):

    def read_object(self, packet_reader: BinaryReader) -> ObjectT:
        width: int = packet_reader.read_int()
        height: int = packet_reader.read_int()
        pixels: bytes = packet_reader.read_all_bytes()
        flat_array = numpy.frombuffer(pixels, numpy.uint8)
        matrix: ObjectT = flat_array.reshape((height, width, 3))
        return matrix
