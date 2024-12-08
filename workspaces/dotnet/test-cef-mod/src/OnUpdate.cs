using System.Linq;
using System.Runtime.InteropServices;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod
{
    static void OnUpdate()
    {
        var currentApiWorldHandle = V1.GetCApi1CurrentApiWorldHandle();

        if (currentApiWorldHandle == nint.Zero)
        {
            return;
        }

        var nttUniverseProcessingScopeHandle = (nttUniverseProcessingScope.Handle)Marshal.AllocHGlobal(nttUniverseProcessingScope.StructSize);

        nttUniverseProcessingScopeHandle.Constructor(currentApiWorldHandle.GetUniverse(), true);

        var apiWorldProcessingScopeHandle = (ApiWorldProcessingScope.Handle)Marshal.AllocHGlobal(nttUniverseProcessingScope.StructSize);

        apiWorldProcessingScopeHandle.Constructor(currentApiWorldHandle, true);

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

        apiWorldProcessingScopeHandle.Destructor();

        Marshal.FreeHGlobal(apiWorldProcessingScopeHandle);

        nttUniverseProcessingScopeHandle.Destructor();

        Marshal.FreeHGlobal(nttUniverseProcessingScopeHandle);
    }
}