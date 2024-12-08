namespace OMP.LSWTSS.CApi1;

public static class GetFuncParamCallSrc
{
    public static string Execute(IFuncParamSchema funcParamSchema)
    {
        var funcParamModifierSrc = GetFuncParamModifierSrc.Execute(funcParamSchema);

        var funcParamCallSrc = "";

        if (funcParamModifierSrc != null)
        {
            funcParamCallSrc += $"{funcParamModifierSrc} ";
        }

        funcParamCallSrc += funcParamSchema.Name;

        return funcParamCallSrc;
    }
}