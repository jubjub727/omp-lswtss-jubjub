using System.IO;
using Newtonsoft.Json.Linq;

namespace OMP.LSWTSS;

public static class BuildTestCefMod
{
    public static void Execute()
    {
        var testCefModDistDirPath = GetTestCefModDistDirPath.Execute();

        if (Directory.Exists(testCefModDistDirPath))
        {
            Directory.Delete(testCefModDistDirPath, true);
        }

        Directory.CreateDirectory(testCefModDistDirPath);

        var testCefModDotnetPackageDirPath = Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "test-cef-mod"
        );

        var testCefModOverlayJsPackageDirPath = Path.Combine(
            GetJsWorkspaceDirPath.Execute(),
            "test-cef-mod-overlay"
        );

        BuildDotnetPackage.Execute(testCefModDotnetPackageDirPath);
        BuildJsPackage.Execute(testCefModOverlayJsPackageDirPath);

        CopyDirectory.IO.CopyDirectory(
            Path.Combine(
                testCefModDotnetPackageDirPath,
                "bin",
                "Release",
                "net8.0"
            ),
            testCefModDistDirPath
        );

        File.Copy(
            Path.Combine(
                testCefModOverlayJsPackageDirPath,
                "dist",
                "index.html"
            ),
            Path.Combine(
                testCefModDistDirPath,
                "index.html"
            )
        );

        File.WriteAllText(
            Path.Combine(
                testCefModDistDirPath,
                "mod.json"
            ),
            new JObject
            {
                ["name"] = "Test Cef Mod",
                ["actions"] = new JArray
                {
                    new JObject
                    {
                        ["typeId"] = "register-scripting-module-action",
                        ["payload"] = new JObject
                        {
                            ["typeName"] = "OMP.LSWTSS.TestCefMod",
                            ["assemblyPath"] = "test-cef-mod.dll",
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
                    new JObject
                    {
                        ["id"] = "input-hook1",
                    },
                    new JObject
                    {
                        ["id"] = "overlay1",
                    },
                },
            }.ToString(Newtonsoft.Json.Formatting.Indented)
        );
    }
}