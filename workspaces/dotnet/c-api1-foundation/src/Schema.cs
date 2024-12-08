using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public sealed class Schema
{
    public required List<IEnumSchema> Enums { get; set; }

    public required List<IGlobalFuncSchema> GlobalFuncs { get; set; }

    public required List<IClassSchema> Classes { get; set; }
}