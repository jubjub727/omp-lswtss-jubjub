using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public partial class TestCefMod
{
    [StructLayout(LayoutKind.Sequential)]
    struct NativeVector3
    {
        public float X;
        public float Y;
        public float Z;
    }
}