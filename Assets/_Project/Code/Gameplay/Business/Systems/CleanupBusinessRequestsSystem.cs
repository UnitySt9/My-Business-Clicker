using Code.Gameplay.Business.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CleanupBusinessRequestsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _levelUpRequests;
        private EcsFilter _upgradeRequests;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _levelUpRequests = _world.Filter<LevelUpRequestComponent>().End();
            _upgradeRequests = _world.Filter<UpgradePurchasedRequestComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int request in _levelUpRequests)
            {
                _world.DelEntity(request);
            }

            foreach (int request in _upgradeRequests)
            {
                _world.DelEntity(request);
            }
        }
    }
}