from __future__ import annotations
from abc import ABC, abstractmethod
import typing
from utils.io.binary_reader import BinaryReader


ObjectT = typing.TypeVar("ObjectT")


class InboundProxyPacket(ABC, typing.Generic[ObjectT]):

    @abstractmethod
    def read_object(self, packet_reader: BinaryReader) -> ObjectT:
        pass
