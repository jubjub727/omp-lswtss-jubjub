using System;

namespace OMP.LSWTSS.CApi1;

public static class AppendAsIScriptMainContextGetReturnSrc
{
    public static void Execute(
        ITypeSchema angelScriptFuncReturnTypeSchema,
        SrcBuilder angelScriptFuncClassImplSrcBuilder
    )
    {
        if (angelScriptFuncReturnTypeSchema is IPrimitiveTypeSchema angelScriptFuncReturnPrimitiveTypeSchema)
        {
            var angelScriptFuncParamPrimitiveTypeGetReturnSrc = angelScriptFuncReturnPrimitiveTypeSchema.PrimitiveKind switch
            {
                PrimitiveKind.Pointer => "return AsIScriptMainContext.Handle.GetReturnAddress();",
                PrimitiveKind.Bool => "return AsIScriptMainContext.Handle.GetReturnByte() != 0;",
                PrimitiveKind.Byte => "return unchecked((sbyte)AsIScriptMainContext.Handle.GetReturnByte());",
                PrimitiveKind.UByte => "return AsIScriptMainContext.Handle.GetReturnByte();",
                PrimitiveKind.Short => "return unchecked((short)AsIScriptMainContext.Handle.GetReturnWord());",
                PrimitiveKind.UShort => "return AsIScriptMainContext.Handle.GetReturnWord();",
                PrimitiveKind.Int => "return unchecked((int)AsIScriptMainContext.Handle.GetReturnDWord());",
                PrimitiveKind.UInt => "return AsIScriptMainContext.Handle.GetReturnDWord();",
                PrimitiveKind.Long => "return unchecked((long)AsIScriptMainContext.Handle.GetReturnQWord());",
                PrimitiveKind.ULong => "return AsIScriptMainContext.Handle.GetReturnQWord();",
                PrimitiveKind.Float => "return AsIScriptMainContext.Handle.GetReturnFloat();",
                PrimitiveKind.Double => "return AsIScriptMainContext.Handle.GetReturnDouble();",
                _ => throw new InvalidOperationException()
            };

            angelScriptFuncClassImplSrcBuilder.Append(angelScriptFuncParamPrimitiveTypeGetReturnSrc);
        }
        else if (angelScriptFuncReturnTypeSchema is IStringTypeSchema angelScriptFuncReturnStringTypeSchema)
        {
            if (angelScriptFuncReturnStringTypeSchema.IsStringOwned)
            {
                throw new InvalidOperationException();
            }

            angelScriptFuncClassImplSrcBuilder.Append($"return System.Runtime.InteropServices.Marshal.PtrToStringUTF8(System.Runtime.InteropServices.Marshal.ReadIntPtr(AsIScriptMainContext.Handle.GetReturnObject()))!;");
        }
        else if (angelScriptFuncReturnTypeSchema is IClassTypeSchema angelScriptFuncReturnClassTypeSchema)
        {
            var angelScriptFuncReturnClassTypeSrc = GetTypeSrc.Execute(angelScriptFuncReturnClassTypeSchema);

            angelScriptFuncClassImplSrcBuilder.Append($"var returnValue = default({angelScriptFuncReturnClassTypeSrc});");
            angelScriptFuncClassImplSrcBuilder.Append($"System.Runtime.InteropServices.Marshal.PtrToStructure(AsIScriptMainContext.Handle.GetReturnObject(), returnValue);");
            angelScriptFuncClassImplSrcBuilder.Append($"return returnValue;");
        }
        else if (angelScriptFuncReturnTypeSchema is IClassHandleTypeSchema angelScriptFuncReturnClassHandleTypeSchema)
        {
            var angelScriptFuncReturnClassTypeSrc = GetTypeSrc.Execute(angelScriptFuncReturnClassHandleTypeSchema);

            angelScriptFuncClassImplSrcBuilder.Append($"return ({angelScriptFuncReturnClassTypeSrc})AsIScriptMainContext.Handle.GetReturnObject();");
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}