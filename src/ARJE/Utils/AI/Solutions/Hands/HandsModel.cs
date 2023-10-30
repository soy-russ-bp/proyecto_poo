using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using ARJE.Utils.Python;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;
using ARJE.Utils.Python.Proxy;
using ARJE.Utils.Python.Proxy.Packets.Inbound;
using ARJE.Utils.Python.Proxy.Packets.Outbound;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Utils.AI.Solutions.Hands
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public sealed class HandsModel : IDetectionModel<HandDetectionCollection, HandDetection, Matrix>, IDisposable
    {
        public HandsModel()
        {
            this.Proxy = new PythonProxy("SignTrainer", new CustomIdMapper());
        }

        public string PipeName => this.Proxy.PipeName;

        private PythonProxy Proxy { get; }

        public Task StartAsync(PythonAppInfo<VenvInfo> appInfo)
        {
            return this.InternalStartAsync(appInfo);
        }

        public Task StartNoLaunchAsync()
        {
            return this.InternalStartAsync(null);
        }

        public HandDetectionCollection Process(Matrix image)
        {
            this.Proxy.Send<OutboundMatrixPacket>(image);
            return this.Proxy.Receive<HandDetectionCollection, InboundHandDetectionCollectionPacket>();
        }

        public void Dispose()
        {
            this.Proxy.Dispose();
        }

        private Task InternalStartAsync(PythonAppInfo<VenvInfo>? appInfo)
        {
            if (appInfo.HasValue)
            {
                var launcher = new PythonLauncher<VenvInfo>(appInfo.Value);
                launcher.Run("Python app");
            }

            return this.Proxy.StartAsync();
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
