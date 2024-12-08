using System;
using System.Numerics;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod : IDisposable
{
    static Vector3? GetEntityPosition(apiEntity.Handle entityHandle)
    {
        if (entityHandle == nint.Zero)
        {
            return null;
        }

        var entityTransformComponentHandle = (apiTransformComponent.Handle)(nint)entityHandle.FindComponentByTypeName("apiTransformComponent");

        if (entityTransformComponentHandle == nint.Zero)
        {
            return null;
        }

        entityTransformComponentHandle.GetPosition(
            out var entityPositionX,
            out var entityPositionY,
            out var entityPositionZ
        );

        return new Vector3
        {
            X = entityPositionX,
            Y = entityPositionY,
            Z = entityPositionZ
        };
    }
}