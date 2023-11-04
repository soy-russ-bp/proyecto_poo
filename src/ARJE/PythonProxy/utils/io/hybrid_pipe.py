import typing
import sys
from utils.io import endianness
from utils.io.binary_reader import BinaryReader
from utils.io.binary_writer import BinaryWriter
from utils.io.pipe_client import PipeClient
from utils.io.win32_pipe_client import Win32PipeClient


class HybridPipe:

    @staticmethod
    def _get_platform_client(pipe_identifier: str) -> PipeClient:
        if sys.platform == "win32":
            return Win32PipeClient.create(pipe_identifier)

        return None

    def __init__(
            self,
            pipe_identifier: str,
            byte_order: endianness.ByteOrderLiterals = "little"
    ):
        self._pipe: typing.Final = HybridPipe._get_platform_client(pipe_identifier)
        self._byte_order: typing.Final[endianness.ByteOrderLiterals] = byte_order
        self._reader: BinaryReader | None = None
        self._writer: BinaryWriter | None = None

    def start(self) -> typing.Self:
        pipe_stream = self._pipe.wait()
        self._reader = BinaryReader(pipe_stream, True, self._byte_order)
        self._writer = BinaryWriter(pipe_stream, True, self._byte_order)
        return self

    def _not_open_error(self) -> typing.NoReturn:
        raise RuntimeError("Pipe not open.")

    @property
    def byte_order(self) -> endianness.ByteOrderLiterals:
        return self._byte_order

    @property
    def reader(self) -> BinaryReader:
        if self._reader is None:
            self._not_open_error()

        return self._reader

    @property
    def writer(self) -> BinaryWriter:
        if self._writer is None:
            self._not_open_error()

        return self._writer

    def close(self):
        self._pipe.close()

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
