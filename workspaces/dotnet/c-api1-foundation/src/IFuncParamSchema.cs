namespace OMP.LSWTSS.CApi1;

public interface IFuncParamSchema
{
    public string Name { get; set; }

    public bool IsInRef { get; set; }

    public bool IsOutRef { get; set; }

    public ITypeSchema Type { get; set; }
}