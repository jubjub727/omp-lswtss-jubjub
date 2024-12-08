using System.IO;
using Newtonsoft.Json.Linq;

namespace OMP.LSWTSS;

public static class BuildDebugTools
{
    public static void Execute()
    {
        var debugToolsDistDirPath = GetDebugToolsDistDirPath.Execute();

        if (Directory.Exists(debugToolsDistDirPath))
        {
            Directory.Delete(debugToolsDistDirPath, true);
        }

        Directory.CreateDirectory(debugToolsDistDirPath);

        var debugToolsDotnetPackageDirPath = Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "debug-tools"
        );

        BuildDotnetPackage.Execute(debugToolsDotnetPackageDirPath);

        CopyDirectory.IO.CopyDirectory(
            Path.Combine(
                debugToolsDotnetPackageDirPath,
                "bin",
                "Release",
                "net8.0"
            ),
            debugToolsDistDirPath
        );

        File.Delete(
            Path.Combine(
                debugToolsDistDirPath,
                "omp-lswtss-debug-tools.deps.json"
            )
        );

        File.Delete(
            Path.Combine(
                debugToolsDistDirPath,
                "omp-lswtss-debug-tools.pdb"
            )
        );

        File.Delete(
            Path.Combine(
                debugToolsDistDirPath,
                "omp-lswtss-debug-tools.runtimeconfig.json"
            )
        );

        File.WriteAllText(
            Path.Combine(
                debugToolsDistDirPath,
                "mod.json"
            ),
            new JObject
            {
                ["name"] = "Debug Tools",
                ["actions"] = new JArray
                {
                    new JObject
                    {
                        ["typeId"] = "register-scripting-module-action",
                        ["payload"] = new JObject
                        {
                            ["typeName"] = "OMP.LSWTSS.DebugTools",
                            ["assemblyPath"] = "lswtss-omp-debug-tools.dll",
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