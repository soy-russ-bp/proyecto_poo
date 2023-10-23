import numpy
import numpy.typing
from abc import ABC, abstractmethod
from utils.ai.detection import Detection


class PredictionModel(ABC):

    @abstractmethod
    def process(self, image: numpy.typing.NDArray[numpy.uint8]) -> list[Detection]:
        pass
