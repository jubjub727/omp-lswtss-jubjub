using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public interface IFuncSchema
{
    public string Name { get; set; }

    public List<IFuncParamSchema> Params { get; set; }

    public ITypeSchema? ReturnType { get; set; }
}