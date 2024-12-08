namespace OMP.LSWTSS.CApi1;

public interface IClassTypeSchema : ITypeSchema
{
    public string ClassName { get; set; }

    public string? ClassNamespace { get; set; }
}