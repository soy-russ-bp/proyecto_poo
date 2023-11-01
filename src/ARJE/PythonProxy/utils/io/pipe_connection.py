from abc import ABC, abstractmethod
from io import RawIOBase


class PipeConnection(ABC):

    @abstractmethod
    def wait(self) -> RawIOBase:
        pass

    @abstractmethod
    def close(self):
        pass
