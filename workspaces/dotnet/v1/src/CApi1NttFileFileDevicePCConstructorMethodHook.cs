using System;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly CFuncHook1<CApi1.nttFile.FileDevicePC.ConstructorMethod.Delegate> cApi1NttFileFileDevicePCConstructorMethodHook = new(
        CApi1.nttFile.FileDevicePC.ConstructorMethod.Ptr,
        (handle, param0, param1) =>
        {
            if (param0 == "host:")
            {
                Console.WriteLine("Acquired nttFileHostFileDevicePCHandle");
                cApi1NttFileHostFileDevicePCHandle = handle;
            }

            return cApi1NttFileFileDevicePCConstructorMethodHook!.Trampoline!(handle, param0, param1);
        }
    );
}