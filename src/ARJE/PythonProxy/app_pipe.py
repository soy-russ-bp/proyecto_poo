import sys


def get_pipe_identifier_type() -> str:
    if sys.platform == "win32":
        return "name"

    return "path"


def get_pipe_identifier_in_ctx() -> str:
    args: list[str] = sys.argv
    identifier_type: str = get_pipe_identifier_type()
    if len(args) > 1:
        pipe_name: str = args[1]
        print(f"Pipe {identifier_type}: {pipe_name}")
        return pipe_name

    return input(f"Pipe {identifier_type}: ")
