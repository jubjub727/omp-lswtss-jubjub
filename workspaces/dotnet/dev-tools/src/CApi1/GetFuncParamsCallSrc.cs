using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public static class GetFuncParamsCallSrc
{
    public static string? Execute(IEnumerable<IFuncParamSchema> funcParamsSchema)
    {
        var funcParamCallSrcs = new List<string>();

        foreach (var funcParamSchema in funcParamsSchema)
        {
            funcParamCallSrcs.Add(
                GetFuncParamCallSrc.Execute(funcParamSchema)
            );
        }

        if (funcParamCallSrcs.Count == 0)
        {
            return null;
        }

        return string.Join(",\n", funcParamCallSrcs);
    }
}