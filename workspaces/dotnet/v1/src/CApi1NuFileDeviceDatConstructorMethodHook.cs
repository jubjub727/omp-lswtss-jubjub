using System;

namespace OMP.LSWTSS;

public static partial class V1
{
    static readonly CFuncHook1<CApi1.nuFileDeviceDat.ConstructorMethod.Delegate> cApi1NuFileDeviceDatConstructorMethodHook = new(
        CApi1.nuFileDeviceDat.ConstructorMethod.Ptr,
        (handle) =>
        {
            cApi1NuFileDeviceDatHandle = handle;
            Console.WriteLine("Acquired nuFileDeviceDatHandle");
            return cApi1NuFileDeviceDatConstructorMethodHook!.Trampoline!(handle);
        }
    );
}