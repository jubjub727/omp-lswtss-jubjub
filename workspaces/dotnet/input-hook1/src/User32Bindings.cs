namespace OMP.LSWTSS;

partial class InputHook1
{
    static class User32Bindings
    {
        public delegate int ShowCursorDelegate(bool bShow);

        public delegate int SetCursorDelegate(nint hCursor);

        public static readonly nint ShowCursorPtr;

        public static readonly nint SetCursorPtr;

        static User32Bindings()
        {
            using var user32ModuleHandle = PInvoke.Kernel32.LoadLibrary("user32.dll");

            ShowCursorPtr = PInvoke.Kernel32.GetProcAddress(user32ModuleHandle, "ShowCursor");

            SetCursorPtr = PInvoke.Kernel32.GetProcAddress(user32ModuleHandle, "SetCursor");
        }
    }
}