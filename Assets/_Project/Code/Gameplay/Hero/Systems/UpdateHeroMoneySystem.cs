using Code.Common.Components;
using Code.Gameplay.Hero.Components;
using Code.Gameplay.Money;
using Code.Gameplay.Money.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Hero.Systems
{
    public class UpdateHeroMoneySystem : IEcsInitSystem, IEcsPostRunSystem
    {
        private readonly IMoneyService _heroMoneyService;

        private EcsWorld _world;
        private EcsFilter _hero;
        private EcsPool<MoneyComponent> _moneyPool;
        private EcsPool<IdComponent> _idPool;

        public UpdateHeroMoneySystem(IMoneyService heroMoneyService)
        {
            _heroMoneyService = heroMoneyService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _hero = _world.Filter<MoneyComponent>()
                .Inc<IdComponent>()
                .Inc<HeroComponent>()
                .End();

            _idPool = _world.GetPool<IdComponent>();

            _moneyPool = _world.GetPool<MoneyComponent>();
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (int hero in _hero)
            {
                int targetBalance = _moneyPool.Get(hero).Value;
                int heroId = _idPool.Get(hero).Value;

                _heroMoneyService.SetOwnerId(heroId);
                _heroMoneyService.Set(targetBalance);
            }
        }
    }
}