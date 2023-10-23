import struct
from utils.io.binary_accessor import BinaryAccessor


class BinaryWriter(BinaryAccessor):

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
