using Code.Common.Features;
using Code.Common.Services;
using Code.Gameplay.Save.Systems;
using Leopotam.EcsLite;

namespace Code.Gameplay.Save
{
    public class SaveFeature : Feature
    {
        private readonly ISaveService _saveService;
        private readonly ITimeService _timeService;

        public SaveFeature(EcsWorld world, IEcsSystems systems, ISaveService saveService, ITimeService timeService) : base(world, systems)
        {
            _saveService = saveService;
            _timeService = timeService;
        }

        public override void RegisterSystems()
        {
            Systems
                .Add(new CreateSaveRequestOnFocusChangedSystem())
                .Add(new CreateSaveRequestOnTimerSystem(_timeService))
                .Add(new SaveOnRequestSystem(_saveService));
        }
    }
} 