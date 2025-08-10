using Leopotam.EcsLite;

namespace Code.Common.Features
{
    public abstract class Feature
    {
        protected readonly EcsWorld World;
        protected readonly IEcsSystems Systems;

        protected Feature(EcsWorld world, IEcsSystems systems)
        {
            World = world;
            Systems = systems;
        }

        public abstract void RegisterSystems();
    }
} 