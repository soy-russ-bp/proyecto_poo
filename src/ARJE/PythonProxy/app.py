from utils.ai.solutions.holistic_model import HolisticModel
from utils.csharp.csharp_proxy import CSharpProxy
from utils.csharp.packets.inbound.inbound_matrix_packet import InboundMatrixPacket
from utils.csharp.packets.inbound.inbound_matrix_packet import TObject as Matrix
from utils.csharp.packets.outbound.outbound_detection_collection_packet import OutboundDetectionCollectionPacket


print(" - App start - ")


with HolisticModel() as model:
    with CSharpProxy("SignTrainer").start() as proxy:
        print(" - Proxy start - ")
        while True:
            cam_image: Matrix = proxy.receive_object(InboundMatrixPacket())
            results = model.process(cam_image)
            proxy.send_object(OutboundDetectionCollectionPacket(results))
