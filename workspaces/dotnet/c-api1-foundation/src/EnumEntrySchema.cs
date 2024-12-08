namespace OMP.LSWTSS.CApi1;

public sealed class EnumEntrySchema : IEnumEntrySchema
{
    public required string Name { get; set; }

    public required int Value { get; set; }
}