from __future__ import annotations
import typing
from io import IOBase, BytesIO
import typing_extensions
from utils.io import endianness


class BinaryReader:

    def __init__(self, stream: IOBase, leave_open: bool, byte_order: endianness.ByteOrderLiterals = "little"):
        self._stream: typing.Final = stream
        self._leave_open: typing.Final = leave_open
        self._byte_order: typing.Final = byte_order

    @staticmethod
    def from_buffer(buffer: typing_extensions.Buffer, byte_order: endianness.ByteOrderLiterals = "little") -> BinaryReader:
        stream = BytesIO(buffer)
        return BinaryReader(stream, False, byte_order)

    def read_bytes(self, count: int | None) -> bytes:
        data: bytes | None = self._stream.read(count)
        if (data is None):
            raise RuntimeError("Pipe broken.")

        return data

    def read_all_bytes(self) -> bytes:
        return self.read_bytes(None)

    def _read_integral_num(self, length: int, signed: bool = True) -> int:
        data = self.read_bytes(length)
        return int.from_bytes(data, byteorder=self._byte_order, signed=signed)

    def read_byte(self) -> int:
        return self._read_integral_num(1, signed=False)

    def read_int(self) -> int:
        return self._read_integral_num(4)

    def close(self):
        if (not self._leave_open):
            self._stream.close()

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
