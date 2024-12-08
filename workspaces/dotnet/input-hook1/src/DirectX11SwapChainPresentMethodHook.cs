namespace OMP.LSWTSS;

partial class InputHook1
{
    readonly static CFuncHook1<DirectX11Bindings.SwapChainPresentMethodDelegate> _directX11SwapChainPresentMethodHook = new(
        DirectX11Bindings.SwapChainPresentMethodPtr,
        (
            swapChainHandle,
            syncInterval,
            flags
        ) =>
        {
            using var swapChain = new SharpDX.DXGI.SwapChain(swapChainHandle);

            var windowHandle = swapChain.Description.OutputHandle;

            if (_windowHook == null || _windowHook.WindowHandle != windowHandle)
            {
                _windowHook?.Dispose();

                _windowHook = new WindowHook(windowHandle);
            }

            return _directX11SwapChainPresentMethodHook!.Trampoline!(swapChainHandle, syncInterval, flags);
        }
    );
}