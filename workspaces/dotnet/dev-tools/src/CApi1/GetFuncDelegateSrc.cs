namespace OMP.LSWTSS.CApi1;

public static class GetFuncDelegateSrc
{
    public static string Execute(IFuncSchema funcSchema)
    {
        var classMethodSchema = funcSchema as IClassMethodSchema;

        var funcDelegateSrcBuilder = new SrcBuilder();

        var funcReturnTypeSrc = GetTypeSrc.Execute(funcSchema.ReturnType);

        var funcReturnTypeMarshalAsModifierSrc = GetTypeMarshalAsAttributeSrc.Execute(funcSchema.ReturnType);

        if (funcReturnTypeMarshalAsModifierSrc != null)
        {
            funcDelegateSrcBuilder.Append($"[return: {funcReturnTypeMarshalAsModifierSrc}]");
        }

        funcDelegateSrcBuilder.Append($"public delegate {funcReturnTypeSrc} Delegate(");
        funcDelegateSrcBuilder.Ident++;

        var funcParamsDelegateSrc = GetFuncParamsDelegateSrc.Execute(funcSchema.Params);

        if (classMethodSchema != null)
        {
            if (funcParamsDelegateSrc == null)
            {
                funcDelegateSrcBuilder.Append("Handle handle");
            }
            else
            {
                funcDelegateSrcBuilder.Append("Handle handle,");
                funcDelegateSrcBuilder.Append(funcParamsDelegateSrc);
            }
        }
        else if (funcParamsDelegateSrc != null)
        {
            funcDelegateSrcBuilder.Append(funcParamsDelegateSrc);
        }

        funcDelegateSrcBuilder.Ident--;
        funcDelegateSrcBuilder.Append(");");

        return funcDelegateSrcBuilder.ToString().TrimEnd();
    }
}