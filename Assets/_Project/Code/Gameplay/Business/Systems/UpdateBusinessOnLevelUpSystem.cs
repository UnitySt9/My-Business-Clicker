using System.Collections.Generic;
using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Business.Requests;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class UpdateBusinessOnLevelUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _businesses;
        private EcsFilter _levelUpRequests;

        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<LevelUpRequestComponent> _levelUpRequestPool;
        private EcsPool<PurchasedComponent> _purchasedPool;
        private EcsPool<IncomeCooldownComponent> _cooldownPool;
        private EcsPool<BusinessModifiersComponent> _modifiersPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            InitializeFilters();
            InitializePools();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int updateRequest in _levelUpRequests)
            {
                var request = _levelUpRequestPool.Get(updateRequest).Value;

                if (request.Level <= -1)
                    continue;

                foreach (int business in _businesses)
                {
                    if (!IsMatchingBusiness(business, request))
                        continue;

                    UpdateBusinessLevel(business, request.Level);
                    UpdateBusinessState(business);
                    ResetUpgradeModifiers(business);
                }
            }
        }

        private bool IsMatchingBusiness(int business, LevelUpRequest request)
        {
            int businessId = _businessPool.Get(business).Id;
            return request.BusinessId == businessId;
        }

        private void ResetUpgradeModifiers(int business)
        {
            if (!_modifiersPool.Has(business))
                return;

            ref var modifiers = ref _modifiersPool.Get(business);
            
            foreach (AccumulatedModifiersData accumulatedModifiersData in modifiers.AccumulatedModifiers)
            {
                accumulatedModifiersData.Purchased = false;
            }
        }

        private void UpdateBusinessLevel(int business, int level)
        {
            ref var businessComponent = ref _businessPool.Get(business);
            businessComponent.Level += level;
        }

        private void UpdateBusinessState(int business)
        {
            var businessComponent = _businessPool.Get(business);

            if (businessComponent.Level > 0)
            {
                MarkPurchasedIfNot(business);
                MarkIncomeCooldownAvailableIfNot(business);
            }
        }

        private void MarkIncomeCooldownAvailableIfNot(int business)
        {
            if (!_cooldownPool.Has(business))
                return;

            ref var cooldown = ref _cooldownPool.Get(business);
            cooldown.IsAvailable = true;
        }

        private void MarkPurchasedIfNot(int business)
        {
            if (!_purchasedPool.Has(business))
            {
                _purchasedPool.Add(business).Value = true;
                return;
            }

            _purchasedPool.Get(business).Value = true;
        }

        private void InitializeFilters()
        {
            _businesses = _world.Filter<BusinessComponent>()
                .End();

            _levelUpRequests = _world.Filter<LevelUpRequestComponent>()
                .End();
        }

        private void InitializePools()
        {
            _businessPool = _world.GetPool<BusinessComponent>();
            _levelUpRequestPool = _world.GetPool<LevelUpRequestComponent>();
            _modifiersPool = _world.GetPool<BusinessModifiersComponent>();
            _purchasedPool = _world.GetPool<PurchasedComponent>();
            _cooldownPool = _world.GetPool<IncomeCooldownComponent>();
        }
    }
} 