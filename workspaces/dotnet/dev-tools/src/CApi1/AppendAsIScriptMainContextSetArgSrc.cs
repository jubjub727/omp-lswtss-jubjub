using System;

namespace OMP.LSWTSS.CApi1;

public static class AppendAsIScriptMainContextSetArgSrc
{
    public static void Execute(
        uint angelScriptFuncParamIndex,
        IFuncParamSchema angelScriptFuncParamSchema,
        SrcBuilder angelScriptFuncClassImplSrcBuilder,
        ref uint blockCount
    )
    {
        if (angelScriptFuncParamSchema.IsInRef || angelScriptFuncParamSchema.IsOutRef)
        {
            var angelScriptFuncParamTypeSrc = GetTypeSrc.Execute(angelScriptFuncParamSchema.Type);

            angelScriptFuncClassImplSrcBuilder.Append($"fixed ({angelScriptFuncParamTypeSrc}* {angelScriptFuncParamSchema.Name}Ptr = &{angelScriptFuncParamSchema.Name})");
            angelScriptFuncClassImplSrcBuilder.Append("{");
            angelScriptFuncClassImplSrcBuilder.Ident++;
            blockCount++;

            if (angelScriptFuncParamSchema.Type is IPrimitiveTypeSchema)
            {
                angelScriptFuncClassImplSrcBuilder.Append($"AsIScriptMainContext.Handle.SetArgAddress({angelScriptFuncParamIndex}, (nint){angelScriptFuncParamSchema.Name}Ptr);");
            }
            else if (angelScriptFuncParamSchema.Type is IStringTypeSchema angelScriptFuncParamStringTypeSchema)
            {
                throw new InvalidOperationException();
            }
            else if (angelScriptFuncParamSchema.Type is IClassTypeSchema angelScriptFuncParamClassTypeSchema)
            {
                var angelScriptFuncParamClassTypeSrc = GetTypeSrc.Execute(angelScriptFuncParamClassTypeSchema);

                angelScriptFuncClassImplSrcBuilder.Append($"using var {angelScriptFuncParamSchema.Name}PtrManualMarshaler = new StructPtrManualMarshaler<{angelScriptFuncParamClassTypeSrc}>({angelScriptFuncParamSchema.Name}Ptr);");
                angelScriptFuncClassImplSrcBuilder.Append();

                angelScriptFuncClassImplSrcBuilder.Append($"AsIScriptMainContext.Handle.SetArgAddress({angelScriptFuncParamIndex}, {angelScriptFuncParamSchema.Name}PtrManualMarshaler.NativeValuePtr);");
            }
            else if (angelScriptFuncParamSchema.Type is IClassHandleTypeSchema angelScriptFuncParamClassHandleTypeSchema)
            {
                var angelScriptFuncParamClassHandleTypeSrc = GetTypeSrc.Execute(angelScriptFuncParamClassHandleTypeSchema);

                angelScriptFuncClassImplSrcBuilder.Append($"using var {angelScriptFuncParamSchema.Name}PtrManualMarshaler = new StructPtrManualMarshaler<{angelScriptFuncParamClassHandleTypeSrc}>({angelScriptFuncParamSchema.Name}Ptr);");
                angelScriptFuncClassImplSrcBuilder.Append();

                angelScriptFuncClassImplSrcBuilder.Append($"AsIScriptMainContext.Handle.SetArgAddress({angelScriptFuncParamIndex}, {angelScriptFuncParamSchema.Name}PtrManualMarshaler.NativeValuePtr);");
            }
            else
            {
                throw new InvalidOperationException();
            }

            angelScriptFuncClassImplSrcBuilder.Append();
        }
        else
        {
            if (angelScriptFuncParamSchema.Type is IPrimitiveTypeSchema angelScriptFuncParamPrimitiveTypeSchema)
            {
                var angelScriptFuncParamPrimitiveTypeSetArgNameSrc = angelScriptFuncParamPrimitiveTypeSchema.PrimitiveKind switch
                {
                    PrimitiveKind.Pointer => "SetArgAddress",
                    PrimitiveKind.Bool => "SetArgByte",
                    PrimitiveKind.Byte => "SetArgByte",
                    PrimitiveKind.UByte => "SetArgByte",
                    PrimitiveKind.Short => "SetArgWord",
                    PrimitiveKind.UShort => "SetArgWord",
                    PrimitiveKind.Int => "SetArgDWord",
                    PrimitiveKind.UInt => "SetArgDWord",
                    PrimitiveKind.Long => "SetArgQWord",
                    PrimitiveKind.ULong => "SetArgQWord",
                    PrimitiveKind.Float => "SetArgFloat",
                    PrimitiveKind.Double => "SetArgDouble",
                    _ => throw new InvalidOperationException()
                };

                var angelScriptFuncParamPrimitiveTypeSetArgCastSrc = angelScriptFuncParamPrimitiveTypeSchema.PrimitiveKind switch
                {
                    PrimitiveKind.Pointer => angelScriptFuncParamSchema.Name,
                    PrimitiveKind.Bool => $"unchecked((byte)({angelScriptFuncParamSchema.Name} ? 1 : 0))",
                    PrimitiveKind.Byte => angelScriptFuncParamSchema.Name,
                    PrimitiveKind.UByte => $"unchecked((byte){angelScriptFuncParamSchema.Name})",
                    PrimitiveKind.Short => $"unchecked((ushort){angelScriptFuncParamSchema.Name})",
                    PrimitiveKind.UShort => angelScriptFuncParamSchema.Name,
                    PrimitiveKind.Int => $"unchecked((uint){angelScriptFuncParamSchema.Name})",
                    PrimitiveKind.UInt => angelScriptFuncParamSchema.Name,
                    PrimitiveKind.Long => $"unchecked((ulong){angelScriptFuncParamSchema.Name})",
                    PrimitiveKind.ULong => angelScriptFuncParamSchema.Name,
                    PrimitiveKind.Float => angelScriptFuncParamSchema.Name,
                    PrimitiveKind.Double => angelScriptFuncParamSchema.Name,
                    _ => throw new InvalidOperationException()
                };

                angelScriptFuncClassImplSrcBuilder.Append($"AsIScriptMainContext.Handle.{angelScriptFuncParamPrimitiveTypeSetArgNameSrc}({angelScriptFuncParamIndex}, {angelScriptFuncParamPrimitiveTypeSetArgCastSrc});");
                angelScriptFuncClassImplSrcBuilder.Append();
            }
            else if (angelScriptFuncParamSchema.Type is IStringTypeSchema angelScriptFuncParamStringTypeSchema)
            {
                if (!angelScriptFuncParamStringTypeSchema.IsStringOwned)
                {
                    throw new InvalidOperationException();
                }

                angelScriptFuncClassImplSrcBuilder.Append($"using var {angelScriptFuncParamSchema.Name}ManualMarshaler = new OwnedStringManualMarshaler({angelScriptFuncParamSchema.Name});");
                angelScriptFuncClassImplSrcBuilder.Append();

                angelScriptFuncClassImplSrcBuilder.Append($"fixed (nint* {angelScriptFuncParamSchema.Name}ManualMarshalerNativeValuePtr = &{angelScriptFuncParamSchema.Name}ManualMarshaler.NativeValue)");
                angelScriptFuncClassImplSrcBuilder.Append("{");
                angelScriptFuncClassImplSrcBuilder.Ident++;
                blockCount++;

                angelScriptFuncClassImplSrcBuilder.Append($"AsIScriptMainContext.Handle.SetArgAddress({angelScriptFuncParamIndex}, (nint){angelScriptFuncParamSchema.Name}ManualMarshalerNativeValuePtr);");
            }
            else if (angelScriptFuncParamSchema.Type is IClassTypeSchema angelScriptFuncParamClassTypeSchema)
            {
                var angelScriptFuncParamClassTypeSrc = GetTypeSrc.Execute(angelScriptFuncParamClassTypeSchema);

                angelScriptFuncClassImplSrcBuilder.Append($"using var {angelScriptFuncParamSchema.Name}ManualMarshaler = new StructManualMarshaler<{angelScriptFuncParamClassTypeSrc}>({angelScriptFuncParamSchema.Name});");
                angelScriptFuncClassImplSrcBuilder.Append();

                angelScriptFuncClassImplSrcBuilder.Append($"AsIScriptMainContext.Handle.SetArgObject({angelScriptFuncParamIndex}, {angelScriptFuncParamSchema.Name}ManualMarshaler.NativeValuePtr);");
                angelScriptFuncClassImplSrcBuilder.Append();
            }
            else if (angelScriptFuncParamSchema.Type is IClassHandleTypeSchema)
            {
                angelScriptFuncClassImplSrcBuilder.Append($"AsIScriptMainContext.Handle.SetArgObject({angelScriptFuncParamIndex}, (nint){angelScriptFuncParamSchema.Name});");
                angelScriptFuncClassImplSrcBuilder.Append();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}