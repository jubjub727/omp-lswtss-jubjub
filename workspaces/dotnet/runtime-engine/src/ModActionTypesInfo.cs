using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace OMP.LSWTSS;

public static partial class RuntimeEngine
{
    static readonly List<ModActionTypeInfo> _modActionTypesInfo = [
        new ModActionTypeInfo
        {
            Id = "register-mod-action-type-action",
            ExecuteModAction = (modId, modDirPath, modActionPayloadAsJson) =>
            {
                var modRegisterModActionTypeActionPayload = JsonConvert.DeserializeObject<ModRegisterModActionTypeActionPayload>(
                    modActionPayloadAsJson
                ) ?? throw Crash($"Failed to deserialize register-mod-action-type-action action payload from {modId}");

                var executeModActionSharedAssemblyInfo = _sharedAssembliesInfo!.Find(
                    (sharedAssemblyInfo) => sharedAssemblyInfo.Name == modRegisterModActionTypeActionPayload.ExecuteModActionSharedAssemblyName
                ) ?? throw Crash($"Failed to find shared assembly {modRegisterModActionTypeActionPayload.ExecuteModActionSharedAssemblyName} for register-mod-action-type-action from {modId}");

                var executeModActionAssembly = Assembly.LoadFrom(
                    executeModActionSharedAssemblyInfo.Path
                );

                var executeModActionStaticType = executeModActionAssembly.GetType(
                    modRegisterModActionTypeActionPayload.ExecuteModActionStaticTypeName
                ) ?? throw Crash($"Failed to find static type {modRegisterModActionTypeActionPayload.ExecuteModActionStaticTypeName} for register-mod-action-type-action from {modId}");

                var executeModActionInfo = executeModActionStaticType.GetMethod(
                    "Execute"
                ) ?? throw Crash($"Failed to find {modRegisterModActionTypeActionPayload.ExecuteModActionStaticTypeName}.Execute for register-mod-action-type-action from {modId}");

                var executeModAction = (ExecuteModActionDelegate)Delegate.CreateDelegate(
                    typeof(ExecuteModActionDelegate),
                    executeModActionInfo
                );

                _modActionTypesInfo!.Add(
                    new ModActionTypeInfo
                    {
                        Id = modRegisterModActionTypeActionPayload.Id,
                        ExecuteModAction = executeModAction
                    }
                );
            }
        },
        new ModActionTypeInfo
        {
            Id = "register-scripting-module-action",
            ExecuteModAction = (modId, modDirPath, modActionPayloadAsJson) =>
            {
                var modRegisterScriptingModuleActionPayload = JsonConvert.DeserializeObject<ModRegisterScriptingModuleActionPayload>(
                    modActionPayloadAsJson
                ) ?? throw new InvalidOperationException();

                _scriptingModuleContexts!.Add(
                    new ScriptingModuleContext
                    {
                        ScriptingModuleInfo = new ScriptingModuleInfo
                        {
                            TypeName = modRegisterScriptingModuleActionPayload.TypeName,
                            AssemblyPath = Path.Combine(
                                modDirPath,
                                modRegisterScriptingModuleActionPayload.AssemblyPath
                            ),
                        },
                    }
                );
            }
        },
        new ModActionTypeInfo
        {
            Id = "register-shared-assembly-action",
            ExecuteModAction = (modId, modDirPath, modActionPayloadAsJson) =>
            {
                var modRegisterSharedAssemblyActionPayload = JsonConvert.DeserializeObject<ModRegisterSharedAssemblyActionPayload>(
                    modActionPayloadAsJson
                ) ?? throw new InvalidOperationException();

                _sharedAssembliesInfo!.Add(
                    new SharedAssemblyInfo
                    {
                        Name = modRegisterSharedAssemblyActionPayload.Name,
                        Path = Path.Combine(
                            modDirPath,
                            modRegisterSharedAssemblyActionPayload.Path
                        ),
                    }
                );
            }
        }
    ];
}