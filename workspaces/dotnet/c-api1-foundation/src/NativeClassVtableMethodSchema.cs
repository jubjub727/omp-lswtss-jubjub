using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public sealed class NativeClassVtableMethodSchema : INativeClassVtableMethodSchema
{
    public required string Name { get; set; }

    public required uint VtableIndex { get; set; }

    public required List<IFuncParamSchema> Params { get; set; }

    public required ITypeSchema? ReturnType { get; set; }
}