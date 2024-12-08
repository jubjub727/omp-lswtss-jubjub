namespace OMP.LSWTSS.CApi1;

public sealed class ClassTypeSchema : IClassTypeSchema
{
    public required string ClassName { get; set; }

    public required string? ClassNamespace { get; set; }
}