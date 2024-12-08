namespace OMP.LSWTSS.CApi1;

public interface INativeFuncSchema : IFuncSchema
{
    public uint? SteamOffset { get; set; }

    public uint? EGSOffset { get; set; }
}