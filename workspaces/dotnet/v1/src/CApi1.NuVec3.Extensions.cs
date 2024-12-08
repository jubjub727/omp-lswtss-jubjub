namespace OMP.LSWTSS;

public static partial class V1
{
    public static System.Numerics.Vector3 ToSystemNumericsVector3(this CApi1.NuVec3 @this)
    {
        return new System.Numerics.Vector3
        {
            X = @this.X,
            Y = @this.Y,
            Z = @this.Z
        };
    }
}