namespace OMP.LSWTSS;

public static class GetCFuncHook1DotnetPackageDirPath
{
    public static string Execute()
    {
        return System.IO.Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "c-func-hook1"
        );
    }
}