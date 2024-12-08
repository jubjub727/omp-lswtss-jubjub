using System;
using System.Runtime.InteropServices;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod
{
    static ApiWorld.Handle GetCurrentApiWorldHandle()
    {
        var currentApiWorldHandle = (ApiWorld.Handle)Marshal.ReadIntPtr(
            NativeFunc.GetPtr(
                GetVariantValue.Execute(steamValue: 0x5f129f8, egsValue: 0x5f13308)
            )
        );

        if (currentApiWorldHandle == nint.Zero)
        {
            return currentApiWorldHandle;
        }

        var currentApiWorldCurrentState = currentApiWorldHandle.get_CurrentState();

        if (currentApiWorldCurrentState != 2)
        {
            return (ApiWorld.Handle)nint.Zero;
        }

        return currentApiWorldHandle;
    }
}