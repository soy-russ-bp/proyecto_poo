using System;

namespace ARJE.Utils.IO.Pipes
{
    public static class PlatformNamedPipe
    {
        public static INamedPipe Create(string pipeName)
        {
            if (OperatingSystem.IsWindows())
            {
                return Win32NamedPipe.Create(pipeName);
            }
            else if (OperatingSystem.IsMacOS())
            {
                return PosixNamedPipe.Create(pipeName);
            }

            throw new PlatformNotSupportedException();
        }
    }
}
