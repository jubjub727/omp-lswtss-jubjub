namespace OMP.LSWTSS;

partial class InputHook1
{
    readonly static CFuncHook1<User32Bindings.SetCursorDelegate> _user32SetCursorHook = new(
        User32Bindings.SetCursorPtr,
        (
            hCursor
        ) =>
        {
            if (CursorOverrideImageHandle != null)
            {
                return 0;
            }

            return _user32SetCursorHook!.Trampoline!(hCursor);
        }
    );
}