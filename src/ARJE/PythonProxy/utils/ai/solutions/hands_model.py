import typing
import cv2 as opencv
from mediapipe.python.solutions.hands import Hands as HandsSolution
from utils.ai.detection_model import DetectionModel
from utils.ai.detection import Detection
from utils.ai.landmarks import LandmarksContainer, LandmarkCollection
from utils.csharp.packets.inbound.inbound_matrix_packet import ObjectT as Matrix


class _HandsResults:
    landmark: LandmarksContainer


class _HandsResultsContainer(typing.NamedTuple):
    multi_hand_landmarks: typing.Collection[_HandsResults] | None


class HandsModel(DetectionModel):

    def __init__(self):
        self._solution: typing.Final = HandsSolution()

    def process(self, image: Matrix) -> list[Detection]:
        image = opencv.cvtColor(image, opencv.COLOR_BGR2RGB)  # type: ignore
        detections = list[Detection]()
        results_container: _HandsResultsContainer = self._solution.process(image)  # type: ignore
        if results_container.multi_hand_landmarks is None:
            return detections

        for hand in results_container.multi_hand_landmarks:
            HandsModel._add_detection(detections, hand.landmark)

        image = opencv.cvtColor(image, opencv.COLOR_RGB2BGR)  # type: ignore
        return detections

    @staticmethod
    def _add_detection(
        detections: list[Detection],
        solution_result: LandmarksContainer
    ):
        landmarks = LandmarkCollection(solution_result, None)
        detection = Detection(landmarks)
        detections.append(detection)

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self._solution.close()
