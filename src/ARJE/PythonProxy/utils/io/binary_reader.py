from __future__ import annotations
import typing_extensions
from io import BytesIO
from utils.io import endianness
from utils.io.binary_accessor import BinaryAccessor


class BinaryReader(BinaryAccessor):

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
