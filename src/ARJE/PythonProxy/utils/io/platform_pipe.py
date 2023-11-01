import typing
import sys
from namedpipe import NPopen
from utils.io.pipe_connection import PipeConnection
from utils.io.posix_named_pipe import PosixNamedPipe


def get_platform_connection(pipe_name: str, mode: typing.Optional[str]) -> PipeConnection:
    if sys.platform.startswith("win"):
        return typing.cast(PipeConnection, NPopen(mode=mode, bufsize=0, name=pipe_name))

    return PosixNamedPipe(pipe_name, mode)
