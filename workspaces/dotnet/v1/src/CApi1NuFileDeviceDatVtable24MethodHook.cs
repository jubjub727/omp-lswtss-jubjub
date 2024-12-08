using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly CFuncHook1<CApi1.nuFileDeviceDat.Vtable24Method.Delegate> cApi1NuFileDeviceDatVtable24MethodHook = new(
        CApi1.nuFileDeviceDat.Vtable24Method.Ptr,
        (
            handle,
            param0,
            param1,
            param2
        ) =>
        {
            if (handle == cApi1NuFileDeviceDatHandle)
            {
                var param1AsStr = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(param1));

                if (param1AsStr != null && FetchIsDiskResourceRegistered(GetResourceCanonPath(param1AsStr)))
                {
                    Console.WriteLine("Rewiring Vtable24Method resource path: " + param1AsStr);

                    return cApi1NttFileHostFileDevicePCHandle.Vtable24(param0, param1, param2);
                }
            }

            return cApi1NuFileDeviceDatVtable24MethodHook!.Trampoline!(handle, param0, param1, param2);
        }
    );
}