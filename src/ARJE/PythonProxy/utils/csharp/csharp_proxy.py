import typing
from utils.io import endianness
from utils.io.named_pipe import NamedPipe
from utils.csharp.packets.inbound_proxy_packet import InboundProxyPacket
from utils.csharp.packets.outbound_proxy_packet import OutboundProxyPacket


TObject = typing.TypeVar("TObject")


class CSharpProxy:

    def __init__(self, pipe_name: str):
        self._pipe = NamedPipe(pipe_name)

    def start(self) -> typing.Self:
        self._pipe.start()
        return self

    @property
    def byte_order(self) -> endianness.ByteOrderLiterals:
        return self._pipe.byte_order

    def receive_object(self, packet: InboundProxyPacket[TObject]) -> TObject:
        return InboundProxyPacket.receive_object(packet, self._pipe)

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
