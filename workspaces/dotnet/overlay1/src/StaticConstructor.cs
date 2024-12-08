using System;
using System.IO;

namespace OMP.LSWTSS;

partial class Overlay1
{
    static Overlay1()
    {
        var cefOffScreenSettings = new CefSharp.OffScreen.CefSettings
        {
            CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "omp-lswtss-overlay1\\Cache")
        };

        // This line improves the performance
        cefOffScreenSettings.CefCommandLineArgs.Add("disable-threaded-scrolling", "1");

        // TODO: Perf check this
        // cefSettings.SetOffScreenRenderingBestPerformanceArgs();

        CefSharp.Cef.Initialize(
            cefOffScreenSettings,
            performDependencyCheck: true,
            browserProcessHandler: null
        );

        _directX11SwapChainPresentMethodHook.Enable();
        _directX11SwapChainResizeBuffersMethodHook.Enable();
    }
}