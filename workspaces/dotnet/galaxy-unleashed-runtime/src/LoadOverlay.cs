using System;

namespace OMP.LSWTSS;

public partial class GalaxyUnleashed : IDisposable
{
    void LoadOverlay()
    {
        _overlay.ChromiumWebBrowser.JavascriptObjectRepository.NameConverter =
            new CefSharp.JavascriptBinding.CamelCaseJavascriptNameConverter();

        _overlay.ChromiumWebBrowser.JavascriptObjectRepository.Register(
            "galaxyUnleashedRuntimeApi",
            new RuntimeApi(),
            options: CefSharp.BindingOptions.DefaultBinder
        );

        string overlayUrl;

        if (Environment.GetEnvironmentVariable("DEV_GALAXY_UNLEASHED") == "1")
        {
            overlayUrl = "http://localhost:5173/";
        }
        else
        {
            string overlayIndexHtmlFilePath = System.IO.Path.Combine(
                System.IO.Path.Combine(Environment.CurrentDirectory, "mods", "galaxy-unleashed"),
                "index.html"
            ).Replace("\\", "/");

            overlayUrl = "file://" + overlayIndexHtmlFilePath;
        }

        _overlay.ChromiumWebBrowser.LoadUrl(overlayUrl);
    }
}