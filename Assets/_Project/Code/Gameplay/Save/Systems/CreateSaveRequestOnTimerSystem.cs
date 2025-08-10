using Code.Common.Services;
using Code.Gameplay.Save.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Save.Systems
{
    public class CreateSaveRequestOnTimerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float SaveInterval = 5f;
        
        private readonly ITimeService _timeService;
        
        private EcsWorld _world;
        private float _timeSinceLastSave;
        
        public CreateSaveRequestOnTimerSystem(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _timeSinceLastSave = 0f;
        }

        public void Run(IEcsSystems systems)
        {
            _timeSinceLastSave += _timeService.DeltaTime;
            
            if (_timeSinceLastSave >= SaveInterval)
            {
                 SendSaveRequest();
                _timeSinceLastSave = 0f;
            }
        }

        private void SendSaveRequest()
        {
            int entity = _world.NewEntity();
            _world.GetPool<SaveRequestComponent>().Add(entity);
        }
    }
} 