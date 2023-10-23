import typing


ByteOrderLiterals = typing.Literal["little", "big"]


def get_float_format(byte_order: ByteOrderLiterals) -> str:
    return ("<" if byte_order == "little" else ">") + "f"
