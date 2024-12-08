namespace OMP.LSWTSS.CApi1;

public sealed class ClassFieldSchema : IClassFieldSchema
{
    public required string Name { get; set; }

    public required ITypeSchema Type { get; set; }

    public required uint? SteamOffset { get; set; }

    public required uint? EGSOffset { get; set; }
}