import typing
import numpy
import numpy.typing
import cv2 as opencv
from detection_model import DetectionModel
import mediapipe.python.solutions.holistic as HolisticConnections
from mediapipe.python.solutions.holistic import Holistic as HolisticSolution
from detection import Detection
from landmarks import LandmarksContainer, LandmarkCollection, LandmarkConnectionSet


class _HolisticResult(typing.NamedTuple):
    pose_landmarks: LandmarksContainer | None
    right_hand_landmarks: LandmarksContainer | None
    left_hand_landmarks: LandmarksContainer | None
    face_landmarks: LandmarksContainer | None


class HolisticModel(DetectionModel):
    H_HAND_CONNECTIONS: LandmarkConnectionSet = getattr(HolisticConnections, "HAND_CONNECTIONS")
    H_FACEMESH_TESSELATION: LandmarkConnectionSet = getattr(HolisticConnections, "FACEMESH_TESSELATION")

    def __init__(self):
        self._solution: typing.Final = HolisticSolution()

    def process(self, image: numpy.typing.NDArray[numpy.uint8]) -> list[Detection]:
        image = opencv.cvtColor(image, opencv.COLOR_BGR2RGB)
        detections = list[Detection]()
        solutionResults: _HolisticResult = self._solution.process(image)  # type: ignore
        HolisticModel._try_add_detection(detections, "Pose", solutionResults.pose_landmarks, None)
        HolisticModel._try_add_detection(detections, "RightHand", solutionResults.right_hand_landmarks, self.H_HAND_CONNECTIONS)
        HolisticModel._try_add_detection(detections, "LeftHand", solutionResults.left_hand_landmarks, self.H_HAND_CONNECTIONS)
        HolisticModel._try_add_detection(detections, "Face", solutionResults.face_landmarks, None)
        image = opencv.cvtColor(image, opencv.COLOR_RGB2BGR)
        return detections

    @staticmethod
    def _try_add_detection(
        detections: list[Detection],
        name: str,
        solutionResult: LandmarksContainer | None,
        connections: LandmarkConnectionSet
    ):

        if (solutionResult is None):
            return

        landmarks = LandmarkCollection(solutionResult.landmark, connections)
        detection = Detection(name, landmarks)
        detections.append(detection)

    def __enter__(self):
        return self

    def __exit__(self, *_):
        self._solution.close()
