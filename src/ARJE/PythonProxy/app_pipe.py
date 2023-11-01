import sys


def get_pipe_input_type() -> str:
    if sys.platform.startswith("win"):
        return "name"

    return "path"


def get_pipe_name_in_ctx() -> str:
    args: list[str] = sys.argv
    input_type: str = get_pipe_input_type()
    if len(args) > 1:
        pipe_name: str = args[1]
        print(f"Pipe {input_type}: {pipe_name}")
        return pipe_name

    return input(f"Pipe {input_type}: ")
