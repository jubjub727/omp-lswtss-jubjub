using System;

namespace OMP.LSWTSS.CApi1;

public static class GetTypeSrc
{
    public static string Execute(ITypeSchema? typeSchema)
    {
        if (typeSchema == null)
        {
            return "void";
        }

        if (typeSchema is IPrimitiveTypeSchema primitiveTypeSchema)
        {
            switch (primitiveTypeSchema.PrimitiveKind)
            {
                case PrimitiveKind.Pointer:
                    return "nint";
                case PrimitiveKind.Bool:
                    return "bool";
                case PrimitiveKind.Byte:
                    return "sbyte";
                case PrimitiveKind.UByte:
                    return "byte";
                case PrimitiveKind.Short:
                    return "short";
                case PrimitiveKind.UShort:
                    return "ushort";
                case PrimitiveKind.Int:
                    return "int";
                case PrimitiveKind.UInt:
                    return "uint";
                case PrimitiveKind.Long:
                    return "long";
                case PrimitiveKind.ULong:
                    return "ulong";
                case PrimitiveKind.Float:
                    return "float";
                case PrimitiveKind.Double:
                    return "double";
            }
        }

        if (typeSchema is IStringTypeSchema stringTypeSchema)
        {
            return stringTypeSchema.IsStringNullable ? "string?" : "string";
        }

        if (typeSchema is IClassTypeSchema classTypeSchema)
        {
            if (classTypeSchema.ClassNamespace != null)
            {
                return $"OMP.LSWTSS.CApi1.{classTypeSchema.ClassNamespace}.{classTypeSchema.ClassName}";
            }

            return $"OMP.LSWTSS.CApi1.{classTypeSchema.ClassName}";
        }

        if (typeSchema is IClassHandleTypeSchema classHandleTypeSchema)
        {
            if (classHandleTypeSchema.ClassNamespace != null)
            {
                return $"OMP.LSWTSS.CApi1.{classHandleTypeSchema.ClassNamespace}.{classHandleTypeSchema.ClassName}.Handle";
            }

            return $"OMP.LSWTSS.CApi1.{classHandleTypeSchema.ClassName}.Handle";
        }

        throw new InvalidOperationException();
    }
}