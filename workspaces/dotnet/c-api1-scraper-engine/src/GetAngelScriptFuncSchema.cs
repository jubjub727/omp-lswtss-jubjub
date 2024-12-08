using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public static class GetAngelScriptFuncSchema
{
    public static IAngelScriptFuncSchema? Execute(
        Schema schema,
        AsIScriptEngine.Handle asIScriptEngineHandle,
        uint asIScriptFunctionIndex,
        AsIScriptFunction.Handle asIScriptFunctionHandle,
        bool isAngelScriptClassMethod
    )
    {
        var variant = GetVariant.Execute();

        var asIScriptFunctionSteamIndex = (uint?)null;
        var asIScriptFunctionEGSIndex = (uint?)null;

        if (variant == Variant.Steam)
        {
            asIScriptFunctionSteamIndex = asIScriptFunctionIndex;
        }
        else if (variant == Variant.EGS)
        {
            asIScriptFunctionEGSIndex = asIScriptFunctionIndex;
        }

        var angelScriptFuncSchemaName = asIScriptFunctionHandle.GetName();

        var angelScriptFuncSchemaNamespace = asIScriptFunctionHandle.GetNamespace();

        if (angelScriptFuncSchemaNamespace == string.Empty)
        {
            angelScriptFuncSchemaNamespace = null;
        }
        else
        {
            angelScriptFuncSchemaNamespace = angelScriptFuncSchemaNamespace.Replace("::", ".");
        }

        if (isAngelScriptClassMethod)
        {
            if (
                angelScriptFuncSchemaName.StartsWith("op")
                &&
                angelScriptFuncSchemaName != "opImplCast"
            )
            {
                return null;
            }

            if (angelScriptFuncSchemaName == "GetType")
            {
                angelScriptFuncSchemaName = "GetType_";
            }
        }
        else
        {
            if (angelScriptFuncSchemaNamespace == null)
            {
                if (
                    angelScriptFuncSchemaName == "_GetBehaviourCppData"
                    ||
                    angelScriptFuncSchemaName == "parseInt"
                    ||
                    angelScriptFuncSchemaName == "parseUInt"
                )
                {
                    return null;
                }
            }
        }

        var asIScriptFunctionReturnTypeId = asIScriptFunctionHandle.GetReturnTypeId(out var asIScriptFunctionReturnTypeFlags);

        var angelScriptFuncReturnTypeSchema = GetAngelScriptTypeSchema.Execute(
            asIScriptEngineHandle,
            asIScriptFunctionReturnTypeId
        );

        var angelScriptFuncReturnClassTypeSchema = angelScriptFuncReturnTypeSchema as IClassTypeSchema;

        if ((asIScriptFunctionReturnTypeFlags & 1) != 0 || (asIScriptFunctionReturnTypeFlags & 2) != 0)
        {
            if (angelScriptFuncReturnClassTypeSchema == null)
            {
                throw new InvalidOperationException();
            }

            angelScriptFuncReturnTypeSchema = new ClassHandleTypeSchema
            {
                ClassName = angelScriptFuncReturnClassTypeSchema.ClassName,
                ClassNamespace = angelScriptFuncReturnClassTypeSchema.ClassNamespace,
            };
        }

        if (angelScriptFuncReturnClassTypeSchema != null)
        {
            if (angelScriptFuncReturnClassTypeSchema.ClassName == "NuDynamicString")
            {
                angelScriptFuncReturnTypeSchema = new StringTypeSchema
                {
                    IsStringNullable = false,
                    IsStringOwned = false,
                };
            }
        }

        IAngelScriptFuncSchema angelScriptFuncSchema;

        var asIScriptFunctionUserData0Handle = asIScriptFunctionHandle.GetUserData(0);

        if (asIScriptFunctionUserData0Handle != nint.Zero)
        {
            var angelScriptFuncOffset = NativeFunc.GetOffset(Marshal.ReadIntPtr(nint.Add(asIScriptFunctionUserData0Handle, 0x70)));

            var angelScriptFuncSteamOffset = (uint?)null;
            var angelScriptFuncEGSOffset = (uint?)null;

            if (variant == Variant.Steam)
            {
                angelScriptFuncSteamOffset = angelScriptFuncOffset;
            }
            else if (variant == Variant.EGS)
            {
                angelScriptFuncEGSOffset = angelScriptFuncOffset;
            }

            if (isAngelScriptClassMethod)
            {
                angelScriptFuncSchema = new AngelScriptClassNativeMethodSchema
                {
                    Name = angelScriptFuncSchemaName,
                    SteamIndex = asIScriptFunctionSteamIndex,
                    EGSIndex = asIScriptFunctionEGSIndex,
                    SteamOffset = angelScriptFuncSteamOffset,
                    EGSOffset = angelScriptFuncEGSOffset,
                    Params = [],
                    ReturnType = angelScriptFuncReturnTypeSchema,
                };
            }
            else
            {
                angelScriptFuncSchema = new AngelScriptGlobalNativeFuncSchema
                {
                    Name = angelScriptFuncSchemaName,
                    Namespace = angelScriptFuncSchemaNamespace,
                    SteamIndex = asIScriptFunctionSteamIndex,
                    EGSIndex = asIScriptFunctionEGSIndex,
                    SteamOffset = angelScriptFuncSteamOffset,
                    EGSOffset = angelScriptFuncEGSOffset,
                    Params = [],
                    ReturnType = angelScriptFuncReturnTypeSchema,
                };
            }
        }
        else
        {
            if (isAngelScriptClassMethod)
            {
                angelScriptFuncSchema = new AngelScriptClassMethodSchema
                {
                    Name = angelScriptFuncSchemaName,
                    SteamIndex = asIScriptFunctionSteamIndex,
                    EGSIndex = asIScriptFunctionEGSIndex,
                    Params = [],
                    ReturnType = angelScriptFuncReturnTypeSchema,
                };
            }
            else
            {
                angelScriptFuncSchema = new AngelScriptGlobalFuncSchema
                {
                    Name = angelScriptFuncSchemaName,
                    Namespace = angelScriptFuncSchemaNamespace,
                    SteamIndex = asIScriptFunctionSteamIndex,
                    EGSIndex = asIScriptFunctionEGSIndex,
                    Params = [],
                    ReturnType = angelScriptFuncReturnTypeSchema,
                };
            }
        }

        for (uint asIScriptFunctionParamIndex = 0; asIScriptFunctionParamIndex < asIScriptFunctionHandle.GetParamCount(); asIScriptFunctionParamIndex++)
        {
            asIScriptFunctionHandle.GetParam(
                asIScriptFunctionParamIndex,
                out var asIScriptFunctionParamTypeId,
                out var asIScriptFunctionParamFlags,
                out var asIScriptFunctionParamName,
                out _
            );

            var angelScriptFuncParamType = GetAngelScriptTypeSchema.Execute(asIScriptEngineHandle, asIScriptFunctionParamTypeId) ?? throw new InvalidOperationException();

            var isAngelScriptFuncParamInRef = (asIScriptFunctionParamFlags & 1) != 0;
            var isAngelScriptFuncParamOutRef = (asIScriptFunctionParamFlags & 2) != 0;

            if (angelScriptFuncParamType is IClassTypeSchema angelScriptFuncParamClassTypeSchema)
            {
                if (angelScriptFuncParamClassTypeSchema.ClassName == "NuDynamicString")
                {
                    angelScriptFuncParamType = new StringTypeSchema
                    {
                        IsStringNullable = false,
                        IsStringOwned = true,
                    };

                    if (!isAngelScriptFuncParamInRef || isAngelScriptFuncParamOutRef)
                    {
                        throw new InvalidOperationException();
                    }

                    isAngelScriptFuncParamInRef = false;
                }
                else if (isAngelScriptFuncParamInRef || isAngelScriptFuncParamOutRef)
                {
                    if (!schema.Enums.Any((e) => e.Name == angelScriptFuncParamClassTypeSchema.ClassName && e.Namespace == angelScriptFuncParamClassTypeSchema.ClassNamespace))
                    {
                        angelScriptFuncParamType = new ClassHandleTypeSchema
                        {
                            ClassName = angelScriptFuncParamClassTypeSchema.ClassName,
                            ClassNamespace = angelScriptFuncParamClassTypeSchema.ClassNamespace,
                        };
                        isAngelScriptFuncParamInRef = false;
                        isAngelScriptFuncParamOutRef = false;
                    }
                }
            }

            angelScriptFuncSchema.Params.Add(
                new FuncParamSchema
                {
                    Name = string.IsNullOrEmpty(asIScriptFunctionParamName) ? $"param{asIScriptFunctionParamIndex}" : asIScriptFunctionParamName,
                    IsInRef = isAngelScriptFuncParamInRef,
                    IsOutRef = isAngelScriptFuncParamOutRef,
                    Type = angelScriptFuncParamType,
                }
            );
        }

        return angelScriptFuncSchema;
    }
}