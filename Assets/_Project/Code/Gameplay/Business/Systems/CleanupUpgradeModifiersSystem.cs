using Code.Gameplay.Business.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CleanupUpgradeModifiersSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _upgradeModifiers;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _upgradeModifiers = _world.Filter<UpgradeModifierComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int modifier in _upgradeModifiers)
            {
                _world.DelEntity(modifier);
            }
        }
    }
}