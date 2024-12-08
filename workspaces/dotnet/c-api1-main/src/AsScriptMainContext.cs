using System;

namespace OMP.LSWTSS.CApi1;

public static class AsIScriptMainContext
{
    private static AsIScriptEngine.Handle _engineHandle;

    private static AsIScriptContext.Handle _handle;

    public static void Init(AsIScriptEngine.Handle asIScriptEngineHandle)
    {
        if (_handle != nint.Zero)
        {
            return;
        }
        Console.WriteLine("Initialized AsIScriptMainContext");
        _engineHandle = asIScriptEngineHandle;
        _handle = asIScriptEngineHandle.CreateContext();
    }

    public static AsIScriptEngine.Handle EngineHandle
    {
        get
        {
            if (_engineHandle == nint.Zero)
            {
                throw new InvalidOperationException();
            }

            return _engineHandle;
        }
    }

    public static AsIScriptContext.Handle Handle
    {
        get
        {
            if (_handle == nint.Zero)
            {
                throw new InvalidOperationException();
            }

            return _handle;
        }
    }
}