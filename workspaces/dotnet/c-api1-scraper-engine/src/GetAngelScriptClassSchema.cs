using System;
using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public static class GetAngelScriptClassSchema
{
    public static IAngelScriptClassSchema? Execute(
        Schema schema,
        AsIScriptEngine.Handle asIScriptEngineHandle,
        uint asIObjectTypeIndex,
        AsITypeInfo.Handle asIObjectTypeHandle
    )
    {
        var variant = GetVariant.Execute();

        var asIObjectTypeSteamIndex = (uint?)null;
        var asIObjectTypeEGSIndex = (uint?)null;

        if (variant == Variant.Steam)
        {
            asIObjectTypeSteamIndex = asIObjectTypeIndex;
        }
        else if (variant == Variant.EGS)
        {
            asIObjectTypeEGSIndex = asIObjectTypeIndex;
        }

        var angelScriptClassName = asIObjectTypeHandle.GetName();

        var angelScriptClassNamespace = asIObjectTypeHandle.GetNamespace();

        if (angelScriptClassNamespace == string.Empty)
        {
            angelScriptClassNamespace = null;
        }
        else
        {
            angelScriptClassNamespace = angelScriptClassNamespace.Replace("::", ".");
        }

        if (angelScriptClassNamespace == null)
        {
            if (
                angelScriptClassName == "array"
                ||
                angelScriptClassName == "Array"
                ||
                angelScriptClassName == "dictionary"
                ||
                angelScriptClassName == "dictionaryValue"
                ||
                angelScriptClassName == "_AngelScriptBehaviour"
                ||
                angelScriptClassName == "_AngelScriptComponent"
                ||
                angelScriptClassName == "ManagedAction"
                ||
                angelScriptClassName == "ManagedComponent"
                ||
                angelScriptClassName == "weakref"
                ||
                angelScriptClassName == "const_weakref"
            )
            {
                return null;
            }
        }

        var angelScriptClassSize = (uint?)asIObjectTypeHandle.GetSize();

        if (angelScriptClassSize == 0)
        {
            angelScriptClassSize = null;
        }

        var angelScriptClassSchema = new AngelScriptClassSchema
        {
            SteamIndex = asIObjectTypeSteamIndex,
            EGSIndex = asIObjectTypeEGSIndex,
            Name = angelScriptClassName,
            Namespace = angelScriptClassNamespace,
            ParentClassTypes = [],
            Methods = [],
            Fields = [],
            StructFields = null,
            Size = angelScriptClassSize,
        };

        var angelScriptClassMethodNamesCounter = new Dictionary<string, int>();

        for (uint asIObjectTypeMethodIndex = 0; asIObjectTypeMethodIndex < asIObjectTypeHandle.GetMethodCount(); asIObjectTypeMethodIndex++)
        {
            var asIObjectTypeMethodHandle = asIObjectTypeHandle.GetMethodByIndex(asIObjectTypeMethodIndex, true);

            var angelScriptClassMethodSchema = GetAngelScriptFuncSchema.Execute(
                schema,
                asIScriptEngineHandle,
                asIObjectTypeMethodIndex,
                asIObjectTypeMethodHandle,
                true
            ) as IAngelScriptClassMethodSchema;

            if (angelScriptClassMethodSchema != null)
            {
                if (angelScriptClassMethodSchema.Name == "opImplCast")
                {
                    var angelScriptClassMethodReturnTypeSchema = angelScriptClassMethodSchema.ReturnType as IClassHandleTypeSchema ?? throw new InvalidOperationException();

                    angelScriptClassSchema.ParentClassTypes.Add(
                        new ClassTypeSchema
                        {
                            ClassName = angelScriptClassMethodReturnTypeSchema.ClassName,
                            ClassNamespace = angelScriptClassMethodReturnTypeSchema.ClassNamespace,
                        }
                    );
                }
                else
                {
                    if (angelScriptClassMethodNamesCounter.TryGetValue(angelScriptClassMethodSchema.Name, out int angelScriptClassMethodNameCounter))
                    {
                        angelScriptClassMethodNameCounter += 1;
                        angelScriptClassMethodSchema.Name += angelScriptClassMethodNameCounter;
                        angelScriptClassMethodNamesCounter[angelScriptClassMethodSchema.Name] = angelScriptClassMethodNameCounter;
                    }
                    else
                    {
                        angelScriptClassMethodNamesCounter.Add(angelScriptClassMethodSchema.Name, 1);
                    }

                    angelScriptClassSchema.Methods.Add(angelScriptClassMethodSchema);
                }
            }
        }

        return angelScriptClassSchema;
    }
}