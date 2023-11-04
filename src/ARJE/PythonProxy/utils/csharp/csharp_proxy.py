import typing
from utils.io.hybrid_pipe import HybridPipe
from utils.csharp.packets.inbound_proxy_packet import InboundProxyPacket
from utils.csharp.packets.outbound_proxy_packet import OutboundProxyPacket
from utils.csharp.packets.packet_reader import PacketReader
from utils.csharp.packets.packet_writer import PacketWriter


ObjectT = typing.TypeVar("ObjectT")


class CSharpProxy:

    def __init__(
        self,
        pipe_identifier: str
    ):
        self._pipe = HybridPipe(pipe_identifier)
        self._packet_reader: PacketReader | None = None
        self._packet_writer: PacketWriter | None = None

    def start(self) -> typing.Self:
        self._pipe.start()
        self._packet_reader = PacketReader(self._pipe.reader)
        self._packet_writer = PacketWriter(self._pipe.writer)
        return self

    def _not_open_error(self) -> typing.NoReturn:
        raise RuntimeError("Proxy not open.")

    def receive_object(self, packet: InboundProxyPacket[ObjectT]) -> ObjectT:
        if self._packet_reader is None:
            self._not_open_error()

        return self._packet_reader.read_object(packet)

    def send_object(self, packet: OutboundProxyPacket[typing.Any]):
        if self._packet_writer is None:
            self._not_open_error()

        self._packet_writer.write_object(packet)

    def close(self):
        self._pipe.close()

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
