namespace OMP.LSWTSS.CApi1;

public interface IClassFieldSchema
{
    public string Name { get; set; }

    public ITypeSchema Type { get; set; }

    public uint? SteamOffset { get; set; }

    public uint? EGSOffset { get; set; }
}