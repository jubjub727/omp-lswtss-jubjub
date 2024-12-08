using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public class OwnedStringManualMarshaler : IDisposable
{
    public readonly nint NativeValue;

    public OwnedStringManualMarshaler(string value)
    {
        NativeValue = Marshal.StringToCoTaskMemUTF8(value);
    }

    public void Dispose()
    {
        Marshal.FreeCoTaskMem(NativeValue);
    }
}