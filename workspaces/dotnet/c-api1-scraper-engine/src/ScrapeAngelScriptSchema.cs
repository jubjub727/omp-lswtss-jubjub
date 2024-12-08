using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace OMP.LSWTSS.CApi1;

public static class ScrapeAngelScriptSchema
{
    public static void Execute(AsIScriptEngine.Handle asIScriptEngineHandle)
    {
        var angelScriptSchema = GetAngelScriptSchema.Execute(asIScriptEngineHandle);

        var angelScriptSchemaAsJson = JsonConvert.SerializeObject(
            angelScriptSchema,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            }
        );

        var angelScriptDirPath = Path.GetFullPath(
            Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "..",
                "..",
                "..",
                "..",
                "c-api1-main"
            )
        );

        Console.WriteLine(angelScriptDirPath);

        var variant = GetVariant.Execute();

        var angelScriptSchemaFilePath = Path.Combine(angelScriptDirPath, "src", $"AngelScriptSchema.{variant}.json");

        Console.WriteLine(angelScriptSchemaFilePath);

        File.WriteAllText(angelScriptSchemaFilePath, angelScriptSchemaAsJson);
    }
}