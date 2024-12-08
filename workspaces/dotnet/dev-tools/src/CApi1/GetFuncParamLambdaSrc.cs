namespace OMP.LSWTSS.CApi1;

public static class GetFuncParamLambdaSrc
{
    public static string Execute(IFuncParamSchema funcParamSchema)
    {
        var funcParamTypeSrc = GetTypeSrc.Execute(funcParamSchema.Type);

        var funcParamModifierSrc = GetFuncParamModifierSrc.Execute(funcParamSchema);

        var funcParamDefSrc = "";

        if (funcParamModifierSrc != null)
        {
            funcParamDefSrc += $"{funcParamModifierSrc} ";
        }

        funcParamDefSrc += $"{funcParamTypeSrc} ";

        funcParamDefSrc += funcParamSchema.Name;

        return funcParamDefSrc;
    }
}