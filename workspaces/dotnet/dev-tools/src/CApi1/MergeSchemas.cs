using System.Collections.Generic;
using System.Linq;

namespace OMP.LSWTSS.CApi1;

public static class MergeSchemas
{
    public static Schema Execute(IEnumerable<Schema> schemas)
    {
        var finalSchema = new Schema
        {
            Enums = [],
            GlobalFuncs = [],
            Classes = [],
        };

        foreach (var schema in schemas)
        {
            foreach (var enumSchema in schema.Enums)
            {
                var enumFinalSchema = finalSchema.Enums.FirstOrDefault(x => x.Namespace == enumSchema.Namespace && x.Name == enumSchema.Name);

                if (enumFinalSchema == null)
                {
                    finalSchema.Enums.Add(enumSchema);
                    continue;
                }
            }

            foreach (var globalFuncSchema in schema.GlobalFuncs)
            {
                var globalFuncFinalSchema = finalSchema.GlobalFuncs.FirstOrDefault(x => x.Namespace == globalFuncSchema.Namespace && x.Name == globalFuncSchema.Name);

                if (globalFuncFinalSchema == null)
                {
                    finalSchema.GlobalFuncs.Add(globalFuncSchema);
                    continue;
                }

                if (
                    globalFuncSchema is IAngelScriptGlobalFuncSchema angelScriptGlobalFuncSchema
                    &&
                    globalFuncFinalSchema is IAngelScriptGlobalFuncSchema angelScriptGlobalFuncFinalSchema
                )
                {
                    if (angelScriptGlobalFuncFinalSchema.SteamIndex == null && angelScriptGlobalFuncSchema.SteamIndex != null)
                    {
                        angelScriptGlobalFuncFinalSchema.SteamIndex = angelScriptGlobalFuncSchema.SteamIndex;
                    }

                    if (angelScriptGlobalFuncFinalSchema.EGSIndex == null && angelScriptGlobalFuncSchema.EGSIndex != null)
                    {
                        angelScriptGlobalFuncFinalSchema.EGSIndex = angelScriptGlobalFuncSchema.EGSIndex;
                    }
                }

                if (
                    globalFuncSchema is IGlobalNativeFuncSchema globalNativeFuncSchema
                    &&
                    globalFuncFinalSchema is IGlobalNativeFuncSchema globalNativeFuncFinalSchema
                )
                {
                    if (globalNativeFuncFinalSchema.SteamOffset == null && globalNativeFuncSchema.SteamOffset != null)
                    {
                        globalNativeFuncFinalSchema.SteamOffset = globalNativeFuncSchema.SteamOffset;
                    }

                    if (globalNativeFuncFinalSchema.EGSOffset == null && globalNativeFuncSchema.EGSOffset != null)
                    {
                        globalNativeFuncFinalSchema.EGSOffset = globalNativeFuncSchema.EGSOffset;
                    }
                }
            }

            foreach (var classSchema in schema.Classes)
            {
                var classFinalSchema = finalSchema.Classes.FirstOrDefault(x => x.Namespace == classSchema.Namespace && x.Name == classSchema.Name);

                if (classFinalSchema == null)
                {
                    finalSchema.Classes.Add(classSchema);
                    continue;
                }

                if (classFinalSchema.StructFields == null && classSchema.StructFields != null)
                {
                    classFinalSchema.StructFields = classSchema.StructFields;
                }

                if (classFinalSchema.Size == null && classSchema.Size != null)
                {
                    classFinalSchema.Size = classSchema.Size;
                }

                if (
                    classSchema is IAngelScriptClassSchema angelScriptClassSchema
                    &&
                    classFinalSchema is IAngelScriptClassSchema angelScriptClassFinalSchema
                )
                {
                    if (angelScriptClassFinalSchema.SteamIndex == null && angelScriptClassSchema.SteamIndex != null)
                    {
                        angelScriptClassFinalSchema.SteamIndex = angelScriptClassSchema.SteamIndex;
                    }

                    if (angelScriptClassFinalSchema.EGSIndex == null && angelScriptClassSchema.EGSIndex != null)
                    {
                        angelScriptClassFinalSchema.EGSIndex = angelScriptClassSchema.EGSIndex;
                    }

                    foreach (var angelScriptClassMethodSchema in angelScriptClassSchema.Methods)
                    {
                        var angelScriptClassMethodFinalSchema = angelScriptClassFinalSchema.Methods.FirstOrDefault(x => x.Name == angelScriptClassMethodSchema.Name);

                        if (angelScriptClassMethodFinalSchema == null)
                        {
                            angelScriptClassFinalSchema.Methods.Add(angelScriptClassMethodSchema);
                            continue;
                        }

                        if (angelScriptClassMethodFinalSchema.SteamIndex == null && angelScriptClassMethodSchema.SteamIndex != null)
                        {
                            angelScriptClassMethodFinalSchema.SteamIndex = angelScriptClassMethodSchema.SteamIndex;
                        }

                        if (angelScriptClassMethodFinalSchema.EGSIndex == null && angelScriptClassMethodSchema.EGSIndex != null)
                        {
                            angelScriptClassMethodFinalSchema.EGSIndex = angelScriptClassMethodSchema.EGSIndex;
                        }

                        if (
                            angelScriptClassMethodSchema is IAngelScriptClassNativeMethodSchema angelScriptClassNativeMethodSchema
                            &&
                            angelScriptClassMethodFinalSchema is IAngelScriptClassNativeMethodSchema angelScriptClassNativeMethodFinalSchema
                        )
                        {
                            if (angelScriptClassNativeMethodFinalSchema.SteamOffset == null && angelScriptClassNativeMethodSchema.SteamOffset != null)
                            {
                                angelScriptClassNativeMethodFinalSchema.SteamOffset = angelScriptClassNativeMethodSchema.SteamOffset;
                            }

                            if (angelScriptClassNativeMethodFinalSchema.EGSOffset == null && angelScriptClassNativeMethodSchema.EGSOffset != null)
                            {
                                angelScriptClassNativeMethodFinalSchema.EGSOffset = angelScriptClassNativeMethodSchema.EGSOffset;
                            }
                        }
                    }
                }

                if (
                    classSchema is INativeClassSchema nativeClassSchema
                    &&
                    classFinalSchema is INativeClassSchema nativeClassFinalSchema
                )
                {
                    foreach (var nativeClassMethodSchema in nativeClassSchema.Methods)
                    {
                        var nativeClassMethodFinalSchema = nativeClassFinalSchema.Methods.FirstOrDefault(x => x.Name == nativeClassMethodSchema.Name);

                        if (nativeClassMethodFinalSchema == null)
                        {
                            nativeClassFinalSchema.Methods.Add(nativeClassMethodSchema);
                            continue;
                        }

                        if (
                            nativeClassMethodSchema is INativeClassNativeMethodSchema nativeClassNativeMethodSchema
                            &&
                            nativeClassMethodFinalSchema is INativeClassNativeMethodSchema nativeClassNativeMethodFinalSchema
                        )
                        {
                            if (nativeClassNativeMethodFinalSchema.SteamOffset == null && nativeClassNativeMethodSchema.SteamOffset != null)
                            {
                                nativeClassNativeMethodFinalSchema.SteamOffset = nativeClassNativeMethodSchema.SteamOffset;
                            }

                            if (nativeClassNativeMethodFinalSchema.EGSOffset == null && nativeClassNativeMethodSchema.EGSOffset != null)
                            {
                                nativeClassNativeMethodFinalSchema.EGSOffset = nativeClassNativeMethodSchema.EGSOffset;
                            }
                        }
                    }
                }

                foreach (var classFieldSchema in classSchema.Fields)
                {
                    var classFieldFinalSchema = classFinalSchema.Fields.FirstOrDefault(x => x.Name == classFieldSchema.Name);

                    if (classFieldFinalSchema == null)
                    {
                        classFinalSchema.Fields.Add(classFieldSchema);
                        continue;
                    }

                    if (classFieldFinalSchema.SteamOffset == null && classFieldSchema.SteamOffset != null)
                    {
                        classFieldFinalSchema.SteamOffset = classFieldSchema.SteamOffset;
                    }

                    if (classFieldFinalSchema.EGSOffset == null && classFieldSchema.EGSOffset != null)
                    {
                        classFieldFinalSchema.EGSOffset = classFieldSchema.EGSOffset;
                    }
                }
            }
        }

        return finalSchema;
    }
}