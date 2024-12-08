using System;

namespace OMP.LSWTSS.CApi1;

public static class GetNativeClassMethodClassImplSrc
{
    public static string Execute(INativeClassMethodSchema nativeClassMethodSchema)
    {
        var nativeClassNativeMethodSchema = nativeClassMethodSchema as INativeClassNativeMethodSchema;
        var nativeClassVtableMethodSchema = nativeClassMethodSchema as INativeClassVtableMethodSchema;

        if (nativeClassNativeMethodSchema != null)
        {
            return GetNativeFuncClassImplSrc.Execute(nativeClassNativeMethodSchema);
        }
        else if (nativeClassVtableMethodSchema != null)
        {
            return GetNativeClassVtableMethodClassImplSrc.Execute(nativeClassVtableMethodSchema);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}