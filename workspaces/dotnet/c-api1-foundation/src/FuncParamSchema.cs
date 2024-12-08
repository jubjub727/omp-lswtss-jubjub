namespace OMP.LSWTSS.CApi1;

public sealed class FuncParamSchema : IFuncParamSchema
{
    public required string Name { get; set; }

    public required bool IsInRef { get; set; }

    public required bool IsOutRef { get; set; }

    public required ITypeSchema Type { get; set; }
}