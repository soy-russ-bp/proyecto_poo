import typing
from landmarks import LandmarkCollection


class Detection(typing.NamedTuple):
    name: str
    landmarks: LandmarkCollection
