using System.IO;

namespace OMP.LSWTSS;

public static class BuildCApi1MainDotnetPackage
{
    public static void Execute(string steamGameDirPath, string egsGameDirPath)
    {
        BuildCApi1CoreDotnetPackage.Execute();

        if (
            !File.Exists(
                Path.Combine(
                    GetCApi1MainDotnetPackageDirPath.Execute(),
                    "src",
                    "AngelScriptSchema.Steam.json"
                )
            )
        )
        {
            RunCApi1Scraper.Execute(steamGameDirPath, CApi1.Variant.Steam);
        }

        if (
            !File.Exists(
                Path.Combine(
                    GetCApi1MainDotnetPackageDirPath.Execute(),
                    "src",
                    "AngelScriptSchema.EGS.json"
                )
            )
        )
        {
            RunCApi1Scraper.Execute(egsGameDirPath, CApi1.Variant.EGS);
        }

        GenerateCApi1MainDotnetPackageSrcFiles.Execute();

        BuildDotnetPackage.Execute(
            GetCApi1MainDotnetPackageDirPath.Execute()
        );
    }
}