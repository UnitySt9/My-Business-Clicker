using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CreateUpgradeModifierOnUpgradePurchasedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _businesses;
        private EcsFilter _upgradeRequests;

        private EcsPool<BusinessIdComponent> _businessIdPool;
        private EcsPool<UpgradePurchasedRequestComponent> _upgradeRequestPool;
        private EcsPool<UpdateBusinessModifiersComponent> _updateBusinessModifiersPool;
        private EcsPool<PurchasedComponent> _purchasedPool;
        private EcsPool<OwnerIdComponent> _ownerIdPool;
        private EcsPool<UpgradeModifierComponent> _upgradeModifierPool;
        private EcsPool<IdComponent> _idPool;
        private EcsPool<UpgradeModifierIdComponent> _upgradeModifierIdPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            InitializeFilters();
            InitializePools();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int request in _upgradeRequests)
            {
                var upgradeRequest = _upgradeRequestPool.Get(request).Value;

                foreach (int business in _businesses)
                {
                    if (!IsMatchingBusiness(business, upgradeRequest.BusinessId))
                        continue;

                    ref var upgradeDatas = ref _updateBusinessModifiersPool.Get(business).Value;

                    UpgradeData upgradeData = upgradeDatas[upgradeRequest.UpgradeId];
            
                    var upgradeEntity = _world.NewEntity();

                    _purchasedPool.Add(upgradeEntity).Value = true;
                    _upgradeModifierPool.Add(upgradeEntity).Value = upgradeData.IncomeMultiplier;
                    _ownerIdPool.Add(upgradeEntity).Value = _idPool.Get(business).Value;
                    _upgradeModifierIdPool.Add(upgradeEntity).Value = upgradeRequest.UpgradeId;
                }
            }
        }

        private bool IsMatchingBusiness(int business, int businessId)
        {
            return _businessIdPool.Get(business).Value == businessId;
        }

        private void InitializeFilters()
        {
            _businesses = _world.Filter<BusinessComponent>()
                .Inc<BusinessIdComponent>()
                .Inc<UpdateBusinessModifiersComponent>()
                .Inc<AccumulatedModifierComponent>()
                .Inc<NameComponent>()
                .End();

            _upgradeRequests = _world.Filter<UpgradePurchasedRequestComponent>().End();
        }

        private void InitializePools()
        {
            _businessIdPool = _world.GetPool<BusinessIdComponent>();
            _purchasedPool = _world.GetPool<PurchasedComponent>();
            _upgradeModifierIdPool = _world.GetPool<UpgradeModifierIdComponent>();
            _ownerIdPool = _world.GetPool<OwnerIdComponent>();
            _idPool = _world.GetPool<IdComponent>();
            _upgradeModifierPool = _world.GetPool<UpgradeModifierComponent>();
            _upgradeRequestPool = _world.GetPool<UpgradePurchasedRequestComponent>();
            _updateBusinessModifiersPool = _world.GetPool<UpdateBusinessModifiersComponent>();
        }
    }
} 