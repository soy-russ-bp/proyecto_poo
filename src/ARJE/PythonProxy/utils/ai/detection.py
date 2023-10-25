import typing
from utils.ai.landmarks import LandmarkCollection


class Detection(typing.NamedTuple):
    landmarks: LandmarkCollection
