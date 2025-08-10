using Code.Gameplay.Money.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Money.Systems
{
    public class CleanupMoneyRequestsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _moneyUpdateRequest;
        private EcsPool<MoneyUpdateRequestComponent> _moneyUpdateRequestPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _moneyUpdateRequest = _world.Filter<MoneyUpdateRequestComponent>().End();
            _moneyUpdateRequestPool = _world.GetPool<MoneyUpdateRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int updateRequest in _moneyUpdateRequest)
            {
                _moneyUpdateRequestPool.Del(updateRequest);
            }
        }
    }
}