namespace OMP.LSWTSS;

partial class Overlay1
{
    readonly static CFuncHook1<DirectX11Bindings.SwapChainResizeBuffersMethodDelegate> _directX11SwapChainResizeBuffersMethodHook = new(
        DirectX11Bindings.SwapChainResizeBuffersMethodPtr,
        (
            swapChainHandle,
            bufferCount,
            width,
            height,
            newFormat,
            swapChainFlags
        ) =>
        {
            foreach (var instance in _instances!)
            {
                instance._directX11OverlayQuad?.Dispose();
                instance._directX11OverlayQuad = null;
            }

            return _directX11SwapChainResizeBuffersMethodHook!.Trampoline!(
                swapChainHandle,
                bufferCount,
                width,
                height,
                newFormat,
                swapChainFlags
            );
        }
    );
}