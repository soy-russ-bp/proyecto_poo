from utils.ai.detection import Detection
from utils.ai.solutions.holistic_model import HolisticModel
from utils.csharp.csharp_proxy import CSharpProxy
from utils.csharp.packets.inbound.inbound_matrix_packet import InboundMatrixPacket
from utils.csharp.packets.inbound.inbound_matrix_packet import ObjectT as Matrix
from utils.csharp.packets.outbound.outbound_detection_collection_packet import OutboundDetectionCollectionPacket


print(" - App start - ")


with CSharpProxy("SignTrainer").start() as proxy:
    with HolisticModel() as model:
        print(" - Proxy start - ")
        while True:
            image: Matrix = proxy.receive_object(InboundMatrixPacket())
            detections: list[Detection] = model.process(image)
            proxy.send_object(OutboundDetectionCollectionPacket(detections))
