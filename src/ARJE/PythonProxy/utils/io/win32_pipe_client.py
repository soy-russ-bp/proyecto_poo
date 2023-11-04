import typing
import time
from io import RawIOBase
import win32file
import pywintypes
from namedpipe._win32 import Win32RawIO, PyHANDLE
from utils.io.pipe_client import PipeClient


class Win32PipeClient(PipeClient):

    @staticmethod
    def _connect_pipe(pipe_identifier: str) -> PyHANDLE:
        pipe_path = rf"\\.\pipe\{pipe_identifier}"
        while True:
            try:
                handle = win32file.CreateFile(
                    pipe_path,
                    win32file.GENERIC_READ | win32file.GENERIC_WRITE,
                    0,
                    None,
                    win32file.OPEN_EXISTING,
                    0,
                    None
                )
                return typing.cast(PyHANDLE, handle)
            except pywintypes.error as error:
                if error.args[0] == 2:
                    time.sleep(1)
                else:
                    raise error

    @staticmethod
    def create(pipe_identifier: str) -> PipeClient:
        return Win32PipeClient(pipe_identifier)

    def connect(self) -> RawIOBase:
        pipe_handle = Win32PipeClient._connect_pipe(self._pipe_identifier)
        self._stream = Win32RawIO(pipe_handle, True, True)
        return self._stream

    def close(self):
        if self._stream is not None:
            self._stream.close()
