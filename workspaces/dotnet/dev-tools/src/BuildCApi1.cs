using System.IO;
using Newtonsoft.Json.Linq;

namespace OMP.LSWTSS;

public static class BuildCApi1
{
    public static void Execute(string steamGameDirPath, string egsGameDirPath)
    {
        var cApi1DistDirPath = GetCApi1DistDirPath.Execute();

        if (Directory.Exists(cApi1DistDirPath))
        {
            Directory.Delete(cApi1DistDirPath, true);
        }

        Directory.CreateDirectory(cApi1DistDirPath);

        var cApi1ManagerDotnetPackageDirPath = Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "c-api1-manager"
        );

        BuildCApi1MainDotnetPackage.Execute(steamGameDirPath, egsGameDirPath);

        BuildDotnetPackage.Execute(cApi1ManagerDotnetPackageDirPath);

        File.Copy(
            Path.Combine(
                GetCApi1MainDotnetPackageDirPath.Execute(),
                "bin",
                "Release",
                "net8.0",
                "omp-lswtss-c-api1-core.dll"
            ),
            Path.Combine(
                cApi1DistDirPath,
                "omp-lswtss-c-api1-core.dll"
            )
        );

        File.Copy(
            Path.Combine(
                GetCApi1MainDotnetPackageDirPath.Execute(),
                "bin",
                "Release",
                "net8.0",
                "omp-lswtss-c-api1-main.dll"
            ),
            Path.Combine(
                cApi1DistDirPath,
                "omp-lswtss-c-api1-main.dll"
            )
        );

        File.Copy(
            Path.Combine(
                cApi1ManagerDotnetPackageDirPath,
                "bin",
                "Release",
                "net8.0",
                "omp-lswtss-c-api1-manager.dll"
            ),
            Path.Combine(
                cApi1DistDirPath,
                "omp-lswtss-c-api1-manager.dll"
            )
        );

        File.WriteAllText(
            Path.Combine(
                cApi1DistDirPath,
                "mod.json"
            ),
            new JObject
            {
                ["name"] = "CApi1",
                ["actions"] = new JArray
                {
                    new JObject
                    {
                        ["typeId"] = "register-shared-assembly-action",
                        ["payload"] = new JObject
                        {
                            ["name"] = "omp-lswtss-c-api1-core",
                            ["path"] = "omp-lswtss-c-api1-core.dll",
                        },
                    },
                    new JObject
                    {
                        ["typeId"] = "register-shared-assembly-action",
                        ["payload"] = new JObject
                        {
                            ["name"] = "omp-lswtss-c-api1-main",
                            ["path"] = "omp-lswtss-c-api1-main.dll",
                        },
                    },
                    new JObject
                    {
                        ["typeId"] = "register-scripting-module-action",
                        ["payload"] = new JObject
                        {
                            ["typeName"] = "OMP.LSWTSS.CApi1.Manager",
                            ["assemblyPath"] = "omp-lswtss-c-api1-manager.dll",
                        },
                    },
                },
            }.ToString(Newtonsoft.Json.Formatting.Indented)
        );
    }
}