import typing
from abc import ABC
from io import IOBase
from utils.io import endianness


class BinaryAccessor(ABC):

    def __init__(self, stream: IOBase, leave_open: bool, byte_order: endianness.ByteOrderLiterals = "little"):
        self._stream: typing.Final = stream
        self._leave_open: typing.Final = leave_open
        self._byte_order: typing.Final = byte_order
        self._float_byte_order_fmt: typing.Final[str] = endianness.get_float_format(byte_order)

    def close(self):
        if (not self._leave_open):
            self._stream.close()

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self.close()
