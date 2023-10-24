from __future__ import annotations
import typing
from abc import ABC, abstractmethod
from utils.io.named_pipe import NamedPipe
from utils.io.binary_reader import BinaryReader


TObject = typing.TypeVar("TObject")


class InboundProxyPacket(ABC, typing.Generic[TObject]):

    @abstractmethod
    def read_object(self, packet: InboundProxyPacket[TObject], packet_reader: BinaryReader) -> TObject:
        pass

    @staticmethod
    def _receive_raw_packet(pipe_reader: BinaryReader) -> bytes:
        actual_type_id: int = pipe_reader.read_int()
        type_id: int = actual_type_id  # TODO
        if type_id != actual_type_id:
            raise RuntimeError(f"Type id mismatch. Expecting: {type_id}. Actual: {actual_type_id}.")

        package_len: int = pipe_reader.read_int()
        if package_len == 0:
            return bytes(0)

        data: bytes = pipe_reader.read_bytes(package_len)
        return data

    @staticmethod
    def receive_object(packet: InboundProxyPacket[TObject], pipe: NamedPipe) -> TObject:
        packet_bytes: bytes = InboundProxyPacket._receive_raw_packet(pipe.reader)
        with BinaryReader.from_buffer(packet_bytes, pipe.byte_order) as packet_reader:
            inbound_object: TObject = packet.read_object(packet, packet_reader)
            return inbound_object
