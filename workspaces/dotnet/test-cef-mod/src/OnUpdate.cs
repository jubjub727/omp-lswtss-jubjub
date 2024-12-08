using System.Linq;
using System.Runtime.InteropServices;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod
{
    delegate void nttUniverseProcessingScopeConstructorDelegate(nttUniverseProcessingScope.Handle handle, nttUniverse.Handle universe, bool flag);

    delegate void nttUniverseProcessingScopeDestructorDelegate(nttUniverseProcessingScope.Handle handle);

    delegate void ApiWorldProcessingScopeConstructorDelegate(ApiWorldProcessingScope.Handle handle, ApiWorld.Handle universe, bool flag);

    delegate void ApiWorldProcessingScopeDestructorDelegate(ApiWorldProcessingScope.Handle handle);

    static void OnUpdate()
    {
        var currentApiWorldHandle = GetCurrentApiWorldHandle();

        if (currentApiWorldHandle == nint.Zero)
        {
            return;
        }

        var nttUniverseProcessingScopeHandle = (nttUniverseProcessingScope.Handle)Marshal.AllocHGlobal(0x20);

        var nttUniverseProcessingScopeConstructor = NativeFunc.GetExecute<nttUniverseProcessingScopeConstructorDelegate>(
            NativeFunc.GetPtr(
                GetVariantValue.Execute(steamValue: 0x2E4B3F0, egsValue: 0x2E4AF90)
            )
        ); 

        nttUniverseProcessingScopeConstructor(nttUniverseProcessingScopeHandle, currentApiWorldHandle.GetUniverse(), true);

        var apiWorldProcessingScopeHandle = (ApiWorldProcessingScope.Handle)Marshal.AllocHGlobal(0x20);

        var apiWorldProcessingScopeConstructor = NativeFunc.GetExecute<ApiWorldProcessingScopeConstructorDelegate>(
            NativeFunc.GetPtr(
                GetVariantValue.Execute(steamValue: 0x2E4C050, egsValue: 0x2E4BBF0)
            )
        );

        apiWorldProcessingScopeConstructor(apiWorldProcessingScopeHandle, currentApiWorldHandle, true);

        if (_charactersInfo == null)
        {
            _charactersInfo = V1.FetchCharactersInfo();
        }

        _playerEntityLastPosition = GetPlayerEntityPosition();

        _spawnerInPlayerEntityRange = _spawners.FirstOrDefault(spawner => spawner.IsInPlayerEntityRange);

        foreach (var spawnNpcTask in _spawnNpcTasks.ToList())
        {
            spawnNpcTask.Update();
        }

        foreach (var npc in _npcs.ToList())
        {
            npc.Update();
        }

        foreach (var spawner in _spawners.ToList())
        {
            spawner.Update();
        }

        var apiWorldProcessingScopeDestructor = NativeFunc.GetExecute<ApiWorldProcessingScopeDestructorDelegate>(
            NativeFunc.GetPtr(
                GetVariantValue.Execute(steamValue: 0x2E4C110, egsValue: 0x2E4BCB0)
            )
        );

        apiWorldProcessingScopeDestructor(apiWorldProcessingScopeHandle);

        var nttUniverseProcessingScopeDestructor = NativeFunc.GetExecute<nttUniverseProcessingScopeDestructorDelegate>(
            NativeFunc.GetPtr(
                GetVariantValue.Execute(steamValue: 0x2E4B500, egsValue: 0x2E4B0A0)
            )
        );

        nttUniverseProcessingScopeDestructor(nttUniverseProcessingScopeHandle);
    }
}