from __future__ import annotations
from abc import ABC, abstractmethod
from io import RawIOBase


class PipeClient(ABC):

    @staticmethod
    @abstractmethod
    def create(pipe_identifier: str) -> PipeClient:
        pass

    @abstractmethod
    def wait(self) -> RawIOBase:
        pass

    @abstractmethod
    def close(self):
        pass
