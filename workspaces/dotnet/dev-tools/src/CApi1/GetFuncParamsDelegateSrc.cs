using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public static class GetFuncParamsDelegateSrc
{
    public static string? Execute(IEnumerable<IFuncParamSchema> funcParamsSchema)
    {
        var funcParamDelegateSrcs = new List<string>();

        foreach (var funcParamSchema in funcParamsSchema)
        {
            funcParamDelegateSrcs.Add(
                GetFuncParamDelegateSrc.Execute(funcParamSchema)
            );
        }

        if (funcParamDelegateSrcs.Count == 0)
        {
            return null;
        }

        return string.Join(",\n", funcParamDelegateSrcs);
    }
}