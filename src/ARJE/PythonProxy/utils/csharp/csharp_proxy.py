import typing
from utils.io.named_pipe import NamedPipe
from utils.csharp.packets.outbound_proxy_packet import OutboundProxyPacket
from utils.io import endianness


class CSharpProxy:

    def __init__(self, pipe_name: str):
        self._pipe = NamedPipe(pipe_name)

    def start(self) -> typing.Self:
        self._pipe.start()
        return self

    @property
    def byte_order(self) -> endianness.ByteOrderLiterals:
        return self._pipe.byte_order

    def receive_packet(self, type_id: int) -> bytes:
        actual_type_id: int = self._pipe.reader.read_int()
        if type_id != actual_type_id:
            raise RuntimeError(f"Type id mismatch. Expecting: {type_id}. Actual: {actual_type_id}.")

        package_len: int = self._pipe.reader.read_int()
        if package_len == 0:
            return bytes(0)

        data: bytes = self._pipe.reader.read_bytes(package_len)
        return data

    def send_packet(self, type_id: int, packet: OutboundProxyPacket[typing.Any]):
        package_len: int = packet.compute_length()
        self._pipe.writer.write_int(type_id)
        self._pipe.writer.write_int(package_len)
        packet.send_object(self._pipe)

    def close(self):
        self._pipe.close()

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
