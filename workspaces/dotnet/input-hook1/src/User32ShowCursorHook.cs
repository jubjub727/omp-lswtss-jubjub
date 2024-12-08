namespace OMP.LSWTSS;

partial class InputHook1
{
    readonly static CFuncHook1<User32Bindings.ShowCursorDelegate> _user32ShowCursorHook = new(
        User32Bindings.ShowCursorPtr,
        (
            bShow
        ) =>
        {
            if (IsCursorVisible)
            {
                return bShow ? 1 : 0;
            }

            return _user32ShowCursorHook!.Trampoline!(bShow);
        }
    );
}