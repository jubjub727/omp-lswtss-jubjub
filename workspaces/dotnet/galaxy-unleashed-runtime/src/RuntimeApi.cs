using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OMP.LSWTSS;

#pragma warning disable 0649
public partial class GalaxyUnleashed
{
    class RuntimeApi()
    {
        readonly static JsonSerializerSettings _jsonSerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public string FetchCharactersInfo(string argsAsJson)
        {
            if (_instance!._charactersInfo == null)
            {
                throw new System.InvalidOperationException();
            }

            return JsonConvert.SerializeObject(_instance!._charactersInfo, _jsonSerializerSettings);
        }

        public string FetchNpcSpawnersState(string argsAsJson)
        {
            return JsonConvert.SerializeObject(_instance!._npcSpawners.Select(npcSpawner => npcSpawner.State).ToArray(), _jsonSerializerSettings);
        }

        public string FetchClosestNpcSpawnerInPlayerEntityRangeId(string argsAsJson)
        {
            return JsonConvert.SerializeObject(_instance!._closestNpcSpawnerInPlayerEntityRange?.State?.Id ?? null, _jsonSerializerSettings);
        }

        struct SetIsNpcSpawnerEnabledArgs
        {
            public string NpcSpawnerId;

            public bool IsNpcSpawnerEnabled;
        }

        public void SetIsNpcSpawnerEnabled(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SetIsNpcSpawnerEnabledArgs>(argsAsJson, _jsonSerializerSettings);

            var npcSpawner = _instance!._npcSpawners.FirstOrDefault(npcSpawner => npcSpawner.State.Id == args.NpcSpawnerId);

            if (npcSpawner != null)
            {
                npcSpawner.State.IsEnabled = args.IsNpcSpawnerEnabled;
            }
        }

        struct SetNpcSpawnerConfigArgs
        {
            public string NpcSpawnerId;

            public NpcSpawnerConfig NpcSpawnerConfig;
        }

        public void SetNpcSpawnerConfig(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SetNpcSpawnerConfigArgs>(argsAsJson, _jsonSerializerSettings);

            var npcSpawner = _instance!._npcSpawners.FirstOrDefault(npcSpawner => npcSpawner.State.Id == args.NpcSpawnerId);

            if (npcSpawner != null)
            {
                npcSpawner.State.Config = args.NpcSpawnerConfig;
            }
        }

        struct DestroyNpcSpawnerArgs
        {
            public string NpcSpawnerId;
        }

        public void DestroyNpcSpawner(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<DestroyNpcSpawnerArgs>(argsAsJson, _jsonSerializerSettings);

            var npcSpawner = _instance!._npcSpawners.FirstOrDefault(npcSpawner => npcSpawner.State.Id == args.NpcSpawnerId);

            if (npcSpawner != null)
            {
                npcSpawner.ShouldBeDestroyed = true;
            }
        }

        public string FetchBattleState(string argsAsJson)
        {
            return JsonConvert.SerializeObject(_instance!._battle.State, _jsonSerializerSettings);
        }

        struct SetIsBattleActiveArgs
        {
            public bool IsBattleActive;
        }

        public void SetIsBattleActive(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SetIsBattleActiveArgs>(argsAsJson, _jsonSerializerSettings);

            _instance!._battle.State.IsActive = args.IsBattleActive;
        }

        public string FetchModeState(string argsAsJson)
        {
            return JsonConvert.SerializeObject(_instance!._modeState, _jsonSerializerSettings);
        }

        struct SwitchToPlayModeArgs
        {
        }

        public void SwitchToPlayMode(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SwitchToPlayModeArgs>(argsAsJson, _jsonSerializerSettings);

            _instance!._modeState = new PlayModeState
            {
            };
        }

        struct SwitchToMenuModeArgs
        {
            public MenuModeConfig MenuModeConfig;
        }

        public void SwitchToMenuMode(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SwitchToMenuModeArgs>(argsAsJson, _jsonSerializerSettings);

            _instance!._modeState = new MenuModeState
            {
                Config = args.MenuModeConfig
            };
        }

        struct SwitchToQuickSpawnNpcsModeArgs
        {
            public QuickSpawnNpcsModeConfig QuickSpawnNpcsModeConfig;
        }

        public void SwitchToQuickSpawnNpcsMode(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SwitchToQuickSpawnNpcsModeArgs>(argsAsJson, _jsonSerializerSettings);

            _instance!._modeState = new QuickSpawnNpcsModeState
            {
                Config = args.QuickSpawnNpcsModeConfig
            };
        }

        struct SwitchToCreateNpcSpawnersModeArgs
        {
            public CreateNpcSpawnersModeConfig CreateNpcSpawnersModeConfig;
        }

        public void SwitchToCreateNpcSpawnersMode(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SwitchToCreateNpcSpawnersModeArgs>(argsAsJson, _jsonSerializerSettings);

            _instance!._modeState = new CreateNpcSpawnersModeState
            {
                Config = args.CreateNpcSpawnersModeConfig
            };
        }

        struct SwitchToManageBattleFlagModeArgs
        {
        }

        public void SwitchToManageBattleFlagMode(string argsAsJson)
        {
            var args = JsonConvert.DeserializeObject<SwitchToManageBattleFlagModeArgs>(argsAsJson, _jsonSerializerSettings);

            _instance!._modeState = new ManageBattleFlagModeState
            {
            };
        }
    }
}
#pragma warning restore 0649