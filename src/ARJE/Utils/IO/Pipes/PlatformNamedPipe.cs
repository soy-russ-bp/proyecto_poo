using System.Runtime.Versioning;

namespace ARJE.Utils.IO.Pipes
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public static class PlatformNamedPipe
    {
        public static INamedPipe Create(string pipeName)
        {
#if OS_WINDOWS
            return Win32NamedPipe.Create(pipeName);
#elif OS_MAC
            return PosixNamedPipe.Create(pipeName);
#else
#warning PlatformNotSupported: PlatformNamedPipe.Create(string)
            throw new System.PlatformNotSupportedException();
#endif
        }
    }
}
