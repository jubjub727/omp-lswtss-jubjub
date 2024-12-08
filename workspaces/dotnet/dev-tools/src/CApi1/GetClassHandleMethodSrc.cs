namespace OMP.LSWTSS.CApi1;

public static class GetClassHandleMethodSrc
{
    public static string Execute(IClassMethodSchema classMethodSchema)
    {
        var classHandleMethodSrcBuilder = new SrcBuilder();

        var classMethodReturnTypeSrc = GetTypeSrc.Execute(classMethodSchema.ReturnType);
        var classMethodParamsLambdaSrc = GetFuncParamsLambdaSrc.Execute(classMethodSchema.Params);
        var classMethodParamsCallSrc = GetFuncParamsCallSrc.Execute(classMethodSchema.Params);

        classHandleMethodSrcBuilder.Append($"public readonly {classMethodReturnTypeSrc} {classMethodSchema.Name}(");
        classHandleMethodSrcBuilder.Ident++;
        classHandleMethodSrcBuilder.Append(classMethodParamsLambdaSrc);
        classHandleMethodSrcBuilder.Ident--;
        classHandleMethodSrcBuilder.Append(")");
        classHandleMethodSrcBuilder.Append("{");
        classHandleMethodSrcBuilder.Ident++;

        if (classMethodSchema.ReturnType == null)
        {
            classHandleMethodSrcBuilder.Append($"{classMethodSchema.Name}Method.Execute(");
        }
        else
        {
            classHandleMethodSrcBuilder.Append($"return {classMethodSchema.Name}Method.Execute(");
        }
        classHandleMethodSrcBuilder.Ident++;
        if (classMethodParamsCallSrc == null)
        {
            classHandleMethodSrcBuilder.Append("this");
        }
        else
        {
            classHandleMethodSrcBuilder.Append("this,");
        }
        classHandleMethodSrcBuilder.Append(classMethodParamsCallSrc);
        classHandleMethodSrcBuilder.Ident--;
        classHandleMethodSrcBuilder.Append(");");

        classHandleMethodSrcBuilder.Ident--;
        classHandleMethodSrcBuilder.Append("}");

        return classHandleMethodSrcBuilder.ToString().TrimEnd();
    }
}