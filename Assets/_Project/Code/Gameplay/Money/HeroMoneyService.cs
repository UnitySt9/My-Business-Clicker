using Code.Gameplay.Money.Components;
using Code.Gameplay.Money.Requests;
using Leopotam.EcsLite;

namespace Code.Gameplay.Money
{
    public class HeroMoneyService : IMoneyService
    {
        private readonly CurrencyModel _currencyModel;
        private readonly EcsWorld _ecsWorld;
        private EcsPool<MoneyUpdateRequestComponent> _moneyUpdateRequestPool;
        private int _ownerId;

        public HeroMoneyService(CurrencyModel currencyModel, EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
            _currencyModel = currencyModel;
        }

        public void Initialize() => _moneyUpdateRequestPool = _ecsWorld.GetPool<MoneyUpdateRequestComponent>();

        public void SetOwnerId(int id) => _ownerId = id;

        public void Set(int money) => _currencyModel.Money = money;

        public bool TryPurchase(int cost)
        {
            if (_currencyModel.Money < cost)
                return false;

            CreateUpdateMoneyRequest(-cost, _ownerId);
            return true;
        }

        private void CreateUpdateMoneyRequest(int cost, int heroId)
        {
            var request = _ecsWorld.NewEntity();
            _moneyUpdateRequestPool
                .Add(request)
                .Value = new MoneyUpdateRequest(heroId, cost);
        }
    }
}