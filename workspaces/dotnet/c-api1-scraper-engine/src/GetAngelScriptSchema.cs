using System;

namespace OMP.LSWTSS.CApi1;

public static class GetAngelScriptSchema
{
    public static Schema Execute(
        AsIScriptEngine.Handle asIScriptEngineHandle
    )
    {
        var angelScriptSchema = new Schema
        {
            Enums = [],
            GlobalFuncs = [],
            Classes = [],
        };

        for (uint asIEnumIndex = 0; asIEnumIndex < asIScriptEngineHandle.GetEnumCount(); asIEnumIndex++)
        {
            var asIEnumHandle = asIScriptEngineHandle.GetEnumByIndex(asIEnumIndex);

            angelScriptSchema.Enums.Add(
                GetAngelScriptEnumSchema.Execute(
                    asIEnumHandle
                )
            );
        }

        for (uint asIGlobalFunctionIndex = 0; asIGlobalFunctionIndex < asIScriptEngineHandle.GetGlobalFunctionCount(); asIGlobalFunctionIndex++)
        {
            var asIGlobalFunctionHandle = asIScriptEngineHandle.GetGlobalFunctionByIndex(asIGlobalFunctionIndex);

            var angelScriptGlobalFuncSchema = GetAngelScriptFuncSchema.Execute(
                angelScriptSchema,
                asIScriptEngineHandle,
                asIGlobalFunctionIndex,
                asIGlobalFunctionHandle,
                false
            );

            if (angelScriptGlobalFuncSchema != null)
            {
                angelScriptSchema.GlobalFuncs.Add(
                    angelScriptGlobalFuncSchema as IAngelScriptGlobalFuncSchema ?? throw new InvalidOperationException()
                );
            }
        }

        for (uint asIObjectTypeIndex = 0; asIObjectTypeIndex < asIScriptEngineHandle.GetObjectTypeCount(); asIObjectTypeIndex++)
        {
            var asIObjectTypeHandle = asIScriptEngineHandle.GetObjectTypeByIndex(asIObjectTypeIndex);

            var angelScriptClassSchema = GetAngelScriptClassSchema.Execute(
                angelScriptSchema,
                asIScriptEngineHandle,
                asIObjectTypeIndex,
                asIObjectTypeHandle
            );

            if (angelScriptClassSchema != null)
            {
                angelScriptSchema.Classes.Add(
                    angelScriptClassSchema
                );
            }
        }

        return angelScriptSchema;
    }
}