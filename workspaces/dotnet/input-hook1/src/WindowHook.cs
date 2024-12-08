using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

partial class InputHook1
{
    static WindowHook? _windowHook;

    sealed class WindowHook : IDisposable
    {
        public readonly nint WindowHandle;

        readonly PInvoke.User32.SafeHookHandle _windowsHookExHandle;

        readonly GCHandle _windowsHookExProcGcHandle;

        bool _isDisposed;

        public WindowHook(nint windowHandle)
        {
            WindowHandle = windowHandle;

            var windowThreadId = PInvoke.User32.GetWindowThreadProcessId(windowHandle, out _);

            PInvoke.User32.WindowsHookDelegate windowsHookExProc = (nCode, wParam, lParam) =>
            {
                unsafe
                {
                    if (nCode >= 0 && wParam == (nint)PInvoke.User32.PeekMessageRemoveFlags.PM_REMOVE)
                    {
                        var msg = (PInvoke.User32.MSG*)lParam;

                        if (IsCursorVisible)
                        {
                            var cursorInfo = new PInvoke.User32.CURSORINFO();
                            cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);

                            if (PInvoke.User32.GetCursorInfo(&cursorInfo))
                            {
                                if (cursorInfo.flags == PInvoke.User32.CURSORINFOFlags.CURSOR_HIDDEN)
                                {
                                    _user32ShowCursorHook.Trampoline!(true);
                                }
                            }
                        }

                        var cursorOverrideImageHandle = CursorOverrideImageHandle;

                        if (cursorOverrideImageHandle != null)
                        {
                            _user32SetCursorHook.Trampoline!((nint)cursorOverrideImageHandle);
                        }

                        if (
                            msg->message == PInvoke.User32.WindowMessage.WM_INPUT
                        )
                        {
                            if (AreKeyboardEventsIntercepted)
                            {
                                PInvoke.User32.TranslateMessage(msg);
                                msg->message = PInvoke.User32.WindowMessage.WM_NULL;
                                return 0;
                            }
                        }
                        else if (
                            msg->message >= PInvoke.User32.WindowMessage.WM_KEYFIRST
                            &&
                            msg->message <= PInvoke.User32.WindowMessage.WM_KEYLAST
                        )
                        {
                            var nativeMessage = new NativeMessage()
                            {
                                WindowHandle = windowHandle,
                                Type = (int)msg->message,
                                WParam = msg->wParam,
                                LParam = msg->lParam,
                                MouseScreenX = msg->pt.x,
                                MouseScreenY = msg->pt.y,
                            };

                            lock (_clients)
                            {
                                foreach (var client in _clients)
                                {
                                    if (client.HandleNativeMessage(nativeMessage))
                                    {
                                        break;
                                    }
                                }
                            }

                            if (AreKeyboardEventsIntercepted)
                            {
                                PInvoke.User32.TranslateMessage(msg);
                                msg->message = PInvoke.User32.WindowMessage.WM_NULL;
                                return 0;
                            }
                        }
                        else if (
                            msg->message >= PInvoke.User32.WindowMessage.WM_MOUSEFIRST
                            &&
                            msg->message <= PInvoke.User32.WindowMessage.WM_MOUSELAST
                        )
                        {
                            if (
                                msg->message == PInvoke.User32.WindowMessage.WM_MOUSEWHEEL
                                ||
                                msg->message == PInvoke.User32.WindowMessage.WM_MOUSEHWHEEL
                            )
                            {
                                var nativeMessage = new NativeMessage
                                {
                                    WindowHandle = windowHandle,
                                    Type = (int)msg->message,
                                    WParam = msg->wParam,
                                    LParam = msg->lParam,
                                    MouseScreenX = msg->pt.x,
                                    MouseScreenY = msg->pt.y,
                                };

                                lock (_clients)
                                {
                                    foreach (var client in _clients)
                                    {
                                        if (client.HandleNativeMessage(nativeMessage))
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (AreMouseEventsIntercepted)
                                {
                                    PInvoke.User32.TranslateMessage(msg);
                                    msg->message = PInvoke.User32.WindowMessage.WM_NULL;
                                    return 0;
                                }
                            }
                            else
                            {
                                var x = (short)msg->lParam;
                                var y = (short)(msg->lParam >> 16);

                                PInvoke.User32.GetClientRect(windowHandle, out var clientRect);

                                if (x >= 0 && x <= clientRect.right - clientRect.left && y >= 0 && y <= clientRect.bottom - clientRect.top)
                                {
                                    var nativeMessage = new NativeMessage()
                                    {
                                        WindowHandle = windowHandle,
                                        Type = (int)msg->message,
                                        WParam = msg->wParam,
                                        LParam = msg->lParam,
                                        MouseScreenX = msg->pt.x,
                                        MouseScreenY = msg->pt.y,
                                    };

                                    lock (_clients)
                                    {
                                        foreach (var client in _clients)
                                        {
                                            if (client.HandleNativeMessage(nativeMessage))
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    if (AreMouseEventsIntercepted)
                                    {
                                        PInvoke.User32.TranslateMessage(msg);
                                        msg->message = PInvoke.User32.WindowMessage.WM_NULL;
                                        return 0;
                                    }
                                }
                            }
                        }
                    }
                };

                return PInvoke.User32.CallNextHookEx(nint.Zero, nCode, wParam, lParam);
            };

            _windowsHookExProcGcHandle = GCHandle.Alloc(windowsHookExProc);

            _windowsHookExHandle = PInvoke.User32.SetWindowsHookEx(
                PInvoke.User32.WindowsHookType.WH_GETMESSAGE,
                windowsHookExProc,
                nint.Zero,
                windowThreadId
            );
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _windowsHookExHandle.Dispose();
                _windowsHookExProcGcHandle.Free();

                _isDisposed = true;
            }
        }
    }
}