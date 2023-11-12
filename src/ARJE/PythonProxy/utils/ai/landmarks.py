import typing


class Vector3(typing.NamedTuple):
    x: float
    y: float
    z: float


LandmarkConnection = tuple[int, int]
LandmarkConnectionSet = frozenset[LandmarkConnection]
LandmarksContainer = typing.Collection[Vector3]


class LandmarkCollection(typing.NamedTuple):
    positions: LandmarksContainer
    connections: LandmarkConnectionSet
