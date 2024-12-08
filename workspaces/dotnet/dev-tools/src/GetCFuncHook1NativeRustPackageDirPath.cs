namespace OMP.LSWTSS;

public static class GetCFuncHook1NativeRustPackageDirPath
{
    public static string Execute()
    {
        return System.IO.Path.Combine(
            GetRustWorkspaceDirPath.Execute(),
            "c-func-hook1-native"
        );
    }
}