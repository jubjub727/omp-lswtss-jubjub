using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public interface IClassSchema
{
    public string Name { get; set; }

    public string? Namespace { get; set; }

    public List<IClassTypeSchema> ParentClassTypes { get; set; }

    public List<IClassFieldSchema> Fields { get; set; }

    public List<IClassStructFieldSchema>? StructFields { get; set; }

    public uint? Size { get; set; }
}