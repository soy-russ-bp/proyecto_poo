from __future__ import annotations
import typing
from abc import ABC, abstractmethod
from io import RawIOBase


class PipeClient(ABC):

    def __init__(self, pipe_identifier: str):
        self._pipe_identifier: typing.Final = pipe_identifier
        self._stream: RawIOBase | None = None

    @property
    def pipe_identifier(self) -> str:
        return self._pipe_identifier

    @staticmethod
    @abstractmethod
    def create(pipe_identifier: str) -> PipeClient:
        pass

    @abstractmethod
    def connect(self) -> RawIOBase:
        pass

    @abstractmethod
    def close(self):
        pass
