using System.Reflection;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public class DebugTools
{
    readonly Assembly _runtimeEngineAssembly;
    readonly CFuncHook1<GameFramework.UpdateMethod.Delegate> _gameFrameworkUpdateMethodHook;

    volatile bool _isRefreshShortcutTriggered = false;

    bool _isRefreshShortcutShiftKeyTriggered = false;

    bool _isRefreshShortcutRKeyTriggered = false;

    public DebugTools()
    {
        _runtimeEngineAssembly = Assembly.Load("omp-lswtss-runtime-engine");

        _ = new InputHook1.Client(
            int.MaxValue,
            (
                inputHookClientNativeMessage
            ) =>
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYDOWN)
                {
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_SHIFT)
                    {
                        _isRefreshShortcutShiftKeyTriggered = true;
                    }
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_R)
                    {
                        _isRefreshShortcutRKeyTriggered = true;
                    }

                    if (_isRefreshShortcutShiftKeyTriggered && _isRefreshShortcutRKeyTriggered)
                    {
                        _isRefreshShortcutTriggered = true;

                        _isRefreshShortcutShiftKeyTriggered = false;
                        _isRefreshShortcutRKeyTriggered = false;
                    }
                }
                else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_SHIFT)
                    {
                        _isRefreshShortcutShiftKeyTriggered = false;
                    }
                    if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_R)
                    {
                        _isRefreshShortcutRKeyTriggered = false;
                    }
                }

                return false;
            }
        );

        _gameFrameworkUpdateMethodHook = new(
            GameFramework.UpdateMethod.Ptr,
            (handle) =>
            {
                if (_isRefreshShortcutTriggered)
                {
                    _runtimeEngineAssembly.GetType("OMP.LSWTSS.RuntimeEngine")!.GetMethod("UnloadUnloadableScriptingModules")!.Invoke(null, null);
                    _runtimeEngineAssembly.GetType("OMP.LSWTSS.RuntimeEngine")!.GetMethod("LoadUnloadedScriptingModules")!.Invoke(null, null);

                    _isRefreshShortcutTriggered = false;
                }

                return _gameFrameworkUpdateMethodHook!.Trampoline!(handle);
            }
        );

        _gameFrameworkUpdateMethodHook.Enable();
    }
}