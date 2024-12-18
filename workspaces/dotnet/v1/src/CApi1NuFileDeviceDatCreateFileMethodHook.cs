using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly CFuncHook1<CApi1.NuFileDeviceDat.CreateFileMethod.NativeDelegate> _cApi1NuFileDeviceDatCreateFileMethodHook = new(
        CApi1.NuFileDeviceDat.CreateFileMethod.Info.NativePtr,
        (
            nativeDataRawPtr,
            arg0,
            arg1,
            arg2
        ) =>
        {
            if (nativeDataRawPtr == _cApi1NuFileDeviceDat)
            {
                var arg1AsString = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(arg1));

                if (arg1AsString != null && FetchIsDiskResourceRegistered(GetResourceCanonPath(arg1AsString)))
                {
                    Console.WriteLine("Rewiring NuFileDeviceDat::CreateFile for: " + arg1AsString);

                    return _cApi1NttFileHostFileDevicePC.CreateFile(arg0, arg1, arg2);
                }
            }

            return _cApi1NuFileDeviceDatCreateFileMethodHook!.Trampoline!(nativeDataRawPtr, arg0, arg1, arg2);
        }
    );
}