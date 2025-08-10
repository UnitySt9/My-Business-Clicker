using System.Collections.Generic;
using System.Linq;
using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Business.Utils;
using Code.Utils;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Business.Systems
{
    public class RecalculateBusinessValuesSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly BusinessService _businessService;

        private EcsWorld _world;
        private EcsFilter _businesses;

        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessModifiersComponent> _modifiersPool;
        private EcsPool<LevelUpPriceComponent> _levelUpPricePool;

        public RecalculateBusinessValuesSystem(BusinessService businessService)
        {
            _businessService = businessService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            InitializeFilters();
            InitializePools();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int business in _businesses)
            {
                RecalculateValues(business);
                NotifyBusinessUpdate(business);
            }
        }

        private void RecalculateValues(int business)
        {
            ref var businessComponent = ref _businessPool.Get(business);
            var modifiers = _modifiersPool.Get(business);
            ref var levelUpPrice = ref _levelUpPricePool.Get(business).Value;

            var (firstModifier, secondModifier) = BusinessModifierUtils.GetModifiers(modifiers.AccumulatedModifiers);

            businessComponent.CurrentIncome = Mathf.RoundToInt(BusinessCalculator.CalculateIncome(businessComponent.Level, businessComponent.BaseIncome, firstModifier, secondModifier));
            businessComponent.TotalIncome = businessComponent.CurrentIncome;
            levelUpPrice = BusinessCalculator.CalculateLevelUpPrice(businessComponent.Level, businessComponent.BaseCost);
        }

        private void NotifyBusinessUpdate(int business)
        {
            var businessComponent = _businessPool.Get(business);
            var levelUpPrice = _levelUpPricePool.Get(business).Value;
            var modifiers = _modifiersPool.Get(business);

            _businessService.NotifyBusinessDataUpdated(businessComponent.Id, businessComponent.Level, businessComponent.CurrentIncome, levelUpPrice, businessComponent.Name, modifiers.AccumulatedModifiers);
        }

        private void InitializeFilters()
        {
            _businesses = _world.Filter<BusinessComponent>()
                .Inc<BusinessModifiersComponent>()
                .Inc<LevelUpPriceComponent>()
                .End();
        }

        private void InitializePools()
        {
            _businessPool = _world.GetPool<BusinessComponent>();
            _modifiersPool = _world.GetPool<BusinessModifiersComponent>();
            _levelUpPricePool = _world.GetPool<LevelUpPriceComponent>();
        }
    }
}