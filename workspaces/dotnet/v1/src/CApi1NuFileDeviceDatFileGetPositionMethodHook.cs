using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly CFuncHook1<CApi1.NuFileDeviceDat.FileGetPositionMethod.NativeDelegate> _cApi1NuFileDeviceDatFileGetPositionMethodHook = new(
        CApi1.NuFileDeviceDat.FileGetPositionMethod.Info.NativePtr,
        (
            nativeDataRawPtr,
            arg0,
            arg1
        ) =>
        {
            if (nativeDataRawPtr == _cApi1NuFileDeviceDat)
            {
                var param0AsString = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(arg0));

                if (param0AsString != null && FetchIsDiskResourceRegistered(GetResourceCanonPath(param0AsString)))
                {
                    Console.WriteLine("Rewiring NuFileDeviceDat::FileGetPosition for: " + param0AsString);

                    return 1;
                }
            }

            return _cApi1NuFileDeviceDatFileGetPositionMethodHook!.Trampoline!(nativeDataRawPtr, arg0, arg1);
        }
    );
}