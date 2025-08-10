using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CalculateBusinessProgressSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly BusinessService _businessService;

        private EcsFilter _businesses;
        private EcsWorld _world;
        private EcsPool<IncomeCooldownComponent> _cooldownPool;
        private EcsPool<BusinessComponent> _businessPool;

        public CalculateBusinessProgressSystem(BusinessService businessService)
        {
            _businessService = businessService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _businesses = _world
                .Filter<BusinessComponent>()
                .Inc<IncomeCooldownComponent>()
                .Inc<PurchasedComponent>()
                .Inc<IdComponent>()
                .End();

            _cooldownPool = _world.GetPool<IncomeCooldownComponent>();
            _businessPool = _world.GetPool<BusinessComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int business in _businesses)
            {
                var cooldown = _cooldownPool.Get(business);
                ref var businessComponent = ref _businessPool.Get(business);

                float progress = 1f - (cooldown.TimeLeft / cooldown.Duration);

                businessComponent.Progress = progress;

                _businessService.UpdateBusinessProgress(businessComponent.Id, progress);
            }
        }
    }
}