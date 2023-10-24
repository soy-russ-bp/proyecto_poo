import typing
from abc import ABC, abstractmethod
from utils.io.named_pipe import NamedPipe


T = typing.TypeVar("T")


class OutboundProxyPacket(ABC, typing.Generic[T]):

    def __init__(self, data: T):
        self.data = data

    @abstractmethod
    def compute_length(self) -> int:
        pass

    @abstractmethod
    def send_object(self, pipe: NamedPipe):
        pass
