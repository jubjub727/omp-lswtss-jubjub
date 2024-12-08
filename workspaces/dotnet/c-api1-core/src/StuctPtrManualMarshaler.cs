using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public class StructPtrManualMarshaler<T> : IDisposable where T : struct
{
    private unsafe T* valuePtr;

    public nint NativeValuePtr;

    public unsafe StructPtrManualMarshaler(T* valuePtr)
    {
        this.valuePtr = valuePtr;
        NativeValuePtr = Marshal.AllocHGlobal(Marshal.SizeOf(*valuePtr));
        Marshal.StructureToPtr(*valuePtr, NativeValuePtr, false);
    }

    public unsafe void Dispose()
    {
        Marshal.PtrToStructure(NativeValuePtr, *valuePtr);
        Marshal.FreeHGlobal(NativeValuePtr);
    }
}