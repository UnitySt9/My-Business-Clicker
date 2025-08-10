using Code.Common.Features;
using Code.Common.Services;
using Code.Gameplay.Hero.Systems;
using Code.Gameplay.Save;
using Leopotam.EcsLite;

namespace Code.Gameplay.Hero.Features
{
    public class HeroFeature : Feature
    {
        private readonly IIdentifierService _identifierService;
        private readonly ISaveService _saveService;

        public HeroFeature(EcsWorld world, IEcsSystems systems, IIdentifierService identifierService,ISaveService saveService) 
            : base(world, systems)
        {
            _saveService = saveService;
            _identifierService = identifierService;
        }

        public override void RegisterSystems()
        {
            Systems
                .Add(new HeroInitSystem(_identifierService,_saveService));
        }
    }
} 