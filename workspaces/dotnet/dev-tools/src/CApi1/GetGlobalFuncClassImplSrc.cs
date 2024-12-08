using System;

namespace OMP.LSWTSS.CApi1;

public static class GetGlobalFuncClassImplSrc
{
    public static string Execute(IGlobalFuncSchema globalFuncSchema)
    {
        var globalNativeFuncSchema = globalFuncSchema as IGlobalNativeFuncSchema;
        var angelScriptGlobalFuncSchema = globalFuncSchema as IAngelScriptGlobalFuncSchema;

        if (globalNativeFuncSchema != null)
        {
            return GetNativeFuncClassImplSrc.Execute(globalNativeFuncSchema);
        }
        else if (angelScriptGlobalFuncSchema != null)
        {
            return GetAngelScriptGlobalFuncClassImplSrc.Execute(angelScriptGlobalFuncSchema);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}