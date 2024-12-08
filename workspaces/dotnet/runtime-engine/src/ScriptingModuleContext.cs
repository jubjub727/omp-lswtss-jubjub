namespace OMP.LSWTSS;

public static partial class RuntimeEngine
{
    class ScriptingModuleContext
    {
        public required ScriptingModuleInfo ScriptingModuleInfo { get; set; }

        public ScriptingModuleAssemblyLoadContext? ScriptingModuleAssemblyLoadContext;

        public object? ScriptingModule;
    }
}