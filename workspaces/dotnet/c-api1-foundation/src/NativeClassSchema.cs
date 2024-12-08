using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public sealed class NativeClassSchema : INativeClassSchema
{
    public required string Name { get; set; }

    public required string? Namespace { get; set; }

    public required List<IClassTypeSchema> ParentClassTypes { get; set; }

    public required List<IClassFieldSchema> Fields { get; set; }

    public required List<IClassStructFieldSchema>? StructFields { get; set; }

    public required List<INativeClassMethodSchema> Methods { get; set; }

    public required uint? Size { get; set; }
}