import typing
import sys
from utils.io import endianness
from utils.io.binary_reader import BinaryReader
from utils.io.binary_writer import BinaryWriter
if sys.platform == "win32":
    from utils.io.win32_pipe_client import Win32PipeClient as PlatformPipeClient
else:
    from utils.io.unix_sockets_pipe_client import UnixSocketsPipeClient as PlatformPipeClient


class HybridPipe:

    def __init__(
            self,
            pipe_identifier: str,
            byte_order: endianness.ByteOrderLiterals = "little"
    ):
        self._pipe: typing.Final = PlatformPipeClient.create(pipe_identifier)
        self._byte_order: typing.Final[endianness.ByteOrderLiterals] = byte_order
        self._reader: BinaryReader | None = None
        self._writer: BinaryWriter | None = None

    def start(self) -> typing.Self:
        print(f"Connecting to {self._pipe.pipe_identifier}...")
        pipe_stream = self._pipe.connect()
        print("Connected")
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
