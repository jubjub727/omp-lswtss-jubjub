using System.IO;
using Newtonsoft.Json.Linq;

namespace OMP.LSWTSS;

public static class BuildOverlay1
{
    public static void Execute()
    {
        var overlay1DistDirPath = GetOverlay1DistDirPath.Execute();

        if (Directory.Exists(overlay1DistDirPath))
        {
            Directory.Delete(overlay1DistDirPath, true);
        }

        Directory.CreateDirectory(overlay1DistDirPath);

        BuildOverlay1DotnetPackage.Execute();

        CopyDirectory.IO.CopyDirectory(
            Path.Combine(
                GetOverlay1DotnetPackageDirPath.Execute(),
                "bin",
                "Release",
                "net8.0",
                "win-x64"
            ),
            overlay1DistDirPath
        );

        File.Delete(
            Path.Combine(
                overlay1DistDirPath,
                "omp-lswtss-overlay1.deps.json"
            )
        );

        File.Delete(
            Path.Combine(
                overlay1DistDirPath,
                "omp-lswtss-overlay1.pdb"
            )
        );

        File.Delete(
            Path.Combine(
                overlay1DistDirPath,
                "omp-lswtss-overlay1.runtimeconfig.json"
            )
        );

        File.WriteAllText(
            Path.Combine(
                overlay1DistDirPath,
                "mod.json"
            ),
            new JObject
            {
                ["name"] = "Overlay1",
                ["actions"] = new JArray
                {
                    new JObject
                    {
                        ["typeId"] = "register-shared-assembly-action",
                        ["payload"] = new JObject
                        {
                            ["name"] = "omp-lswtss-overlay1",
                            ["path"] = "omp-lswtss-overlay1.dll",
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
                        ["id"] = "input-hook1",
                    },
                },
            }.ToString(Newtonsoft.Json.Formatting.Indented)
        );
    }
}