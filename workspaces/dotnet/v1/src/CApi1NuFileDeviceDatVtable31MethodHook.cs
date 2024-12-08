using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly CFuncHook1<CApi1.nuFileDeviceDat.Vtable31Method.Delegate> cApi1NuFileDeviceDatVtable31MethodHook = new(
        CApi1.nuFileDeviceDat.Vtable31Method.Ptr,
        (
            handle,
            param0,
            param1
        ) =>
        {
            if (handle == cApi1NuFileDeviceDatHandle)
            {
                var param0AsStr = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(param0));

                if (param0AsStr != null && FetchIsDiskResourceRegistered(GetResourceCanonPath(param0AsStr)))
                {
                    Console.WriteLine("Rewiring Vtable31Method resource path: " + param0AsStr);

                    return 1;
                }
            }

            return cApi1NuFileDeviceDatVtable31MethodHook!.Trampoline!(handle, param0, param1);
        }
    );
}