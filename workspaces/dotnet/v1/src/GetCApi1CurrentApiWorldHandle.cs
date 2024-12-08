using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

public partial class V1
{
    public static CApi1.ApiWorld.Handle GetCApi1CurrentApiWorldHandle()
    {
        var cApi1CurrentApiWorldHandle = (CApi1.ApiWorld.Handle)Marshal.ReadIntPtr(
            CApi1.NativeFunc.GetPtr(
                GetVariantValue.Execute(steamValue: 0x5f129f8, egsValue: 0x5f13308)
            )
        );

        if (cApi1CurrentApiWorldHandle == nint.Zero)
        {
            return cApi1CurrentApiWorldHandle;
        }

        var cApi1CurrentApiWorldCurrentState = cApi1CurrentApiWorldHandle.get_CurrentState();

        if (cApi1CurrentApiWorldCurrentState != 2)
        {
            return (CApi1.ApiWorld.Handle)nint.Zero;
        }

        return cApi1CurrentApiWorldHandle;
    }
}