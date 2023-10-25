import typing
from utils.io.binary_writer import BinaryWriter
from utils.csharp.packets.outbound_proxy_packet import OutboundProxyPacket


class PacketWriter:

    def __init__(self, pipe_writer: BinaryWriter):
        self._pipe_writer: typing.Final = pipe_writer

    def write_object(self, packet: OutboundProxyPacket[typing.Any]):
        type_id: int = 202 # TODO
        packet_length: int = packet.length
        self._pipe_writer.write_int(type_id)
        self._pipe_writer.write_int(packet_length)
        packet.send_object(self._pipe_writer)
