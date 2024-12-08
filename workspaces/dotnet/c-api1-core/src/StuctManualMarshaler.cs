using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public class StructManualMarshaler<T> : IDisposable where T : struct
{
    public readonly nint NativeValuePtr;

    public StructManualMarshaler(T value)
    {
        NativeValuePtr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
        Marshal.StructureToPtr(value, NativeValuePtr, false);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(NativeValuePtr);
    }
}