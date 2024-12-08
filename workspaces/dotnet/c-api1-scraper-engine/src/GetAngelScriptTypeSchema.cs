namespace OMP.LSWTSS.CApi1;

public static class GetAngelScriptTypeSchema
{
    public static ITypeSchema? Execute(AsIScriptEngine.Handle asIScriptEngineHandle, int asITypeId)
    {
        switch (asITypeId)
        {
            case 0: return null;
            case 1: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Bool };
            case 2: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Byte };
            case 3: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Short };
            case 4: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Int };
            case 5: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Long };
            case 6: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.UByte };
            case 7: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.UShort };
            case 8: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.UInt };
            case 9: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.ULong };
            case 10: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Float };
            case 11: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Double };
            case -1: return new PrimitiveTypeSchema { PrimitiveKind = PrimitiveKind.Pointer };
        }

        var asITypeInfoIsObjectTypeHandle = (asITypeId & 0x40000000) != 0;

        var asITypeInfoHandle = asIScriptEngineHandle.GetTypeInfoById(asITypeId);

        var className = asITypeInfoHandle.GetName();
        var classNamespace = asITypeInfoHandle.GetNamespace();

        if (string.IsNullOrEmpty(classNamespace))
        {
            classNamespace = null;
        }
        else
        {
            classNamespace = classNamespace.Replace("::", ".");
        }

        if (classNamespace == null)
        {
            if (className == "Array")
            {
                var asITypeSubTypeId = asITypeInfoHandle.GetSubTypeId(0);

                // TODO: Wrap it in array
                return Execute(asIScriptEngineHandle, asITypeSubTypeId);
            }
        }

        if (!asITypeInfoIsObjectTypeHandle)
        {
            return new ClassTypeSchema
            {
                ClassName = className,
                ClassNamespace = classNamespace,
            };
        }

        return new ClassHandleTypeSchema
        {
            ClassName = className,
            ClassNamespace = classNamespace,
        };
    }
}