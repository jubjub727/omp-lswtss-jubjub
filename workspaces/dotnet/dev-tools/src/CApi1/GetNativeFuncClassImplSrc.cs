namespace OMP.LSWTSS.CApi1;

public static class GetNativeFuncClassImplSrc
{
    public static string Execute(INativeFuncSchema nativeFuncSchema)
    {
        var nativeFuncClassImplSrcBuilder = new SrcBuilder();

        nativeFuncClassImplSrcBuilder.Append($"Ptr = NativeFunc.GetPtr(GetVariantValue.Execute({nativeFuncSchema.SteamOffset ?? 0}, {nativeFuncSchema.EGSOffset ?? 0}));");
        nativeFuncClassImplSrcBuilder.Append($"Execute = NativeFunc.GetExecute<Delegate>(Ptr);");
        nativeFuncClassImplSrcBuilder.Append();

        return nativeFuncClassImplSrcBuilder.ToString().TrimEnd();
    }
}