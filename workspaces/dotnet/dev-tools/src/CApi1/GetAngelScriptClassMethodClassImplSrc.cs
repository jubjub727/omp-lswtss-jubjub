namespace OMP.LSWTSS.CApi1;

public static class GetAngelScriptClassMethodClassImplSrc
{
    public static string Execute(IAngelScriptClassSchema angelScriptClassSchema, IAngelScriptClassMethodSchema angelScriptClassMethodSchema)
    {
        var angelScriptClassNativeMethodSchema = angelScriptClassMethodSchema as IAngelScriptClassNativeMethodSchema;

        if (angelScriptClassNativeMethodSchema != null)
        {
            return GetNativeFuncClassImplSrc.Execute(angelScriptClassNativeMethodSchema);
        }

        var angelScriptClassMethodClassImplSrcBuilder = new SrcBuilder();

        var angelScriptMethodParamsLambdaSrc = GetFuncParamsLambdaSrc.Execute(angelScriptClassMethodSchema.Params);

        angelScriptClassMethodClassImplSrcBuilder.Append("Execute = (");
        angelScriptClassMethodClassImplSrcBuilder.Ident++;
        if (angelScriptMethodParamsLambdaSrc == null)
        {
            angelScriptClassMethodClassImplSrcBuilder.Append($"Handle handle");
        }
        else
        {
            angelScriptClassMethodClassImplSrcBuilder.Append($"Handle handle,");
            angelScriptClassMethodClassImplSrcBuilder.Append(angelScriptMethodParamsLambdaSrc);
        }
        angelScriptClassMethodClassImplSrcBuilder.Ident--;
        angelScriptClassMethodClassImplSrcBuilder.Append(") =>");

        angelScriptClassMethodClassImplSrcBuilder.Append("{");
        angelScriptClassMethodClassImplSrcBuilder.Ident++;

        angelScriptClassMethodClassImplSrcBuilder.Append("unsafe");
        angelScriptClassMethodClassImplSrcBuilder.Append("{");
        angelScriptClassMethodClassImplSrcBuilder.Ident++;

        angelScriptClassMethodClassImplSrcBuilder.Append($"var asIObjectTypeHandle = AsIScriptMainContext.EngineHandle.GetObjectTypeByIndex(GetVariantValue.Execute({angelScriptClassSchema.SteamIndex ?? 0}, {angelScriptClassSchema.EGSIndex ?? 0}));");
        angelScriptClassMethodClassImplSrcBuilder.Append($"var asIObjectTypeMethodHandle = asIObjectTypeHandle.GetMethodByIndex(GetVariantValue.Execute({angelScriptClassMethodSchema.SteamIndex ?? 0}, {angelScriptClassMethodSchema.EGSIndex ?? 0}), true);");
        angelScriptClassMethodClassImplSrcBuilder.Append("AsIScriptMainContext.Handle.Prepare(asIObjectTypeMethodHandle);");
        angelScriptClassMethodClassImplSrcBuilder.Append("AsIScriptMainContext.Handle.SetObject(handle);");

        uint blockCount = 0;

        for (uint angelScriptClassMethodParamIndex = 0; angelScriptClassMethodParamIndex < angelScriptClassMethodSchema.Params.Count; angelScriptClassMethodParamIndex++)
        {
            AppendAsIScriptMainContextSetArgSrc.Execute(
                angelScriptClassMethodParamIndex,
                angelScriptClassMethodSchema.Params[(int)angelScriptClassMethodParamIndex],
                angelScriptClassMethodClassImplSrcBuilder,
                ref blockCount
            );
        }

        angelScriptClassMethodClassImplSrcBuilder.Append("if (AsIScriptMainContext.Handle.Execute() != 0)");
        angelScriptClassMethodClassImplSrcBuilder.Append("{");
        angelScriptClassMethodClassImplSrcBuilder.Ident++;
        angelScriptClassMethodClassImplSrcBuilder.Append("throw new System.InvalidOperationException();");
        angelScriptClassMethodClassImplSrcBuilder.Ident--;
        angelScriptClassMethodClassImplSrcBuilder.Append("}");

        if (angelScriptClassMethodSchema.ReturnType != null)
        {
            AppendAsIScriptMainContextGetReturnSrc.Execute(
                angelScriptClassMethodSchema.ReturnType,
                angelScriptClassMethodClassImplSrcBuilder
            );
        }

        for (uint i = 0; i < blockCount; i++)
        {
            angelScriptClassMethodClassImplSrcBuilder.Ident--;
            angelScriptClassMethodClassImplSrcBuilder.Append("}");
        }

        angelScriptClassMethodClassImplSrcBuilder.Ident--;
        angelScriptClassMethodClassImplSrcBuilder.Append("};");

        angelScriptClassMethodClassImplSrcBuilder.Ident--;
        angelScriptClassMethodClassImplSrcBuilder.Append("};");

        return angelScriptClassMethodClassImplSrcBuilder.ToString().TrimEnd();
    }
}