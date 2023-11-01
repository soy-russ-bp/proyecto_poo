from io import RawIOBase
import typing
import time
import os
from utils.io.pipe_connection import PipeConnection


class PosixNamedPipe(PipeConnection):

    def __init__(self, pipe_name: str, mode: typing.Optional[str] = "r"):
        if mode is None:
            mode = "r"

        if 't' not in mode and 'b' not in mode:
            mode += 'b'

        self._pipe_path: typing.Final = pipe_name
        self._stream: RawIOBase | None = None
        self._open_args: typing.Final = {
            "mode": mode,
            "buffering": 0,
        }

    def wait(self) -> RawIOBase:
        while not os.path.isfile(self._pipe_path):
            time.sleep(1)

        # pylint: disable=unspecified-encoding
        # pylint: disable=consider-using-with
        self._stream = typing.cast(RawIOBase, open(self._pipe_path, **self._open_args))  # type: ignore
        return self._stream

    def close(self):
        if self._stream is not None:
            self._stream.close()
