using System;

namespace OMP.LSWTSS.CApi1;

public static class GetNativeClassVtableMethodClassImplSrc
{
    public static string Execute(INativeClassVtableMethodSchema nativeClassVtableMethodSchema)
    {
        var nativeClassMethodClassImplSrcBuilder = new SrcBuilder();

        var nativeClassVtableMethodParamsLambdaSrc = GetFuncParamsLambdaSrc.Execute(nativeClassVtableMethodSchema.Params);
        var nativeClassVtableMethodParamsCallSrc = GetFuncParamsCallSrc.Execute(nativeClassVtableMethodSchema.Params);

        nativeClassMethodClassImplSrcBuilder.Append("Execute = (");
        nativeClassMethodClassImplSrcBuilder.Ident++;
        if (nativeClassVtableMethodParamsLambdaSrc == null)
        {
            nativeClassMethodClassImplSrcBuilder.Append($"Handle handle");
        }
        else
        {
            nativeClassMethodClassImplSrcBuilder.Append($"Handle handle,");
        }
        nativeClassMethodClassImplSrcBuilder.Append(nativeClassVtableMethodParamsLambdaSrc);
        nativeClassMethodClassImplSrcBuilder.Ident--;
        nativeClassMethodClassImplSrcBuilder.Append(") =>");
        nativeClassMethodClassImplSrcBuilder.Append("{");
        nativeClassMethodClassImplSrcBuilder.Ident++;
        nativeClassMethodClassImplSrcBuilder.Append($"var internalExecute = NativeClassVtableMethod.GetInternalExecute<Delegate>(handle, {nativeClassVtableMethodSchema.VtableIndex});");
        nativeClassMethodClassImplSrcBuilder.Append();
        if (nativeClassVtableMethodSchema.ReturnType == null)
        {
            nativeClassMethodClassImplSrcBuilder.Append($"internalExecute(");
        }
        else
        {
            nativeClassMethodClassImplSrcBuilder.Append($"return internalExecute(");
        }
        nativeClassMethodClassImplSrcBuilder.Ident++;
        if (nativeClassVtableMethodParamsCallSrc == null)
        {
            nativeClassMethodClassImplSrcBuilder.Append($"handle");
        }
        else
        {
            nativeClassMethodClassImplSrcBuilder.Append($"handle,");
        }
        nativeClassMethodClassImplSrcBuilder.Append(nativeClassVtableMethodParamsCallSrc);
        nativeClassMethodClassImplSrcBuilder.Ident--;
        nativeClassMethodClassImplSrcBuilder.Append(");");
        nativeClassMethodClassImplSrcBuilder.Ident--;
        nativeClassMethodClassImplSrcBuilder.Append("};");

        return nativeClassMethodClassImplSrcBuilder.ToString().TrimEnd();
    }
}