namespace OMP.LSWTSS;

public static class GetCApi1MainDotnetPackageDirPath
{
    public static string Execute()
    {
        return System.IO.Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "c-api1-main"
        );
    }
}