namespace OMP.LSWTSS;

public static partial class V1
{
    public static CApi1.NuVec3 ToCApiNuVec3(this System.Numerics.Vector3 @this)
    {
        return new CApi1.NuVec3
        {
            X = @this.X,
            Y = @this.Y,
            Z = @this.Z
        };
    }
}