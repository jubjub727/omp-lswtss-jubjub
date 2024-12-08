namespace OMP.LSWTSS;

partial class InputHook1
{
    public struct NativeMessage
    {
        public required nint WindowHandle;

        public required int Type;

        public required nint WParam;

        public required nint LParam;

        public required int MouseScreenX;

        public required int MouseScreenY;
    }
}