using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using ARJE.Utils.Python;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;
using ARJE.Utils.Python.Proxy;
using ARJE.Utils.Python.Proxy.Packets.Inbound;
using ARJE.Utils.Python.Proxy.Packets.Outbound;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.AI.Solutions.Hands
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public sealed class HandsModel : IDetectionModel<HandsDetectionResult, HandDetection, Matrix>
    {
        private HandsModel(PythonAppInfo<VenvInfo>? appInfo)
        {
            if (appInfo.HasValue)
            {
                var launcher = new PythonLauncher<VenvInfo>(appInfo.Value);
                launcher.Run("Python app");
            }

            this.Proxy = new PythonProxy("SignTrainer", new CustomIdMapper()).Start();
        }

        private PythonProxy Proxy { get; }

        public static HandsModel Start(PythonAppInfo<VenvInfo> appInfo) => new(appInfo);

        public static HandsModel StartNoLaunch() => new(null);

        public HandsDetectionResult Process(Matrix image)
        {
            this.Proxy.Send<OutboundMatrixPacket>(image);
            return this.Proxy.Receive<HandsDetectionResult, InboundDetectionCollectionPacket>();
        }

        private class CustomIdMapper : IIdMapper
        {
            public IReadOnlySet<Type> AllowedTypes => throw new NotImplementedException();

            public int GetTypeId(Type type)
            {
                return type.Name switch
                {
                    "Mat" => 201,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
        }
    }
}
