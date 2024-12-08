namespace OMP.LSWTSS;

public static class GetInputHook1DotnetPackageDirPath
{
    public static string Execute()
    {
        return System.IO.Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "input-hook1"
        );
    }
}