using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public sealed class EnumSchema : IEnumSchema
{
    public required string Name { get; set; }

    public required string? Namespace { get; set; }

    public required List<IEnumEntrySchema> Entries { get; set; }
}