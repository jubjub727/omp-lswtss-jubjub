namespace OMP.LSWTSS;

partial class Overlay1
{
    readonly static CFuncHook1<DirectX11Bindings.SwapChainPresentMethodNativeDelegate> _directX11SwapChainPresentMethodHook = new(
        DirectX11Bindings.SwapChainPresentMethodNativePtr,
        (
            swapChainNativeHandle,
            syncInterval,
            flags
        ) =>
        {
            foreach (var instance in _instances!)
            {
                if (
                    instance._directX11OverlayQuad == null
                    ||
                    instance._directX11OverlayQuad.SwapChain.NativePointer != swapChainNativeHandle
                )
                {
                    instance._directX11OverlayQuad?.Dispose();

                    instance._directX11OverlayQuad = new DirectX11OverlayQuad(
                        new SharpDX.DXGI.SwapChain(swapChainNativeHandle)
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

            return _directX11SwapChainPresentMethodHook!.Trampoline!(swapChainNativeHandle, syncInterval, flags);
        }
    );
}