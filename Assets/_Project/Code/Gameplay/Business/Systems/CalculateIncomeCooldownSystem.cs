using Code.Common.Components;
using Code.Common.Services;
using Code.Gameplay.Business.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CalculateIncomeCooldownSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly ITimeService _timeService;
        
        private EcsFilter _incomeCooldowns;
        private EcsWorld _world;
        private EcsPool<IncomeCooldownComponent> _cooldownPool;
        
        public CalculateIncomeCooldownSystem(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _incomeCooldowns = _world
                .Filter<BusinessComponent>()
                .Inc<IncomeCooldownComponent>()
                .Inc<IdComponent>()
                .End();

            _cooldownPool = _world.GetPool<IncomeCooldownComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _incomeCooldowns)
            {
                ref var cooldown = ref _cooldownPool.Get(entity);

                if (!cooldown.IsAvailable)
                    continue;

                cooldown.TimeLeft -= _timeService.DeltaTime; 

                if (cooldown.TimeLeft <= 0)
                {
                    if (!cooldown.IsCompleted)
                        cooldown.IsCompleted = true;
                    
                    cooldown.TimeLeft = cooldown.Duration;
                }
                else
                {
                    if (cooldown.IsCompleted)
                        cooldown.IsCompleted = false;
                }
            }
        }
    }
}
