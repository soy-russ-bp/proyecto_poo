import typing
from utils.csharp.packets.inbound_proxy_packet import InboundProxyPacket
from utils.io.binary_reader import BinaryReader


TObject = typing.TypeVar("TObject")


class PacketReader:

    def __init__(self, pipe_reader: BinaryReader):
        self._pipe_reader: typing.Final = pipe_reader

    def _read_raw_packet(self) -> bytes:
        actual_type_id: int = self._pipe_reader.read_int()
        type_id: int = actual_type_id  # TODO
        if type_id != actual_type_id:
            raise RuntimeError(f"Type id mismatch. Expecting: {type_id}. Actual: {actual_type_id}.")

        package_len: int = self._pipe_reader.read_int()
        if package_len == 0:
            return bytes(0)

        data: bytes = self._pipe_reader.read_bytes(package_len)
        return data

    def read_object(self, packet: InboundProxyPacket[TObject]) -> TObject:
        packet_bytes: bytes = self._read_raw_packet()
        with BinaryReader.from_buffer(packet_bytes, self._pipe_reader.byte_order) as packet_reader:
            inbound_object: TObject = packet.read_object(packet_reader)
            return inbound_object
