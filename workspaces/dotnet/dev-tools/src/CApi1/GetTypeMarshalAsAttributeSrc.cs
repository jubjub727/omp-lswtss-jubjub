using System;

namespace OMP.LSWTSS.CApi1;

public static class GetTypeMarshalAsAttributeSrc
{
    public static string? Execute(ITypeSchema? typeSchema)
    {
        if (typeSchema == null)
        {
            return null;
        }

        if (typeSchema is IPrimitiveTypeSchema primitiveTypeSchema)
        {
            return primitiveTypeSchema.PrimitiveKind switch
            {
                PrimitiveKind.Bool => "System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I1)",
                _ => null,
            };
        }

        if (typeSchema is IStringTypeSchema stringTypeSchema)
        {
            if (stringTypeSchema.IsStringOwned)
            {
                return "System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPUTF8Str)";
            }
            else
            {
                return "System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NotOwnedStringMarshaler))";
            }
        }

        if (typeSchema is IClassTypeSchema)
        {
            return null;
        }

        if (typeSchema is IClassHandleTypeSchema)
        {
            return null;
        }

        throw new InvalidOperationException();
    }
}