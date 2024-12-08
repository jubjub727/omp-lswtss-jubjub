using System;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod : IDisposable
{
    static apiEntity.Handle GetPlayerEntityHandle()
    {
        var currentApiWorldHandle = GetCurrentApiWorldHandle();

        if (currentApiWorldHandle == nint.Zero)
        {
            return (apiEntity.Handle)nint.Zero;
        }

        var nttUniverseHandle = currentApiWorldHandle.GetUniverse();

        if (nttUniverseHandle == nint.Zero)
        {
            return (apiEntity.Handle)nint.Zero;
        }

        var playerControlSystemHandle = PlayerControlSystem.GetFromGlobalFunc.Execute(nttUniverseHandle);

        if (playerControlSystemHandle == nint.Zero)
        {
            return (apiEntity.Handle)nint.Zero;
        }

        return playerControlSystemHandle.GetPlayerEntityForPlayerIdx(0);
    }
}