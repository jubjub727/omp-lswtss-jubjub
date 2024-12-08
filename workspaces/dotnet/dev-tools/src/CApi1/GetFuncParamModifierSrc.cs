namespace OMP.LSWTSS.CApi1;

public static class GetFuncParamModifierSrc
{
    public static string? Execute(IFuncParamSchema funcParamSchema)
    {
        if (funcParamSchema.IsOutRef)
        {
            return "out";
        }

        if (funcParamSchema.IsInRef)
        {
            return "ref";
        }

        return null;
    }
}