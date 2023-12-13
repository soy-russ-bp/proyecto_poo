from io import RawIOBase
from socket import socket as socketType
from typing_extensions import Buffer


class SocketIo(RawIOBase):
    def __init__(self, socket: socketType):
        super().__init__()
        self._socket = socket

    def read(self, __size: int) -> bytes | None:
        buffer = bytearray(__size)
        read_count: int = 0
        while read_count < __size:
            remaining_size = __size - read_count
            recv_data = self._socket.recv(remaining_size)
            recv_count = len(recv_data)
            if recv_count == 0:
                break

            recv_end = read_count + recv_count
            buffer[read_count:recv_end] = recv_data
            read_count += recv_count

        return bytes(buffer)

    def write(self, __b: Buffer) -> int | None:
        self._socket.send(__b)

    def close(self) -> None:
        # pylint: disable=using-constant-test
        if self.closed:
            return

        self._socket.close()
        super().close()
