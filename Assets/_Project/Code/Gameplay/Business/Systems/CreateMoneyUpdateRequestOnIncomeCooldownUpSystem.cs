using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Money.Components;
using Code.Gameplay.Money.Requests;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CreateMoneyUpdateRequestOnIncomeCooldownUpSystem : IEcsPostRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _businesses;
        
        private EcsPool<IncomeCooldownComponent> _cooldownPool;
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<MoneyUpdateRequestComponent> _moneyUpdateRequestPool;
        private EcsPool<OwnerIdComponent> _ownerIdPool;
        private EcsPool<PurchasedComponent> _purchasedPool;

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
                if (!IsCooldownCompleted(business) || !IsPurchased(business))
                    continue;

                CreateMoneyUpdateRequest(business);
            }
        }

        private bool IsCooldownCompleted(int business)
        {
            return _cooldownPool.Get(business).IsCompleted;
        }

        private bool IsPurchased(int business)
        {
            return _purchasedPool.Get(business).Value;
        }

        private void CreateMoneyUpdateRequest(int business)
        {
            var ownerId = _ownerIdPool.Get(business).Value;
            var businessComponent = _businessPool.Get(business);
            
            CreateNewMoneyUpdateRequest(ownerId, businessComponent.TotalIncome);
        }

        private void CreateNewMoneyUpdateRequest(int ownerId, int totalIncome)
        {
            int updateRequest = _world.NewEntity();
            _moneyUpdateRequestPool.Add(updateRequest)
                .Value = new MoneyUpdateRequest(ownerId, totalIncome);
        }

        private void InitializeFilters()
        {
            _businesses = _world.Filter<BusinessComponent>()
                .Inc<IncomeCooldownComponent>()
                .Inc<PurchasedComponent>()
                .Inc<OwnerIdComponent>()
                .Inc<IdComponent>()
                .End();
        }

        private void InitializePools()
        {
            _cooldownPool = _world.GetPool<IncomeCooldownComponent>();
            _businessPool = _world.GetPool<BusinessComponent>();
            _ownerIdPool = _world.GetPool<OwnerIdComponent>();
            _moneyUpdateRequestPool = _world.GetPool<MoneyUpdateRequestComponent>();
            _purchasedPool = _world.GetPool<PurchasedComponent>();
        }
    }
}