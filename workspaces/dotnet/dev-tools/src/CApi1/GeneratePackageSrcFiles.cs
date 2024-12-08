using System;
using System.Linq;

namespace OMP.LSWTSS.CApi1;

public static class GeneratePackageSrcFiles
{
    public static void Execute(string packageDirPath, Schema packageSchema)
    {
        var packageSrcGeneratedDirPath = System.IO.Path.Combine(
            packageDirPath,
            "src",
            "Generated"
        );

        if (System.IO.Directory.Exists(packageSrcGeneratedDirPath))
        {
            System.IO.Directory.Delete(packageSrcGeneratedDirPath, true);
        }

        System.IO.Directory.CreateDirectory(packageSrcGeneratedDirPath);

        foreach (var packageEnumSchema in packageSchema.Enums)
        {
            var packageEnumSrcFileName = packageEnumSchema.Namespace == null
                ? $"{packageEnumSchema.Name}.cs"
                : $"{packageEnumSchema.Namespace}.{packageEnumSchema.Name}.cs";

            var packageEnumSrcFilePath = System.IO.Path.Combine(
                packageSrcGeneratedDirPath,
                packageEnumSrcFileName
            );

            System.IO.File.WriteAllText(
                packageEnumSrcFilePath,
                GetEnumSrc.Execute(packageEnumSchema)
            );
        }

        foreach (var packageGlobalFuncSchema in packageSchema.GlobalFuncs)
        {
            var packageGlobalFuncSrcFileName = packageGlobalFuncSchema.Namespace == null
                ? $"{packageGlobalFuncSchema.Name}GlobalFunc.cs"
                : $"{packageGlobalFuncSchema.Namespace}.{packageGlobalFuncSchema.Name}GlobalFunc.cs";

            var packageGlobalFuncSrcFilePath = System.IO.Path.Combine(
                packageSrcGeneratedDirPath,
                packageGlobalFuncSrcFileName
            );

            var packageGlobalFuncSrcBuilder = new SrcBuilder();

            var packageGlobalFuncNamespaceEntries = (packageGlobalFuncSchema.Namespace ?? "").Split(".").Where(x => x != "").ToArray();

            packageGlobalFuncSrcBuilder.Append("namespace OMP.LSWTSS.CApi1;");
            packageGlobalFuncSrcBuilder.Append();

            foreach (var packageGlobalFuncNamespaceEntry in packageGlobalFuncNamespaceEntries)
            {
                packageGlobalFuncSrcBuilder.Append($"public partial struct {packageGlobalFuncNamespaceEntry}");
                packageGlobalFuncSrcBuilder.Append("{");
                packageGlobalFuncSrcBuilder.Ident++;
            }

            packageGlobalFuncSrcBuilder.Append();

            packageGlobalFuncSrcBuilder.Append(
                GetFuncClassSrc.Execute(
                    packageGlobalFuncSchema,
                    GetGlobalFuncClassImplSrc.Execute(packageGlobalFuncSchema)
                )
            );

            foreach (var _ in packageGlobalFuncNamespaceEntries)
            {
                packageGlobalFuncSrcBuilder.Ident--;
                packageGlobalFuncSrcBuilder.Append("}");
            }

            System.IO.File.WriteAllText(
                packageGlobalFuncSrcFilePath,
                packageGlobalFuncSrcBuilder.ToString().TrimEnd()
            );
        }

        foreach (var packageClassSchema in packageSchema.Classes)
        {
            var packageClassSrcFileName = packageClassSchema.Namespace == null
                ? $"{packageClassSchema.Name}.cs"
                : $"{packageClassSchema.Namespace}.{packageClassSchema.Name}.cs";

            var packageClassSrcFilePath = System.IO.Path.Combine(
                packageSrcGeneratedDirPath,
                packageClassSrcFileName
            );

            System.IO.File.WriteAllText(
                packageClassSrcFilePath,
                GetClassSrc.Execute(packageClassSchema)
            );
        }
    }
}