namespace OMP.LSWTSS;

public static class GenerateCApi1MainDotnetPackageSrcFiles
{
    public static void Execute()
    {
        CApi1.GeneratePackageSrcFiles.Execute(
            GetCApi1MainDotnetPackageDirPath.Execute(),
            CApi1.MergeSchemas.Execute(
                [
                    CApi1.ReadPackageSchemaFile.Execute(
                        GetCApi1MainDotnetPackageDirPath.Execute(),
                        "AngelScriptSchema.Steam.json"
                    ),
                    CApi1.ReadPackageSchemaFile.Execute(
                        GetCApi1MainDotnetPackageDirPath.Execute(),
                        "AngelScriptSchema.EGS.json"
                    )
                ]
            )
        );
    }
}