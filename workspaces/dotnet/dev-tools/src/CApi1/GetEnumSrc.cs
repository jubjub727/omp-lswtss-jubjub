using System.Linq;

namespace OMP.LSWTSS.CApi1;

public static class GetEnumSrc
{
    public static string Execute(IEnumSchema enumSchema)
    {
        var enumSrcBuilder = new SrcBuilder();

        var enumNamespaceEntries = (enumSchema.Namespace ?? "").Split(".").Where(x => x != "").ToArray();

        enumSrcBuilder.Append("namespace OMP.LSWTSS.CApi1;");
        enumSrcBuilder.Append();

        foreach (var enumNamespaceEntry in enumNamespaceEntries)
        {
            enumSrcBuilder.Append($"public partial struct {enumNamespaceEntry}");
            enumSrcBuilder.Append("{");
            enumSrcBuilder.Ident++;
        }

        enumSrcBuilder.Append($"public enum {enumSchema.Name}");
        enumSrcBuilder.Append("{");
        enumSrcBuilder.Ident++;

        foreach (var enumEntrySchema in enumSchema.Entries)
        {
            enumSrcBuilder.Append($"{enumEntrySchema.Name} = {enumEntrySchema.Value},");
        }

        enumSrcBuilder.Ident--;
        enumSrcBuilder.Append("}");

        foreach (var _ in enumNamespaceEntries)
        {
            enumSrcBuilder.Ident--;
            enumSrcBuilder.Append("}");
        }

        return enumSrcBuilder.ToString().TrimEnd();
    }
}