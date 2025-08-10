using Code.Gameplay.Business.Systems;
using Code.Common.Features;
using Code.Common.Services;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Business.Systems;
using Code.Gameplay.Save;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Features
{
    public class BusinessFeature : Feature
    {
        private readonly BusinessService _businessService;
        private readonly IIdentifierService _identifierService;
        private readonly BusinessUpgradeNamesConfig _businessUpgradeNamesConfig;
        private readonly BusinessConfig _businessConfig;
        private readonly ISaveService _saveService;
        private readonly ITimeService _timeService;

        public BusinessFeature(
            EcsWorld world, 
            IEcsSystems systems,
            BusinessService businessService,
            IIdentifierService identifierService,
            BusinessUpgradeNamesConfig businessUpgradeNamesConfig,
            BusinessConfig businessConfig, 
            ISaveService saveService, ITimeService timeService) 
            : base(world, systems)
        {
            _businessService = businessService;
            _identifierService = identifierService;
            _businessUpgradeNamesConfig = businessUpgradeNamesConfig;
            _businessConfig = businessConfig;
            _saveService = saveService;
            _timeService = timeService;
        }

        public override void RegisterSystems()
        {
            Systems
                .Add(new BusinessInitSystem(_businessUpgradeNamesConfig, _identifierService, _businessConfig, _businessService, _saveService))
                .Add(new CalculateIncomeCooldownSystem(_timeService))
                .Add(new UpdateBusinessOnLevelUpSystem())
                .Add(new CreateUpgradeModifierOnUpgradePurchasedSystem())
                .Add(new CollectBusinessModifiersSystem())
                .Add(new CalculateBusinessProgressSystem(_businessService))
                .Add(new CalculateTotalIncomeOnCooldownUpSystem())
                .Add(new RecalculateBusinessValuesSystem(_businessService))
                .Add(new CreateSaveRequestOnBusinessUpdateSystem())
                .Add(new CleanupBusinessRequestsSystem())
                .Add(new CleanupUpgradeModifiersSystem())
                ;
        }
    }
} 