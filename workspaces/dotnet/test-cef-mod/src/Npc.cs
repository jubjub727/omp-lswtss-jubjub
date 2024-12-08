using System;
using System.Numerics;
using OMP.LSWTSS.CApi1;

namespace OMP.LSWTSS;

public partial class TestCefMod
{
    class Npc : IDisposable
    {
        public bool IsDisposed { get; private set; }

        readonly ApiWorld.Handle _worldHandle;

        readonly apiEntity.Handle _handle;

        readonly apiTransformComponent.Handle _transformComponentHandle;

        readonly RespawnContext.Handle _respawnContextHandle;

        readonly RespawnController.Handle _respawnControllerHandle;

        readonly AnimationRequestComponent.Handle _animationRequestComponentHandle;

        readonly OpponentInfo.Handle _opponentInfoHandle;

        readonly OpponentChooser.Handle _opponentChooserHandle;

        readonly OpponentSelectorComponent.Handle _opponentSelectorComponentHandle;

        readonly HorizontalCharacterMover.Handle _horizontalCharacterMoverHandle;

        readonly NavLinkCapabilitiesComponent.Handle _navLinkCapabilitiesComponentHandle;

        readonly NavRoute.Handle _navRouteHandle;

        public bool IsBattleParticipant { get; set; }

        Vector3? _lastBattleCenterPosition;

        public float MoveToBattleCenterBehaviorSpeed { get; set; }

        public Npc(ApiWorld.Handle worldHandle, apiEntity.Handle prefabHandle)
        {
            IsDisposed = false;

            _worldHandle = worldHandle;

            _handle = prefabHandle.Clone();

            _handle.SetNoSerialise();
            _handle.SetParent(_worldHandle.GetSceneGraphRoot());

            _transformComponentHandle = (apiTransformComponent.Handle)(nint)_handle.FindComponentByTypeName("apiTransformComponent");

            if (_transformComponentHandle == nint.Zero)
            {
                throw new InvalidOperationException();
            }

            _respawnContextHandle = (RespawnContext.Handle)(nint)_handle.FindComponentByTypeNameRecursive("RespawnContext", false);

            if (_respawnContextHandle != nint.Zero)
            {
                _respawnContextHandle.Disable();
            }

            _respawnControllerHandle = (RespawnController.Handle)(nint)_handle.FindComponentByTypeNameRecursive("RespawnController", false);

            _opponentInfoHandle = (OpponentInfo.Handle)(nint)_handle.FindComponentByTypeNameRecursive("OpponentInfo", false);

            _opponentChooserHandle = (OpponentChooser.Handle)(nint)_handle.FindComponentByTypeNameRecursive("OpponentChooser", false);

            _opponentSelectorComponentHandle = (OpponentSelectorComponent.Handle)(nint)_handle.FindComponentByTypeNameRecursive("OpponentSelectorComponent", false);

            _horizontalCharacterMoverHandle = (HorizontalCharacterMover.Handle)(nint)_handle.FindComponentByTypeNameRecursive("HorizontalCharacterMover", false);

            _navLinkCapabilitiesComponentHandle = (NavLinkCapabilitiesComponent.Handle)(nint)_handle.FindComponentByTypeNameRecursive("NavLinkCapabilitiesComponent", false);

            _animationRequestComponentHandle = (AnimationRequestComponent.Handle)(nint)_handle.FindComponentByTypeNameRecursive("AnimationRequestComponent", false);

            if (_animationRequestComponentHandle != nint.Zero && _respawnContextHandle != nint.Zero)
            {
                _animationRequestComponentHandle.RequestAnimation(_respawnContextHandle.get_RespawnAnim());
            }

            if (_navLinkCapabilitiesComponentHandle == nint.Zero)
            {
                _navRouteHandle = (NavRoute.Handle)nint.Zero;
            }
            else
            {
                _navRouteHandle = NavRoute.CreateGlobalFunc.Execute();

                _navRouteHandle.SetCapability(_navLinkCapabilitiesComponentHandle.Get());
                _navRouteHandle.SetRadius(0.5f);
                _navRouteHandle.SetDestinationRadius(0.5f);
            }

            IsBattleParticipant = false;

            _lastBattleCenterPosition = null;

            MoveToBattleCenterBehaviorSpeed = 1.0f;

            _npcs.Add(this);
        }

