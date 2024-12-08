using System.IO;
using Newtonsoft.Json.Linq;

namespace OMP.LSWTSS;

public static class BuildCFuncHook1
{
    public static void Execute()
    {
        var cFuncHook1DistDirPath = GetCFuncHook1DistDirPath.Execute();

        if (Directory.Exists(cFuncHook1DistDirPath))
        {
            Directory.Delete(cFuncHook1DistDirPath, true);
        }

        Directory.CreateDirectory(cFuncHook1DistDirPath);

        BuildCFuncHook1NativeRustPackage.Execute();
        BuildCFuncHook1DotnetPackage.Execute();

        File.Copy(
            Path.Combine(
                GetRustWorkspaceDirPath.Execute(),
                "target",
                "release",
                "omp_lswtss_c_func_hook1_native.dll"
            ),
            Path.Combine(
                cFuncHook1DistDirPath,
                "omp-lswtss-c-func-hook1-native.dll"
            )
        );

        CopyDirectory.IO.CopyDirectory(
            Path.Combine(
                GetCFuncHook1DotnetPackageDirPath.Execute(),
                "bin",
                "Release",
                "net8.0"
            ),
            cFuncHook1DistDirPath
        );

        File.Delete(
            Path.Combine(
                cFuncHook1DistDirPath,
                "omp-lswtss-c-func-hook1.deps.json"
            )
        );

        File.Delete(
            Path.Combine(
                cFuncHook1DistDirPath,
                "omp-lswtss-c-func-hook1.pdb"
            )
        );

        File.Delete(
            Path.Combine(
                cFuncHook1DistDirPath,
                "omp-lswtss-c-func-hook1.runtimeconfig.json"
            )
        );

        File.WriteAllText(
            Path.Combine(
                cFuncHook1DistDirPath,
                "mod.json"
            ),
            new JObject
            {
                ["name"] = "CFuncHook1",
                ["actions"] = new JArray
                {
                    new JObject
                    {
                        ["typeId"] = "register-shared-assembly-action",
                        ["payload"] = new JObject
                        {
                            ["name"] = "omp-lswtss-c-func-hook1",
                            ["path"] = "omp-lswtss-c-func-hook1.dll",
                        },
                    },
                },
            }.ToString(Newtonsoft.Json.Formatting.Indented)
        );
    }
}