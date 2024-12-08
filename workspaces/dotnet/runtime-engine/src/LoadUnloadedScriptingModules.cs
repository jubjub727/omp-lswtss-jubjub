using System;

namespace OMP.LSWTSS;

public static partial class RuntimeEngine
{
    public static void LoadUnloadedScriptingModules()
    {
        Console.WriteLine("Loading unloaded scripting modules...");

        foreach (var scriptingModuleContext in _scriptingModuleContexts)
        {
            LoadScriptingModuleIfUnloaded(scriptingModuleContext);
        }

        Console.WriteLine("Loaded unloaded scripting modules!");
    }
}