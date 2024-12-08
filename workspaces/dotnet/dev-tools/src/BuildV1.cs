using System.IO;
using Newtonsoft.Json.Linq;

namespace OMP.LSWTSS;

public static class BuildV1
{
    public static void Execute()
    {
        var v1DistDirPath = GetV1DistDirPath.Execute();

        if (Directory.Exists(v1DistDirPath))
        {
            Directory.Delete(v1DistDirPath, true);
        }

        Directory.CreateDirectory(v1DistDirPath);

        var v1ManagerDotnetPackageDirPath = Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "v1-manager"
        );

        BuildDotnetPackage.Execute(v1ManagerDotnetPackageDirPath);

        CopyDirectory.IO.CopyDirectory(
            Path.Combine(
                v1ManagerDotnetPackageDirPath,
                "bin",
                "Release",
                "net8.0"
            ),
            v1DistDirPath
        );

        File.Delete(
            Path.Combine(
                v1DistDirPath,
                "omp-lswtss-v1-manager.deps.json"
            )
        );

        File.Delete(
            Path.Combine(
                v1DistDirPath,
                "omp-lswtss-v1.pdb"
            )
        );

        File.Delete(
            Path.Combine(
                v1DistDirPath,
                "omp-lswtss-v1-manager.pdb"
            )
        );

        File.Delete(
            Path.Combine(
                v1DistDirPath,
                "omp-lswtss-v1-manager.runtimeconfig.json"
            )
        );

        File.WriteAllText(
            Path.Combine(
                v1DistDirPath,
                "mod.json"
            ),
            new JObject
            {
                ["name"] = "V1",
                ["actions"] = new JArray
                {
                    new JObject
                    {
                        ["typeId"] = "register-shared-assembly-action",
                        ["payload"] = new JObject
                        {
                            ["name"] = "omp-lswtss-v1",
                            ["path"] = "omp-lswtss-v1.dll",
                        },
                    },
                    new JObject
                    {
                        ["typeId"] = "register-scripting-module-action",
                        ["payload"] = new JObject
                        {
                            ["typeName"] = "OMP.LSWTSS.V1Manager",
                            ["assemblyPath"] = "omp-lswtss-v1-manager.dll",
                        },
                    },
                    new JObject
                    {
                        ["typeId"] = "register-mod-action-type-action",
                        ["payload"] = new JObject
                        {
                            ["id"] = "v1/register-custom-resource-action",
                            ["executeModActionSharedAssemblyName"] = "omp-lswtss-v1",
                            ["executeModActionStaticTypeName"] = "OMP.LSWTSS.V1+ModRegisterCustomResourceAction",
                        },
                    },
                    new JObject
                    {
                        ["typeId"] = "register-mod-action-type-action",
                        ["payload"] = new JObject
                        {
                            ["id"] = "v1/register-custom-character-action",
                            ["executeModActionSharedAssemblyName"] = "omp-lswtss-v1",
                            ["executeModActionStaticTypeName"] = "OMP.LSWTSS.V1+ModRegisterCustomCharacterAction",
                        },
                    },
                },
                ["dependencies"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "c-func-hook1",
                    },
                    new JObject
                    {
                        ["id"] = "c-api1",
                    },
                },
            }.ToString(Newtonsoft.Json.Formatting.Indented)
        );
    }
}