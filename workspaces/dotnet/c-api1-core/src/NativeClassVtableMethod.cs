using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public static class NativeClassVtableMethod
{
    public static T GetInternalExecute<T>(nint classPtr, int vtableIndex) where T : notnull
    {
        var classVtablePtr = Marshal.ReadIntPtr(classPtr);

        var internalExecutePtr = Marshal.ReadIntPtr(classVtablePtr, vtableIndex * nint.Size);

        return Marshal.GetDelegateForFunctionPointer<T>(internalExecutePtr);
    }
}