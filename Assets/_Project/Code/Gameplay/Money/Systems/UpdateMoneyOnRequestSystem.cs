using Code.Common.Components;
using Code.Gameplay.Money.Components;
using Code.Gameplay.Money.Requests;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Money.Systems
{
    public class UpdateMoneyOnRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter _money;
        private EcsFilter _moneyUpdateRequest;
        
        private EcsPool<IdComponent> _idPool;
        private EcsPool<MoneyUpdateRequestComponent> _moneyUpdateRequestPool;
        private EcsPool<MoneyComponent> _moneyPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _money = _world.Filter<MoneyComponent>()
                .Inc<IdComponent>()
                .End();

            _idPool = _world.GetPool<IdComponent>();

            _moneyUpdateRequest = _world.Filter<MoneyUpdateRequestComponent>()
                .End();

            _moneyUpdateRequestPool = _world.GetPool<MoneyUpdateRequestComponent>();
            _moneyPool = _world.GetPool<MoneyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int updateRequest in _moneyUpdateRequest)
            foreach (int money in _money)
            {
                int id = _idPool.Get(money).Value;
                MoneyUpdateRequest moneyUpgradeRequest = _moneyUpdateRequestPool.Get(updateRequest).Value;

                if (moneyUpgradeRequest.OwnerId != id)
                    continue;

                ref var targetMoney = ref _moneyPool.Get(money).Value;
                targetMoney = Mathf.Clamp(targetMoney + moneyUpgradeRequest.Value, 0, int.MaxValue);
            }
        }
    }
}