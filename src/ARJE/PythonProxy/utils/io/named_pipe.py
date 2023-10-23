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
        self._stream = typing.cast(RawIOBase, None)
        self._byte_order: typing.Final[endianness.ByteOrderLiterals] = byte_order
        self._reader = typing.cast(BinaryReader, None)
        self._writer = typing.cast(BinaryWriter, None)

    def start(self) -> typing.Self:
        self._stream = typing.cast(RawIOBase, self._pipe.wait())
        self._reader = BinaryReader(self._stream, True, self._byte_order)
        self._writer = BinaryWriter(self._stream, True, self._byte_order)
        return self

    @property
    def byte_order(self) -> endianness.ByteOrderLiterals:
        return self._byte_order

    @property
    def reader(self) -> BinaryReader:
        return self._reader

    @property
    def writer(self) -> BinaryWriter:
        return self._writer

    def close(self):
        self._pipe.close()
        self._stream = typing.cast(RawIOBase, None)

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
