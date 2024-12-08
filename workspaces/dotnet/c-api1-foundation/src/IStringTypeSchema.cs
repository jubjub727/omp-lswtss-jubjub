namespace OMP.LSWTSS.CApi1;

public interface IStringTypeSchema : ITypeSchema
{
    public bool IsStringNullable { get; set; }

    public bool IsStringOwned { get; set; }
}