using System.Reflection;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public class DebugTools
{
    readonly Assembly _runtimeEngineAssembly;

    readonly CFuncHook1<GameFramework.ProcessMethod.NativeDelegate> _gameFrameworkProcessMethodHook;

    volatile bool _isRefreshShortcutTriggered = false;

    bool _isRefreshShortcutShiftKeyPressed = false;

    bool _isRefreshShortcutRKeyPressed = false;

    public DebugTools()
    {
        _runtimeEngineAssembly = Assembly.Load("omp-lswtss-runtime-engine");

        _ = new InputHook1.Client(
            int.MaxValue,
            (
                in InputHook1.NativeMessage inputHookClientNativeMessage
            ) =>
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYDOWN)
                {
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_SHIFT)
                    {
                        _isRefreshShortcutShiftKeyPressed = true;
                    }
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_R)
                    {
                        _isRefreshShortcutRKeyPressed = true;
                    }

                    if (_isRefreshShortcutShiftKeyPressed && _isRefreshShortcutRKeyPressed)
                    {
                        _isRefreshShortcutTriggered = true;

                        _isRefreshShortcutShiftKeyPressed = false;
                        _isRefreshShortcutRKeyPressed = false;
                    }
                }
                else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_SHIFT)
                    {
                        _isRefreshShortcutShiftKeyPressed = false;
                    }
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_R)
                    {
                        _isRefreshShortcutRKeyPressed = false;
                    }
                }

                return false;
            }
        );

        _gameFrameworkProcessMethodHook = new(
            GameFramework.ProcessMethod.Info.NativePtr,
            (nativeRawDataPtr) =>
            {
                if (_isRefreshShortcutTriggered)
                {
                    _runtimeEngineAssembly.GetType("OMP.LSWTSS.RuntimeEngine")!.GetMethod("UnloadUnloadableScriptingModules")!.Invoke(null, null);
                    _runtimeEngineAssembly.GetType("OMP.LSWTSS.RuntimeEngine")!.GetMethod("LoadUnloadedScriptingModules")!.Invoke(null, null);

                    _isRefreshShortcutTriggered = false;
                }

                return _gameFrameworkProcessMethodHook!.Trampoline!(nativeRawDataPtr);
            }
        );

        _gameFrameworkProcessMethodHook.Enable();
    }
}