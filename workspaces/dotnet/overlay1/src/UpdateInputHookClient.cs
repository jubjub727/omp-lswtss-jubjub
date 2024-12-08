namespace OMP.LSWTSS;

partial class Overlay1
{
    readonly nint _cursorArrowImageHandle = PInvoke.User32.LoadCursor(nint.Zero, (nint)PInvoke.User32.Cursors.IDC_ARROW).DangerousGetHandle();

    void UpdateInputHookClient()
    {
        _inputHookClient.AreKeyboardEventsIntercepted = IsActive && _areKeyboardEventsIntercepted;
        _inputHookClient.AreMouseEventsIntercepted = IsActive && _areMouseEventsIntercepted;
        _inputHookClient.CursorOverrideImageHandle = IsActive && AreMouseEventsEnabled ? _cursorArrowImageHandle : null;
    }
}