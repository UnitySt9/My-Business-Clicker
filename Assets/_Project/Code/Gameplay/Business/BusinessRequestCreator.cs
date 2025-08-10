using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Requests;
using Code.Gameplay.Money;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business
{
    public class BusinessRequestCreator
    {
        private readonly EcsWorld _ecsWorld;
        private readonly IMoneyService _moneyService;
        private EcsPool<LevelUpRequestComponent> _levelUpRequestPool;
        private EcsPool<UpgradePurchasedRequestComponent> _upgradeRequestPool;

        public BusinessRequestCreator(EcsWorld ecsWorld, IMoneyService moneyService)
        {
            _ecsWorld = ecsWorld;
            _moneyService = moneyService;
        }

        public void Initialize()
        {
            _levelUpRequestPool = _ecsWorld.GetPool<LevelUpRequestComponent>();
            _upgradeRequestPool = _ecsWorld.GetPool<UpgradePurchasedRequestComponent>();
        }

        public bool TryPurchaseLevelUp(int businessId, int levelPrice, int level)
        {
            if(!_moneyService.TryPurchase(levelPrice))
                return false;
            
            CreateLevelUpRequest(businessId, level);
            return true;
        }

        public bool TryPurchaseUpgrade(int businessId, int upgradeId, int price)
        {
            if (!_moneyService.TryPurchase(price))
                return false;

            CreateUpgradeRequest(businessId, upgradeId);
            return true;
        }

        private void CreateLevelUpRequest(int businessId, int level)
        {
            int request = _ecsWorld.NewEntity();
            _levelUpRequestPool.Add(request).Value = new LevelUpRequest(businessId, level);
        }

        private void CreateUpgradeRequest(int businessId, int upgradeId)
        {
            int request = _ecsWorld.NewEntity();
            _upgradeRequestPool.Add(request).Value = new UpgradePurchasedRequest(businessId, upgradeId);
        }
    }
} 