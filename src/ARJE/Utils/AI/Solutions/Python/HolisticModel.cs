using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using ARJE.Utils.AI.Solutions.Python.ProxyPackets;
using ARJE.Utils.Python;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;
using ARJE.Utils.Python.Proxy;
using ARJE.Utils.Python.Proxy.Packets.Outbound;
using Matrix = Emgu.CV.Mat;

namespace ARJE.Utils.AI.Solutions.Python
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public sealed class HolisticModel : IPredictionModel<Matrix>
    {
        private HolisticModel(PythonAppInfo<VenvInfo>? appInfo)
        {
            if (appInfo.HasValue)
            {
                var launcher = new PythonLauncher<VenvInfo>(appInfo.Value);
                launcher.Run("Python app");
            }

            this.Proxy = new PythonProxy("SignTrainer", new CustomIdMapper()).Start();
        }

        public PythonProxy Proxy { get; }

        public static HolisticModel Start(PythonAppInfo<VenvInfo> appInfo) => new(appInfo);

        public static HolisticModel StartNoLaunch() => new(null);

        public ReadOnlyCollection<Detection> Process(Matrix image)
        {
            this.Proxy.Send<OutboundMatrixPacket, Matrix>(image);
            return this.Proxy.Receive<DetectionCollectionPacket, ReadOnlyCollection<Detection>>();
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
