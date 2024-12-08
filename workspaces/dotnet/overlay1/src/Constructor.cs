using System;

namespace OMP.LSWTSS;

partial class Overlay1
{
    public Overlay1(int order)
    {
        Order = order;
        IsVisible = true;

        ChromiumWebBrowser = new CefSharp.OffScreen.ChromiumWebBrowser(
            automaticallyCreateBrowser: false
        )
        {
            RenderHandler = new CefOffScreenRenderHandler(this)
        };

        var cefWindowInfo = CefSharp.Core.ObjectFactory.CreateWindowInfo();

        // TODO: Rework & enable it once CefSharp 124 is released (https://chromium-review.googlesource.com/c/chromium/src/+/5265077)
        // windowInfo.SharedTextureEnabled = true;
        cefWindowInfo.SetAsWindowless(IntPtr.Zero);

        ChromiumWebBrowser.CreateBrowser(cefWindowInfo, null);

        AreKeyboardEventsEnabled = true;
        _areKeyboardEventsIntercepted = true;
        AreMouseEventsEnabled = true;
        _areMouseEventsIntercepted = true;

        _inputHookClient = new InputHook1.Client(
            order + 1000, // TODO: Expose 1000 as const InputHook1ClientBaseOrder
            (
                inputHookClientNativeMessage
            ) =>
            {
                if (IsActive)
                {
                    if (
                        AreKeyboardEventsEnabled
                        &&
                        (PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type >= PInvoke.User32.WindowMessage.WM_KEYFIRST
                        &&
                        (PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type <= PInvoke.User32.WindowMessage.WM_KEYLAST
                    )
                    {
                        CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                        CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendKeyEvent(
                            inputHookClientNativeMessage.Type,
                            (int)inputHookClientNativeMessage.WParam,
                            (int)inputHookClientNativeMessage.LParam
                        );

                        return true;
                    }
                    else if (
                        AreMouseEventsEnabled
                        &&
                        (PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type >= PInvoke.User32.WindowMessage.WM_MOUSEFIRST
                        &&
                        (PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type <= PInvoke.User32.WindowMessage.WM_MOUSELAST
                    )
                    {
                        var cefMouseEventFlags = CefSharp.CefEventFlags.None;

                        var vkState = WinMacros.GET_KEYSTATE_PARAM((uint)inputHookClientNativeMessage.WParam);

                        if ((vkState & 0x0001) != 0) // MK_LBUTTON
                        {
                            cefMouseEventFlags |= CefSharp.CefEventFlags.LeftMouseButton;
                        }
                        if ((vkState & 0x0002) != 0) // MK_RBUTTON
                        {
                            cefMouseEventFlags |= CefSharp.CefEventFlags.RightMouseButton;
                        }
                        if ((vkState & 0x0004) != 0) // MK_SHIFT
                        {
                            cefMouseEventFlags |= CefSharp.CefEventFlags.ShiftDown;
                        }
                        if ((vkState & 0x0008) != 0) // MK_CONTROL
                        {
                            cefMouseEventFlags |= CefSharp.CefEventFlags.ControlDown;
                        }
                        if ((vkState & 0x0010) != 0) // MK_MBUTTON
                        {
                            cefMouseEventFlags |= CefSharp.CefEventFlags.MiddleMouseButton;
                        }
                        if ((vkState & 0x0020) != 0) // MK_ALT
                        {
                            cefMouseEventFlags |= CefSharp.CefEventFlags.AltDown;
                        }

                        if (
                            (PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MOUSEWHEEL
                            ||
                            (PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MOUSEHWHEEL
                        )
                        {
                            var mouseScreenX = (short)inputHookClientNativeMessage.LParam;
                            var mouseScreenY = (short)(inputHookClientNativeMessage.LParam >> 16);

                            var mouseClientPos = new PInvoke.POINT
                            {
                                x = mouseScreenX,
                                y = mouseScreenY
                            };

                            PInvoke.User32.ScreenToClient(inputHookClientNativeMessage.WindowHandle, ref mouseClientPos);

                            var cefMouseEvent = new CefSharp.MouseEvent(mouseClientPos.x, mouseClientPos.y, cefMouseEventFlags);

                            if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MOUSEWHEEL)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseWheelEvent(
                                    cefMouseEvent,
                                    0,
                                    unchecked((short)WinMacros.GET_WHEEL_DELTA_WPARAM((uint)inputHookClientNativeMessage.WParam))
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MOUSEHWHEEL)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseWheelEvent(
                                    cefMouseEvent,
                                    unchecked((short)WinMacros.GET_WHEEL_DELTA_WPARAM((uint)inputHookClientNativeMessage.WParam)),
                                    0
                                );

                                return true;
                            }
                        }
                        else
                        {
                            var mouseClientX = (short)inputHookClientNativeMessage.LParam;
                            var mouseClientY = (short)(inputHookClientNativeMessage.LParam >> 16);

                            var cefMouseEvent = new CefSharp.MouseEvent(mouseClientX, mouseClientY, cefMouseEventFlags);

                            if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MOUSEMOVE)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseMoveEvent(
                                    cefMouseEvent,
                                    false
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_LBUTTONDOWN)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Left,
                                    false,
                                    1
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_LBUTTONUP)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Left,
                                    true,
                                    1
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_LBUTTONDBLCLK)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Left,
                                    false,
                                    2
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_RBUTTONDOWN)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Right,
                                    false,
                                    1
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_RBUTTONUP)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Right,
                                    true,
                                    1
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_RBUTTONDBLCLK)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Right,
                                    false,
                                    2
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MBUTTONDOWN)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Middle,
                                    false,
                                    1
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MBUTTONUP)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Middle,
                                    true,
                                    1
                                );

                                return true;
                            }
                            else if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_MBUTTONDBLCLK)
                            {
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SetFocus(true);
                                CefSharp.WebBrowserExtensions.GetBrowserHost(ChromiumWebBrowser).SendMouseClickEvent(
                                    cefMouseEvent,
                                    CefSharp.MouseButtonType.Middle,
                                    false,
                                    2
                                );

                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        );

        _instances.Add(this);
        SortInstances();
    }
}