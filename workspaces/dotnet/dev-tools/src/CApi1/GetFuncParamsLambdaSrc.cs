using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public static class GetFuncParamsLambdaSrc
{
    public static string? Execute(IEnumerable<IFuncParamSchema> funcParamsSchema)
    {
        var funcParamLambdaSrcs = new List<string>();

        foreach (var funcParamSchema in funcParamsSchema)
        {
            funcParamLambdaSrcs.Add(
                GetFuncParamLambdaSrc.Execute(funcParamSchema)
            );
        }

        if (funcParamLambdaSrcs.Count == 0)
        {
            return null;
        }

        return string.Join(",\n", funcParamLambdaSrcs);
    }
}