        void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException();
            }
        }

        public Vector3 FetchPosition()
        {
            ThrowIfDisposed();

            _transformComponentHandle.GetPosition(out var positionX, out var positionY, out var positionZ);

            return new Vector3(positionX, positionY, positionZ);
        }

        public void SetPosition(Vector3 position)
        {
            ThrowIfDisposed();

            _transformComponentHandle.SetPosition(position.X, position.Y, position.Z);
        }

        public string? FetchFactionName()
        {
            ThrowIfDisposed();

            if (_opponentInfoHandle == nint.Zero)
            {
                return null;
            }

            var factionType = _opponentInfoHandle.GetFaction();

            if (factionType == nint.Zero)
            {
                return null;
            }

            return factionType.get_FactionName();
        }

        public void SetFactionId(NpcFactionId factionId)
        {
            ThrowIfDisposed();

            if (_opponentInfoHandle == nint.Zero)
            {
                return;
            }

            var factionName = factionId switch
            {
                NpcFactionId.Empire => "Empire",
                NpcFactionId.Rebel => "RebelAlliance",
                NpcFactionId.Sith => "Sith",
                NpcFactionId.Resistance => "Resistance",
                _ => null
            };

            if (factionName == null)
            {
                return;
            }

            _opponentInfoHandle.ChangeFaction(factionName);
        }

        void UpdateMoveToBattleCenterBehavior()
        {
            if (!IsBattleParticipant)
            {
                return;
            }

            if (_battleConfig.CenterPosition == null)
            {
                return;
            }

            if (_opponentSelectorComponentHandle == nint.Zero || _opponentSelectorComponentHandle.CalculateBest() != nint.Zero)
            {
                return;
            }

            if (_horizontalCharacterMoverHandle == nint.Zero)
            {
                return;
            }

            var aiSystemHandle = AISystem.GetFromCreateGlobalFunc.Execute(_worldHandle);

            if (aiSystemHandle == nint.Zero)
            {
                return;
            }

            var battleCenterPosition = _battleConfig.CenterPosition.Value;

            if (
                _lastBattleCenterPosition == null
                ||
                (_lastBattleCenterPosition.Value - battleCenterPosition).Length() > 0.1f)
            {
                var battleCenterPositionAsNuVec3 = battleCenterPosition.ToCApiNuVec3();

                DestinationGoal.Handle destinationGoalHandle;

                unsafe
                {
                    destinationGoalHandle = CreateDestinationGoalGlobalFunc.Execute((NuVec3.Handle)(nint)(&battleCenterPositionAsNuVec3));
                }

                _navRouteHandle.SetGoal((BaseRouteGoal.Handle)(nint)destinationGoalHandle);

                _lastBattleCenterPosition = battleCenterPosition;
            }

            var position = FetchPosition();

            var positionAsNuVec3 = position.ToCApiNuVec3();

            unsafe
            {
                _navRouteHandle.SetStart(&positionAsNuVec3);
            }

            _navRouteHandle.Update(aiSystemHandle);

            if (!_navRouteHandle.HasPath())
            {
                return;
            }

            var moveToBattleCenterBehaviorDirectionAsNuVec3 = new NuVec3
            {
                X = 0f,
                Y = 0f,
                Z = 0f,
            };

            unsafe
            {
                _navRouteHandle.GetCurrentHeading2(&moveToBattleCenterBehaviorDirectionAsNuVec3);
            }

            var moveToBattleCenterBehaviorDirection = moveToBattleCenterBehaviorDirectionAsNuVec3.ToSystemNumericsVector3();

            if (
                MathF.Abs(moveToBattleCenterBehaviorDirection.X)
                +
                MathF.Abs(moveToBattleCenterBehaviorDirection.Z)
                <
                MathF.Abs(moveToBattleCenterBehaviorDirection.Y)
            )
            {
                // TODO: Add jumping
                return;
            }

            var moveToBattleCenterBehaviorVelocity = new Vector3
            {
                X = moveToBattleCenterBehaviorDirection.X,
                Y = 0f,
                Z = moveToBattleCenterBehaviorDirection.Z,
            };

            moveToBattleCenterBehaviorVelocity =
                Vector3.Normalize(moveToBattleCenterBehaviorVelocity)
                *
                MoveToBattleCenterBehaviorSpeed;

            var moveToBattleCenterBehaviorVelocityAsNuVec3 = moveToBattleCenterBehaviorVelocity.ToCApiNuVec3();

            unsafe
            {
                _horizontalCharacterMoverHandle.SetMoveLaneVelocity(&moveToBattleCenterBehaviorVelocityAsNuVec3);
            }

            var moveToBattleCenterBehaviorAngle = MathF.Atan2(
                moveToBattleCenterBehaviorDirection.Z,
                moveToBattleCenterBehaviorDirection.X
            ) * 180f / MathF.PI;

            _transformComponentHandle.SetRotation(0f, 270f - moveToBattleCenterBehaviorAngle, 0f);
        }

        public void Update()
        {
            ThrowIfDisposed();

            if (!_handle.IsActive())
            {
                Dispose();
                return;
            }

            UpdateMoveToBattleCenterBehavior();
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            _npcs.Remove(this);

            if (_handle.IsActive())
            {
                _handle.DeferredDelete();
            }

            IsDisposed = true;
        }
    }
}