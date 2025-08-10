using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Utils;
using Code.Utils;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Business.Systems
{
    public class CalculateTotalIncomeOnCooldownUpSystem : IEcsPostRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _businesses;
        
        private EcsPool<IncomeCooldownComponent> _cooldownPool;
        private EcsPool<BusinessModifiersComponent> _modifiersPool;
        private EcsPool<BusinessComponent> _businessPool;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            InitializeFilters();
            InitializePools();
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (int business in _businesses)
            {
                if (!IsCooldownUp(business))
                    continue;

                CalculateAndSetTotalIncome(business);
            }
        }

        private bool IsCooldownUp(int business)
        {
            return _cooldownPool.Get(business).IsCompleted;
        }

        private void CalculateAndSetTotalIncome(int business)
        {
            ref var businessComponent = ref _businessPool.Get(business);
            var modifiers = _modifiersPool.Get(business);

            var (firstModifier, secondModifier) = BusinessModifierUtils.GetModifiers(modifiers.AccumulatedModifiers);

            var totalIncome = Mathf.RoundToInt(BusinessCalculator.CalculateIncome(
                businessComponent.Level,
                businessComponent.BaseIncome,
                firstModifier,
                secondModifier));

            businessComponent.TotalIncome = totalIncome;
        }

        private void InitializeFilters()
        {
            _businesses = _world.Filter<BusinessComponent>()
                .Inc<IncomeCooldownComponent>()
                .Inc<BusinessModifiersComponent>()
                .Inc<PurchasedComponent>()
                .Inc<IdComponent>()
                .End();
        }

        private void InitializePools()
        {
            _cooldownPool = _world.GetPool<IncomeCooldownComponent>();
            _modifiersPool = _world.GetPool<BusinessModifiersComponent>();
            _businessPool = _world.GetPool<BusinessComponent>();
        }
    }
} 