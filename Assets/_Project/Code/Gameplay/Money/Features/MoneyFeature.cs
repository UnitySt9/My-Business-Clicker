using Code.Common.Features;
using Code.Gameplay.Business.Systems;
using Code.Gameplay.Hero.Systems;
using Code.Gameplay.Money.Systems;
using Leopotam.EcsLite;

namespace Code.Gameplay.Money.Features
{
    public class MoneyFeature : Feature
    {
        private readonly IMoneyService _moneyService;

        public MoneyFeature(EcsWorld world, IEcsSystems systems, IMoneyService moneyService) 
            : base(world, systems)
        {
            _moneyService = moneyService;
        }

        public override void RegisterSystems()
        {
            Systems
                .Add(new CreateMoneyUpdateRequestOnIncomeCooldownUpSystem())
                .Add(new UpdateMoneyOnRequestSystem())
                .Add(new UpdateHeroMoneySystem(_moneyService))
                .Add(new CleanupMoneyRequestsSystem());
        }
    }
} 