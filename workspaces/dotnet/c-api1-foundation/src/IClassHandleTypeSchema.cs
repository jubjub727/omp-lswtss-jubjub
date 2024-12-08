namespace OMP.LSWTSS.CApi1;

public interface IClassHandleTypeSchema : ITypeSchema
{
    public string ClassName { get; set; }

    public string? ClassNamespace { get; set; }
}