from __future__ import annotations
import typing
from abc import ABC, abstractmethod
from utils.io.binary_writer import BinaryWriter


T = typing.TypeVar("T")


class OutboundProxyPacket(ABC, typing.Generic[T]):

    def __init__(self, object: T):
        self.wrapped_object: typing.Final = object

    @property
    @abstractmethod
    def length(self) -> int:
        pass

    @abstractmethod
    def send_object(self, pipe_writer: BinaryWriter):
        pass
