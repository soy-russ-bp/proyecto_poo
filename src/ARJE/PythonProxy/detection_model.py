import numpy
import numpy.typing
from abc import ABC, abstractmethod
from detection import Detection


class DetectionModel(ABC):

    @abstractmethod
    def process(self, image: numpy.typing.NDArray[numpy.uint8]) -> list[Detection]:
        pass
