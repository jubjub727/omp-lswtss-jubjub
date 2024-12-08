using System.Linq;

namespace OMP.LSWTSS.CApi1;

public static class GetClassSrc
{
    public static string Execute(IClassSchema classSchema)
    {
        var angelScriptClassSchema = classSchema as IAngelScriptClassSchema;
        var nativeClassSchema = classSchema as INativeClassSchema;

        var classSrcBuilder = new SrcBuilder();

        var classNamespaceEntries = (classSchema.Namespace ?? "").Split(".").Where(x => x != "").ToArray();

        classSrcBuilder.Append("namespace OMP.LSWTSS.CApi1;");
        classSrcBuilder.Append();

        foreach (var classNamespaceEntry in classNamespaceEntries)
        {
            classSrcBuilder.Append($"public partial struct {classNamespaceEntry}");
            classSrcBuilder.Append("{");
            classSrcBuilder.Ident++;
        }


        classSrcBuilder.Append("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]");
        classSrcBuilder.Append($"public partial struct {classSchema.Name}");
        classSrcBuilder.Append("{");
        classSrcBuilder.Ident++;

        var classHandleSrcBuilder = new SrcBuilder();

        classHandleSrcBuilder.Append("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]");
        classHandleSrcBuilder.Append($"public struct Handle");
        classHandleSrcBuilder.Append("{");
        classHandleSrcBuilder.Ident++;
        classHandleSrcBuilder.Append("public nint Ptr { get; set; }");
        classHandleSrcBuilder.Append();

        if (angelScriptClassSchema != null)
        {
            foreach (var angelScriptClassMethodSchema in angelScriptClassSchema.Methods)
            {
                classSrcBuilder.Append(
                    GetFuncClassSrc.Execute(
                        angelScriptClassMethodSchema,
                        GetAngelScriptClassMethodClassImplSrc.Execute(
                            angelScriptClassSchema,
                            angelScriptClassMethodSchema
                        )
                    )
                );
                classSrcBuilder.Append();

                classHandleSrcBuilder.Append(
                    GetClassHandleMethodSrc.Execute(angelScriptClassMethodSchema)
                );
                classHandleSrcBuilder.Append();
            }
        }
        if (nativeClassSchema != null)
        {
            foreach (var nativeClassMethodSchema in nativeClassSchema.Methods)
            {
                classSrcBuilder.Append(
                    GetFuncClassSrc.Execute(
                        nativeClassMethodSchema,
                        GetNativeClassMethodClassImplSrc.Execute(
                            nativeClassMethodSchema
                        )
                    )
                );
                classSrcBuilder.Append();

                classHandleSrcBuilder.Append(
                    GetClassHandleMethodSrc.Execute(nativeClassMethodSchema)
                );
                classHandleSrcBuilder.Append();
            }
        }

        classHandleSrcBuilder.Append($"public static implicit operator nint(Handle handle) => handle.Ptr;");
        classHandleSrcBuilder.Append();
        classHandleSrcBuilder.Append($"public static explicit operator Handle(nint ptr) => new() {{ Ptr = ptr }};");
        classHandleSrcBuilder.Ident--;
        classHandleSrcBuilder.Append("}");

        classSrcBuilder.Append(classHandleSrcBuilder.ToString().TrimEnd());
        classSrcBuilder.Ident--;
        classSrcBuilder.Append("}");

        foreach (var _ in classNamespaceEntries)
        {
            classSrcBuilder.Ident--;
            classSrcBuilder.Append("}");
        }

        return classSrcBuilder.ToString().TrimEnd();
    }
}