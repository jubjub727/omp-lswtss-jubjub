using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public interface IEnumSchema
{
    public string Name { get; set; }

    public string? Namespace { get; set; }

    public List<IEnumEntrySchema> Entries { get; set; }
}