import typing
from io import RawIOBase
from namedpipe import NPopen
from utils.io import endianness
from utils.io.binary_reader import BinaryReader
from utils.io.binary_writer import BinaryWriter


class NamedPipe:

    def __init__(
            self,
            pipe_name: str,
            mode: typing.Optional[str] = "r+",
            byte_order: endianness.ByteOrderLiterals = "little"
    ):
        self._pipe: typing.Final = NPopen(
            mode=mode,
            bufsize=0,
            name=pipe_name)
        self._stream: RawIOBase | None = None
        self._byte_order: typing.Final[endianness.ByteOrderLiterals] = byte_order
        self._reader: BinaryReader | None = None
        self._writer: BinaryWriter | None = None

    def start(self) -> typing.Self:
        self._stream = typing.cast(RawIOBase, self._pipe.wait())
        self._reader = BinaryReader(self._stream, True, self._byte_order)
        self._writer = BinaryWriter(self._stream, True, self._byte_order)
        return self

    def _not_started_error(self) -> typing.NoReturn:
        raise RuntimeError("Pipe not started.")

    @property
    def byte_order(self) -> endianness.ByteOrderLiterals:
        return self._byte_order

    @property
    def reader(self) -> BinaryReader:
        if self._reader is None:
            self._not_started_error()

        return self._reader

    @property
    def writer(self) -> BinaryWriter:
        if self._writer is None:
            self._not_started_error()

        return self._writer

    def close(self):
        self._pipe.close()
        self._stream = typing.cast(RawIOBase, None)

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
