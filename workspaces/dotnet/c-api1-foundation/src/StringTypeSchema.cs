namespace OMP.LSWTSS.CApi1;

public sealed class StringTypeSchema : IStringTypeSchema
{
    public required bool IsStringNullable { get; set; }

    public required bool IsStringOwned { get; set; }
}