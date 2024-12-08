namespace OMP.LSWTSS.CApi1;

public static class GetAngelScriptGlobalFuncClassImplSrc
{
    public static string Execute(IAngelScriptGlobalFuncSchema angelScriptGlobalFuncSchema)
    {
        var angelScriptGlobalFuncClassImplSrcBuilder = new SrcBuilder();

        var angelScriptGlobalFuncParamsLambdaSrc = GetFuncParamsLambdaSrc.Execute(angelScriptGlobalFuncSchema.Params);

        angelScriptGlobalFuncClassImplSrcBuilder.Append("Execute = (");
        angelScriptGlobalFuncClassImplSrcBuilder.Ident++;
        angelScriptGlobalFuncClassImplSrcBuilder.Append(angelScriptGlobalFuncParamsLambdaSrc);
        angelScriptGlobalFuncClassImplSrcBuilder.Ident--;
        angelScriptGlobalFuncClassImplSrcBuilder.Append(") =>");

        angelScriptGlobalFuncClassImplSrcBuilder.Append("{");
        angelScriptGlobalFuncClassImplSrcBuilder.Ident++;

        angelScriptGlobalFuncClassImplSrcBuilder.Append("unsafe");
        angelScriptGlobalFuncClassImplSrcBuilder.Append("{");
        angelScriptGlobalFuncClassImplSrcBuilder.Ident++;

        angelScriptGlobalFuncClassImplSrcBuilder.Append($"var asIScriptFunctionHandle = AsIScriptMainContext.EngineHandle.GetGlobalFunctionByIndex(GetVariantValue.Execute({angelScriptGlobalFuncSchema.SteamIndex ?? 0}, {angelScriptGlobalFuncSchema.EGSIndex ?? 0}));");
        angelScriptGlobalFuncClassImplSrcBuilder.Append("AsIScriptMainContext.Handle.Prepare(asIScriptFunctionHandle);");

        uint blockCount = 0;

        for (uint angelScriptGlobalFuncParamIndex = 0; angelScriptGlobalFuncParamIndex < angelScriptGlobalFuncSchema.Params.Count; angelScriptGlobalFuncParamIndex++)
        {
            AppendAsIScriptMainContextSetArgSrc.Execute(
                angelScriptGlobalFuncParamIndex,
                angelScriptGlobalFuncSchema.Params[(int)angelScriptGlobalFuncParamIndex],
                angelScriptGlobalFuncClassImplSrcBuilder,
                ref blockCount
            );
        }

        angelScriptGlobalFuncClassImplSrcBuilder.Append("if (AsIScriptMainContext.Handle.Execute() != 0)");
        angelScriptGlobalFuncClassImplSrcBuilder.Append("{");
        angelScriptGlobalFuncClassImplSrcBuilder.Ident++;
        angelScriptGlobalFuncClassImplSrcBuilder.Append("throw new System.InvalidOperationException();");
        angelScriptGlobalFuncClassImplSrcBuilder.Ident--;
        angelScriptGlobalFuncClassImplSrcBuilder.Append("}");

        if (angelScriptGlobalFuncSchema.ReturnType != null)
        {
            AppendAsIScriptMainContextGetReturnSrc.Execute(
                angelScriptGlobalFuncSchema.ReturnType,
                angelScriptGlobalFuncClassImplSrcBuilder
            );
        }

        for (uint i = 0; i < blockCount; i++)
        {
            angelScriptGlobalFuncClassImplSrcBuilder.Ident--;
            angelScriptGlobalFuncClassImplSrcBuilder.Append("}");
        }

        angelScriptGlobalFuncClassImplSrcBuilder.Ident--;
        angelScriptGlobalFuncClassImplSrcBuilder.Append("};");

        angelScriptGlobalFuncClassImplSrcBuilder.Ident--;
        angelScriptGlobalFuncClassImplSrcBuilder.Append("};");

        return angelScriptGlobalFuncClassImplSrcBuilder.ToString().TrimEnd();
    }
}