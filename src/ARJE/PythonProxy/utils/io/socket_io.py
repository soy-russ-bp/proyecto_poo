from io import RawIOBase
from socket import socket as socketType
from typing_extensions import Buffer


class SocketIo(RawIOBase):
    def __init__(self, socket: socketType):
        super().__init__()
        self._socket = socket

    def read(self, __size: int) -> bytes | None:
        return self._socket.recv(__size)

    def write(self, __b: Buffer) -> int | None:
        self._socket.send(__b)

    def close(self) -> None:
        # pylint: disable=using-constant-test
        if self.closed:
            return

        self._socket.close()
        super().close()
