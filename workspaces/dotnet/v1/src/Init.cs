namespace OMP.LSWTSS;

public static partial class V1
{
    public static void Init()
    {
        if (_isInitialized)
        {
            return;
        }

        cApi1NttFileFileDevicePCConstructorMethodHook.Enable();
        cApi1NuFileDeviceDatConstructorMethodHook.Enable();

        cApi1NuFileDeviceDatVtable24MethodHook.Enable();
        cApi1NuFileDeviceDatVtable31MethodHook.Enable();

        cApi1CollectablesTableConstructorMethodHook.Enable();
        cApi1CollectablesTableAddEntryInnerMethodHook.Enable();

        _isInitialized = true;
    }
}