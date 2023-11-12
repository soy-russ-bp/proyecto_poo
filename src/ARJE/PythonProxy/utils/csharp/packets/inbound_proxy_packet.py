from __future__ import annotations
from abc import ABC, abstractmethod
import typing
from utils.io.buffer_binary_reader import BufferBinaryReader


ObjectT = typing.TypeVar("ObjectT")


class InboundProxyPacket(ABC, typing.Generic[ObjectT]):

    @abstractmethod
    def read_object(self, reader: BufferBinaryReader) -> ObjectT:
        pass
