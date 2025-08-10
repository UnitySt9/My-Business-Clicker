using Code.Common.Components;
using Code.Common.Services;
using Code.Gameplay.Hero.Components;
using Code.Gameplay.Money.Components;
using Code.Gameplay.Save;
using Leopotam.EcsLite;

namespace Code.Gameplay.Hero.Systems
{
    public class HeroInitSystem : IEcsInitSystem
    {
        private readonly IIdentifierService _identifierService;
        private readonly ISaveService _saveService;

        public HeroInitSystem(IIdentifierService identifierService, ISaveService saveService)
        {
            _identifierService = identifierService;
            _saveService = saveService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            int hero = world.NewEntity();

            world.GetPool<HeroComponent>()
                .Add(hero)
                .Value = true;

            ref var moneyComponent = ref world.GetPool<MoneyComponent>().Add(hero);

            moneyComponent.Value = _saveService.HasSave()
                ? _saveService.LoadGame().Hero.Money
                : 0;

            world.GetPool<IdComponent>()
                .Add(hero)
                .Value = _identifierService.Next();
        }
    }
}