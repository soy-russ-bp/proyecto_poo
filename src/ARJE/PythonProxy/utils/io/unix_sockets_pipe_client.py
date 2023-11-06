import sys
import time
import socket
from socket import socket as socketType
from io import RawIOBase
from utils.io.pipe_client import PipeClient
from utils.io.socket_io import SocketIo


class UnixSocketsPipeClient(PipeClient):

    @staticmethod
    def _connect_pipe(pipe_identifier: str) -> socketType:
        if sys.platform == "win32":
            raise RuntimeError("Use Win32PipeClient instead.")

        client = socketType(socket.AF_UNIX, socket.SOCK_STREAM)
        while True:
            try:
                client.connect(pipe_identifier)
                return client
            except FileNotFoundError:
                time.sleep(1)

    @staticmethod
    def create(pipe_identifier: str) -> PipeClient:
        return UnixSocketsPipeClient(pipe_identifier)

    def connect(self) -> RawIOBase:
        pipe_socket: socketType = UnixSocketsPipeClient._connect_pipe(self._pipe_identifier)
        self._stream = SocketIo(pipe_socket)
        return self._stream

    def close(self):
        if self._stream is not None:
            self._stream.close()
