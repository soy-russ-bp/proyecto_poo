import sys
import argparse


def get_pipe_identifier_type() -> str:
    if sys.platform == "win32":
        return "name"

    return "path"


def get_pipe_identifier_in_ctx() -> str:
    parser = argparse.ArgumentParser(allow_abbrev=False, add_help=False)
    parser.add_argument("-pipe_identifier")
    args = parser.parse_args()
    identifier_type: str = get_pipe_identifier_type()
    if args.pipe_identifier is None:
        return input(f"Pipe {identifier_type}: ")

    pipe_name: str = args.pipe_identifier
    print(f"Pipe {identifier_type}: {pipe_name}")
    return pipe_name
