using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public class NotOwnedStringMarshaler : ICustomMarshaler
{
    static NotOwnedStringMarshaler? instance;

    public static ICustomMarshaler GetInstance(string cookie)
    {
        instance ??= new NotOwnedStringMarshaler();

        return instance;
    }

    public void CleanUpManagedData(object ManagedObj)
    {
    }

    public void CleanUpNativeData(nint pNativeData)
    {
    }

    public int GetNativeDataSize()
    {
        return nint.Size;
    }

    public nint MarshalManagedToNative(object ManagedObj)
    {
        throw new InvalidOperationException();
    }

    public object MarshalNativeToManaged(nint pNativeData)
    {
        return Marshal.PtrToStringUTF8(pNativeData)!;
    }
}