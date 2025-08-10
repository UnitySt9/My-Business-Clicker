using Code.Gameplay.Business.Components;
using Code.Gameplay.Save.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CreateSaveRequestOnBusinessUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _updateLevelBusinessRequests;
        private EcsFilter _updateBusinessUpgradesRequests;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

           _updateLevelBusinessRequests = _world
               .Filter<LevelUpRequestComponent>()
               .End();
           
           _updateBusinessUpgradesRequests = _world
               .Filter<UpgradePurchasedRequestComponent>()
               .End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_updateBusinessUpgradesRequests.GetEntitiesCount() > 0 || _updateLevelBusinessRequests.GetEntitiesCount() > 0)
            {
                SendSaveRequest();
            }
        }

        private void SendSaveRequest()
        {
            int entity = _world.NewEntity();
            _world.GetPool<SaveRequestComponent>().Add(entity);
        }
    }
}