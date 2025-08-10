using System.Collections.Generic;
using Code.Common.Components;
using Code.Common.Services;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Business.Factory;
using Code.Gameplay.Hero.Components;
using Code.Gameplay.Save;
using Code.Gameplay.Save.Models;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Systems
{
    public class BusinessInitSystem : IEcsInitSystem
    {
        private readonly BusinessConfig _businessConfig;
        private readonly BusinessUpgradeNamesConfig _businessUpgradeNamesConfig;
        private readonly BusinessService _businessService;
        private readonly IIdentifierService _identifierService;
        private readonly ISaveService _saveService;

        private BusinessFactory _businessFactory;

        private EcsPool<IdComponent> _idPool;
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<IncomeCooldownComponent> _cooldownPool;
        private EcsPool<BusinessModifiersComponent> _modifiersPool;
        private EcsPool<AccumulatedModifierComponent> _accumulatedModifierPool;
        private EcsPool<BusinessIdComponent> _businessIdPool;
        private EcsPool<LevelUpPriceComponent> _levelUpPricePool;
        private EcsPool<PurchasedComponent> _purchasedPool;

        public BusinessInitSystem(
            BusinessUpgradeNamesConfig businessUpgradeNamesConfig,
            IIdentifierService identifierService,
            BusinessConfig businessConfig,
            BusinessService businessService,
            ISaveService saveService)
        {
            _identifierService = identifierService;
            _businessUpgradeNamesConfig = businessUpgradeNamesConfig;
            _businessConfig = businessConfig;
            _businessService = businessService;
            _saveService = saveService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _businessFactory = new BusinessFactory(world, _identifierService);
            InitializePools(world);

            var heroId = GetHeroId(world);
            var businessDatas = _businessConfig.GetBusinessDatas();
            var saveData = _saveService.HasSave() ? _saveService.LoadGame() : null;

            InitializeBusinesses(heroId, businessDatas, saveData);
        }

        private void InitializeBusinesses(int heroId, IReadOnlyList<BusinessData> businessDatas, GameSaveModel saveData)
        {
            for (int i = 0; i < businessDatas.Count; i++)
            {
                var businessData = businessDatas[i];
                var businessNameData = _businessUpgradeNamesConfig.BusinessUpgradeNameDatas[i];

                int entity = _businessFactory.CreateBusiness(businessData, businessNameData, i, heroId);

                if (saveData != null)
                    RestoreBusinessState(entity, saveData.Businesses.Find(b => b.Id == i));

                NotifyBusinessDataUpdated(entity, businessNameData.Name);
            }
        }

        private void RestoreBusinessState(int entity, BusinessSaveModel savedBusiness)
        {
            if (savedBusiness == null)
                return;

            // Убеждаемся, что у сущности есть AccumulatedModifierComponent
            if (!_accumulatedModifierPool.Has(entity))
            {
                ref var accumulatedModifier = ref _accumulatedModifierPool.Add(entity);
                accumulatedModifier.Value = 1.0f;
                accumulatedModifier.Modifiers = new List<AccumulatedModifiersData>();
            }

            // Убеждаемся, что у сущности есть BusinessIdComponent
            if (!_businessIdPool.Has(entity))
            {
                ref var businessId = ref _businessIdPool.Add(entity);
                businessId.Value = savedBusiness?.Id ?? 0;
            }

            RestoreBasicProperties(entity, savedBusiness);
            RestoreBusinessFlags(entity, savedBusiness);
            RestoreUpgrades(entity, savedBusiness);
        }

        private void RestoreBasicProperties(int entity, BusinessSaveModel savedBusiness)
        {
            ref var business = ref _businessPool.Get(entity);
            business.Level = savedBusiness.Level;
            business.CurrentIncome = savedBusiness.Income;
            business.Progress = savedBusiness.Progress;
            
            ref var cooldown = ref _cooldownPool.Get(entity);
            cooldown.TimeLeft = savedBusiness.Cooldown;
            
            _levelUpPricePool.Get(entity).Value = savedBusiness.LevelUpPrice;
        }

        private void RestoreBusinessFlags(int entity, BusinessSaveModel savedBusiness)
        {
            if (savedBusiness == null)
                return;

            if (_purchasedPool.Has(entity))
            {
                _purchasedPool.Get(entity).Value = savedBusiness.IsPurchased;
            }
            else
            {
                _purchasedPool.Add(entity).Value = savedBusiness.IsPurchased;
            }

            if (savedBusiness.IsCooldownAvailable && _cooldownPool.Has(entity))
            {
                ref var cooldown = ref _cooldownPool.Get(entity);
                cooldown.IsAvailable = true;
            }
        }

        private void RestoreUpgrades(int entity, BusinessSaveModel savedBusiness)
        {
            if (!_modifiersPool.Has(entity))
                return;

            ref var modifiers = ref _modifiersPool.Get(entity);
            modifiers.AccumulatedModifiers.Clear();

            foreach (UpgradeSaveModel savedUpgrade in savedBusiness.Upgrades)
            {
                modifiers.AccumulatedModifiers.Add(new AccumulatedModifiersData
                {
                    Id = savedUpgrade.Id,
                    Value = savedUpgrade.IncomeModifier,
                    Purchased = savedUpgrade.Purchased
                });
            }
        }

        private void InitializePools(EcsWorld world)
        {
            _idPool = world.GetPool<IdComponent>();
            _businessPool = world.GetPool<BusinessComponent>();
            _cooldownPool = world.GetPool<IncomeCooldownComponent>();
            _modifiersPool = world.GetPool<BusinessModifiersComponent>();
            _accumulatedModifierPool = world.GetPool<AccumulatedModifierComponent>();
            _businessIdPool = world.GetPool<BusinessIdComponent>();
            _levelUpPricePool = world.GetPool<LevelUpPriceComponent>();
            _purchasedPool = world.GetPool<PurchasedComponent>();
        }

        private int GetHeroId(EcsWorld world)
        {
            EcsFilter heroFilter = world.Filter<HeroComponent>().End();

            foreach (int hero in heroFilter)
                return _idPool.Get(hero).Value;

            return -1;
        }

        private void NotifyBusinessDataUpdated(int entity, string name)
        {
            var business = _businessPool.Get(entity);
            var levelUpPrice = _levelUpPricePool.Get(entity).Value;
            var modifiers = _modifiersPool.Get(entity);
            
            _businessService.NotifyBusinessDataUpdated(business.Id, business.Level, business.CurrentIncome, levelUpPrice, name, modifiers.AccumulatedModifiers);
        }
    }
}