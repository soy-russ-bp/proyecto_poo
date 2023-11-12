from abc import ABC, abstractmethod
import numpy
import numpy.typing
from utils.ai.detection import Detection


class DetectionModel(ABC):

    @abstractmethod
    def process(self, image: numpy.typing.NDArray[numpy.uint8]) -> list[Detection]:
        pass
