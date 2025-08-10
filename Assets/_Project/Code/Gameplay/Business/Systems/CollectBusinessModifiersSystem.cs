using System.Collections.Generic;
using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class CollectBusinessModifiersSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _businesses;
        private EcsFilter _upgradeModifiers;

        private EcsPool<IdComponent> _idPool;
        private EcsPool<UpgradeModifierComponent> _upgradeModifierPool;
        private EcsPool<BusinessModifiersComponent> _modifiersPool;
        private EcsPool<OwnerIdComponent> _ownerIdPool;
        private EcsPool<AccumulatedModifierComponent> _accumulatedModifiersPool;
        private EcsPool<UpgradeModifierIdComponent> _upgradeModifierIdPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            InitializeFilters();
            InitializePools();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int business in _businesses)
            {
                var id = _idPool.Get(business).Value;
                ref var modifiers = ref _accumulatedModifiersPool.Get(business).Modifiers;

                foreach (int modifier in _upgradeModifiers)
                {
                    var ownerId = _ownerIdPool.Get(modifier).Value;

                    if (ownerId == id)
                    {
                        int upgradeId = _upgradeModifierIdPool.Get(modifier).Value;
                        float upgradeValue = _upgradeModifierPool.Get(modifier).Multiplier;

                        if (UpgradeExistingModifier(modifiers, upgradeId,upgradeValue))
                            return;

                        AccumulatedModifiersData accumulatedModifiersData = new AccumulatedModifiersData()
                        {
                            Id = upgradeId,
                            Value = upgradeValue,
                            Purchased = true,
                        };
                        
                        modifiers.Add(accumulatedModifiersData);
                    }
                }
            }
        }

        private static bool UpgradeExistingModifier(List<AccumulatedModifiersData> modifiers, int upgradeId,
            float upgradeValue)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                if (modifiers[i].Id == upgradeId)
                {
                    var modifier = modifiers[i];
                    modifier.Purchased = true;
                    modifier.Value += upgradeValue;
                    modifiers[i] = modifier;
                    return true;
                }
            }

            return false;
        }

        private void InitializeFilters()
        {
            _businesses = _world.Filter<BusinessComponent>()
                .Inc<IdComponent>()
                .Inc<AccumulatedModifierComponent>()
                .End();

            _upgradeModifiers = _world.Filter<UpgradeModifierComponent>()
                .Inc<OwnerIdComponent>()
                .Inc<UpgradeModifierIdComponent>()
                .End();
        }

        private void InitializePools()
        {
            _idPool = _world.GetPool<IdComponent>();
            _ownerIdPool = _world.GetPool<OwnerIdComponent>();
            _upgradeModifierIdPool = _world.GetPool<UpgradeModifierIdComponent>();
            _upgradeModifierPool = _world.GetPool<UpgradeModifierComponent>();
            _accumulatedModifiersPool = _world.GetPool<AccumulatedModifierComponent>();
        }
    }
}