namespace OMP.LSWTSS;

public static class BuildCApi1MainDotnetPackage
{
    public static void Execute(string steamGameDirPath, string egsGameDirPath)
    {
        BuildCApi1CoreDotnetPackage.Execute();

        RunCApi1Scraper.Execute(steamGameDirPath, CApi1.Variant.Steam);
        RunCApi1Scraper.Execute(egsGameDirPath, CApi1.Variant.EGS);

        GenerateCApi1MainDotnetPackageSrcFiles.Execute();

        BuildDotnetPackage.Execute(
            GetCApi1MainDotnetPackageDirPath.Execute()
        );
    }
}