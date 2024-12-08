namespace OMP.LSWTSS.CApi1;

public sealed class ClassStructFieldSchema : IClassStructFieldSchema
{
    public required string Name { get; set; }

    public required ITypeSchema Type { get; set; }
}