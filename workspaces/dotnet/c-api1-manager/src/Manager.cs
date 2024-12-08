namespace OMP.LSWTSS.CApi1;

public class Manager
{
    readonly CFuncHook1<AsCScriptEngine.RegisterObjectTypeMethod.Delegate> _asCScriptEngineRegisterObjectTypeMethodHook;

    public Manager()
    {
        _asCScriptEngineRegisterObjectTypeMethodHook = new(
            AsCScriptEngine.RegisterObjectTypeMethod.Ptr,
            (handle, name, byteSize, flags) =>
            {
                var asIScriptEngineHandle = (AsIScriptEngine.Handle)(nint)handle;
                AsIScriptMainContext.Init(asIScriptEngineHandle);
                return _asCScriptEngineRegisterObjectTypeMethodHook!.Trampoline!(handle, name, byteSize, flags);
            }
        );

        _asCScriptEngineRegisterObjectTypeMethodHook.Enable();
    }
}