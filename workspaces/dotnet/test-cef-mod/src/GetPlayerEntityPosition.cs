using System;
using System.Numerics;

namespace OMP.LSWTSS;

public partial class TestCefMod : IDisposable
{
    static Vector3? GetPlayerEntityPosition()
    {
        var playerEntityHandle = GetPlayerEntityHandle();

        if (playerEntityHandle == nint.Zero)
        {
            return null;
        }

        return GetEntityPosition(playerEntityHandle);
    }
}