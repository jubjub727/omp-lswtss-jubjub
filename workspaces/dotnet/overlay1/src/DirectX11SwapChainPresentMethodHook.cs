namespace OMP.LSWTSS;

partial class Overlay1
{
    readonly static CFuncHook1<DirectX11Bindings.SwapChainPresentMethodDelegate> _directX11SwapChainPresentMethodHook = new(
        DirectX11Bindings.SwapChainPresentMethodPtr,
        (
            swapChainHandle,
            syncInterval,
            flags
        ) =>
        {
            foreach (var instance in _instances!)
            {
                if (
                    instance._directX11OverlayQuad == null
                    ||
                    instance._directX11OverlayQuad.SwapChain.NativePointer != swapChainHandle
                )
                {
                    instance._directX11OverlayQuad?.Dispose();

                    instance._directX11OverlayQuad = new DirectX11OverlayQuad(
                        new SharpDX.DXGI.SwapChain(swapChainHandle)
                    );

                    CefSharp.WebBrowserExtensions.GetBrowserHost(instance.ChromiumWebBrowser).WindowlessFrameRate = 60;

                    instance.ChromiumWebBrowser.Size = new System.Drawing.Size(
                        instance._directX11OverlayQuad.TextureWidth,
                        instance._directX11OverlayQuad.TextureHeight
                    );
                }

                if (instance.IsVisible)
                {
                    instance._directX11OverlayQuad.Draw();
                }
            }

            return _directX11SwapChainPresentMethodHook!.Trampoline!(swapChainHandle, syncInterval, flags);
        }
    );
}