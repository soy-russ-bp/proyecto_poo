import typing
import io_utils.endianness as endianness
import struct
from io import RawIOBase


class BinaryWriter:

    def __init__(self, stream: RawIOBase, leave_open: bool, byte_order: endianness.ByteOrderLiterals = "little"):
        self._stream: typing.Final[RawIOBase] = stream
        self._leave_open: typing.Final[bool] = leave_open
        self._byte_order: typing.Final[endianness.ByteOrderLiterals] = byte_order
        self._float_byte_order_fmt: typing.Final[str] = endianness.get_float_format(byte_order)

    def write_bytes(self, data: bytes):
        self._stream.write(data)

    def _write_integral_num(self, data: int, length: int, signed: bool = True):
        dataAsBytes: bytes = data.to_bytes(length=length, byteorder=self._byte_order, signed=signed)
        self.write_bytes(dataAsBytes)

    def write_byte(self, data: int):
        self._write_integral_num(data, 1, signed=False)

    def write_int(self, data: int):
        self._write_integral_num(data, 4)

    def write_float(self, data: float):
        dataAsBytes: bytes = struct.pack(self._float_byte_order_fmt, data)
        self.write_bytes(dataAsBytes)

    def write_string(self, data: str):
        length: int = len(data)
        if (length > 127):
            raise ValueError("Length > 127 is not supported.")

        self.write_byte(length)
        for ch in data:
            numVal: int = ord(ch)
            if (length > 127):
                raise ValueError("Char values > 127 are not supported.")

            self.write_byte(numVal)

    def close(self):
        if (not self._leave_open):
            self._stream.close()

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
