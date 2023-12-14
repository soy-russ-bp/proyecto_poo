using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace ARJE.SignPractice.SignPractice
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}