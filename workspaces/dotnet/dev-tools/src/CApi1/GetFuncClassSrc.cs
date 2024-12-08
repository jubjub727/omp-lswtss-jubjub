using System;

namespace OMP.LSWTSS.CApi1;

public static class GetFuncClassSrc
{
    public static string Execute(IFuncSchema funcSchema, string funcClassImplSrc)
    {
        var nativeFuncSchema = funcSchema as INativeFuncSchema;
        var globalFuncSchema = funcSchema as IGlobalFuncSchema;
        var classMethodSchema = funcSchema as IClassMethodSchema;

        var funcClassSrcBuilder = new SrcBuilder();

        var funcClassNameSrc = funcSchema.Name;

        if (globalFuncSchema != null)
        {
            funcClassNameSrc += "GlobalFunc";
        }
        else if (classMethodSchema != null)
        {
            funcClassNameSrc += "Method";
        }
        else
        {
            throw new InvalidOperationException();
        }

        funcClassSrcBuilder.Append($"public static class {funcClassNameSrc}");
        funcClassSrcBuilder.Append("{");
        funcClassSrcBuilder.Ident++;

        funcClassSrcBuilder.Append(GetFuncDelegateSrc.Execute(funcSchema));
        funcClassSrcBuilder.Append();

        if (nativeFuncSchema != null)
        {
            funcClassSrcBuilder.Append("public static readonly nint Ptr;");
            funcClassSrcBuilder.Append();
        }

        funcClassSrcBuilder.Append("public static readonly Delegate Execute;");
        funcClassSrcBuilder.Append();

        funcClassSrcBuilder.Append($"static {funcClassNameSrc}()");
        funcClassSrcBuilder.Append("{");
        funcClassSrcBuilder.Ident++;
        funcClassSrcBuilder.Append(funcClassImplSrc);
        funcClassSrcBuilder.Ident--;
        funcClassSrcBuilder.Append("}");

        funcClassSrcBuilder.Ident--;
        funcClassSrcBuilder.Append("}");

        return funcClassSrcBuilder.ToString().TrimEnd();
    }
}