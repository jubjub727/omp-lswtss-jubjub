namespace OMP.LSWTSS.CApi1;

public sealed class ClassHandleTypeSchema : IClassHandleTypeSchema
{
    public required string ClassName { get; set; }

    public required string? ClassNamespace { get; set; }
}