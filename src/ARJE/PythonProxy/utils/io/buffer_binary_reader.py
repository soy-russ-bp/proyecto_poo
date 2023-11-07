from io import BytesIO
import typing
from utils.io import endianness
from utils.io.binary_reader import BinaryReader


class BufferBinaryReader(BinaryReader):

    def __init__(
        self,
        buffer: typing.Union[bytes, bytearray],
        byte_order: endianness.ByteOrderLiterals = "little"
    ):
        stream = BytesIO(buffer)
        buffer_length: int = len(buffer)
        super().__init__(stream, False, byte_order)
        self._length: typing.Final[int] = buffer_length
        self._read_count: int = 0

    @property
    def length(self) -> int:
        return self._length

    @property
    def read_count(self) -> int:
        return self._read_count

    @property
    def remaining_count(self) -> int:
        return self.length - self.read_count

    def read_bytes(self, count: int) -> bytes:
        data = super().read_bytes(count)
        self._read_count += len(data)
        return data

    def read_all_bytes(self) -> bytes:
        count: int = self.remaining_count
        return self.read_bytes(count)
