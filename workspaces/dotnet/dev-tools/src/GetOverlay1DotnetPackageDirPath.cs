namespace OMP.LSWTSS;

public static class GetOverlay1DotnetPackageDirPath
{
    public static string Execute()
    {
        return System.IO.Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "overlay1"
        );
    }
}