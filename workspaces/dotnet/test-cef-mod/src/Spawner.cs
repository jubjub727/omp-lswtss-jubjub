using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod
{
    class Spawner : IDisposable
    {
        public bool IsDisposed { get; private set; }

        static int? _lastId;

        public int Id { get; private set; }

        public SpawnerConfig Config { get; set; }

        public Vector3 Position { get; set; }

        public bool IsActive { get; set; }

        public bool ShouldBeDestroyed { get; set; }

        public bool IsInPlayerEntityRange { get; private set; }

        readonly List<Npc> _npcs = [];

        readonly List<SpawnNpcTask> _pendingSpawnNpcTasks = [];

        DateTime? _lastSpawnNpcTaskTime;

        PrefabResource? _hologramPrefabResource;

        apiEntity.Handle _hologramHandle;

        apiTransformComponent.Handle _hologramTransformComponentHandle;

        float _hologramRotationY;

        DateTime? _hologramRotationLastUpdateTime;

        public Spawner(SpawnerConfig config, Vector3 position)
        {
            IsDisposed = false;

            Id = _lastId == null ? 0 : _lastId.Value + 1;

            _lastId = Id;

            Config = config;

            Position = position;

            IsActive = false;

            ShouldBeDestroyed = false;

            IsInPlayerEntityRange = false;

            _hologramPrefabResource = null;

            _hologramHandle = (apiEntity.Handle)nint.Zero;

            _hologramTransformComponentHandle = (apiTransformComponent.Handle)nint.Zero;

            _hologramRotationY = 0.0f;

            _hologramRotationLastUpdateTime = null;

            _spawners.Add(this);
        }

        void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException();
            }
        }

        void TryCreateSpawnNpcTask()
        {
            if (!IsActive)
            {
                return;
            }

            if (_npcs.Count + _pendingSpawnNpcTasks.Count >= Config.NpcsMaxCount)
            {
                return;
            }

            if (_lastSpawnNpcTaskTime != null && DateTime.Now - _lastSpawnNpcTaskTime < TimeSpan.FromSeconds(Config.SpawnNpcTasksIntervalAsSeconds))
            {
                return;
            }

            var currentApiWorldHandle = V1.GetCApi1CurrentApiWorldHandle();

            if (currentApiWorldHandle == nint.Zero)
            {
                return;
            }

            var spawnNpcTask = new SpawnNpcTask(
                Config.NpcPreset,
                Position,
                isGlobal: false
            );

            _pendingSpawnNpcTasks.Add(spawnNpcTask);

            _lastSpawnNpcTaskTime = DateTime.Now;
        }

        void UpdateHologramRotation()
        {
            var now = DateTime.Now;

            var timeSinceLastHologramRotationUpdate = _hologramRotationLastUpdateTime == null ? TimeSpan.Zero : now - _hologramRotationLastUpdateTime.Value;

            _hologramRotationY = (_hologramRotationY + 0.1f * (float)timeSinceLastHologramRotationUpdate.TotalMilliseconds) % 360.0f;

            _hologramTransformComponentHandle.SetRotation(0f, _hologramRotationY, 0f);

            _hologramRotationLastUpdateTime = now;

            var hologramNttRenderComponentHandle = (nttRenderTTExportObject.Handle)(nint)_hologramHandle.FindComponentByTypeName("nttRenderTTExportObject");

            if (hologramNttRenderComponentHandle != nint.Zero)
            {
                var hologramOpacity = IsActive ? 1.0f : 0.1f;
                hologramNttRenderComponentHandle.set_HologramOpacity(ref hologramOpacity);
            }
        }

        void UpdateHologram()
        {
            if (_hologramHandle != nint.Zero)
            {
                if (!_hologramHandle.IsActive())
                {
                    Dispose();
                }

                UpdateHologramRotation();

                return;
            }

            if (_hologramPrefabResource == null)
            {
                _hologramPrefabResource = new PrefabResource("Chars/Minifig/Hologram/Hologram.prefab_baked");
            }

            if (!_hologramPrefabResource.FetchIsLoaded())
            {
                return;
            }

            var currentApiWorldHandle = V1.GetCApi1CurrentApiWorldHandle();

            if (currentApiWorldHandle == nint.Zero)
            {
                return;
            }

            _hologramHandle = _hologramPrefabResource.FetchPrefabHandle().Clone();

            _hologramHandle.SetNoSerialise();
            _hologramHandle.SetParent(currentApiWorldHandle.GetSceneGraphRoot());

            _hologramTransformComponentHandle = (apiTransformComponent.Handle)(nint)_hologramHandle.FindComponentByTypeName("apiTransformComponent");

            _hologramTransformComponentHandle.SetPosition(Position.X, Position.Y, Position.Z);

            UpdateHologramRotation();
        }

        public void Update()
        {
            ThrowIfDisposed();

            if (ShouldBeDestroyed)
            {
                Dispose();

                return;
            }

            UpdateHologram();

            var playerEntityPosition = GetPlayerEntityPosition();

            IsInPlayerEntityRange = playerEntityPosition != null && Vector3.Distance(playerEntityPosition.Value, Position) <= 0.5f;

            foreach (var pendingSpawnNpcTask in _pendingSpawnNpcTasks.ToList())
            {
                if (pendingSpawnNpcTask.Npc != null)
                {
                    _npcs.Add(pendingSpawnNpcTask.Npc);

                    _pendingSpawnNpcTasks.Remove(pendingSpawnNpcTask);

                    pendingSpawnNpcTask.Dispose();
                }
            }

            _npcs.RemoveAll((npc) => npc.IsDisposed);

            if (!IsActive)
            {
                return;
            }

            TryCreateSpawnNpcTask();
        }

        public void DestroyNpcs()
        {
            foreach (var pendingSpawnNpcTasks in _pendingSpawnNpcTasks)
            {
                pendingSpawnNpcTasks.Dispose();
            }

            _pendingSpawnNpcTasks.Clear();

            foreach (var npc in _npcs)
            {
                npc.Dispose();
            }

            _npcs.Clear();
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            if (_hologramHandle != nint.Zero && _hologramHandle.IsActive())
            {
                _hologramHandle.DeferredDelete();
            }

            _hologramPrefabResource?.Dispose();

            _spawners.Remove(this);

            DestroyNpcs();

            IsDisposed = true;
        }
    }
}