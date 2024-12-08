using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public static class NativeFunc
{
    public static uint GetOffset(nint ptr)
    {
        var processMainModuleBaseAddress = Process.GetCurrentProcess().MainModule!.BaseAddress;

        return (uint)(ptr - processMainModuleBaseAddress);
    }

    public static nint GetPtr(uint offset)
    {
        var processMainModuleBaseAddress = Process.GetCurrentProcess().MainModule!.BaseAddress;

        return nint.Add(processMainModuleBaseAddress, (int)offset);
    }

    public static T GetExecute<T>(nint ptr) where T : notnull
    {
        return Marshal.GetDelegateForFunctionPointer<T>(ptr);
    }
}