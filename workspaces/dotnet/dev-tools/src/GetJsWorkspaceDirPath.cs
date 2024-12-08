namespace OMP.LSWTSS;

public static class GetJsWorkspaceDirPath
{
    public static string Execute()
    {
        return System.IO.Path.Combine(
            System.Environment.CurrentDirectory,
            "..",
            "..",
            "js"
        );
    }
}