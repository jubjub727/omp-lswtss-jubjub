using System;

namespace OMP.LSWTSS.CApi1;

public static class GetAngelScriptEnumSchema
{
    public static EnumSchema Execute(
        AsITypeInfo.Handle asIEnumHandle
    )
    {
        var angelScriptEnumName = asIEnumHandle.GetName();

        var angelScriptEnumNamespace = asIEnumHandle.GetNamespace();

        if (angelScriptEnumNamespace == string.Empty)
        {
            angelScriptEnumNamespace = null;
        }
        else
        {
            angelScriptEnumNamespace = angelScriptEnumNamespace.Replace("::", ".");
        }

        var angelScriptEnumSchema = new EnumSchema
        {
            Name = angelScriptEnumName,
            Namespace = angelScriptEnumNamespace,
            Entries = [],
        };

        for (uint asIEnumValueIndex = 0; asIEnumValueIndex < asIEnumHandle.GetEnumValueCount(); asIEnumValueIndex++)
        {
            var angelScriptEnumValueName = asIEnumHandle.GetEnumValueByIndex(asIEnumValueIndex, out int angelScriptEnumValue) ?? throw new InvalidOperationException();

            angelScriptEnumSchema.Entries.Add(
                new EnumEntrySchema
                {
                    Name = angelScriptEnumValueName,
                    Value = angelScriptEnumValue,
                }
            );
        }

        return angelScriptEnumSchema;
    }
}