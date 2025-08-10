using System.Collections.Generic;
using Code.Common.Components;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Hero.Components;
using Code.Gameplay.Money.Components;
using Code.Gameplay.Save.Components;
using Code.Gameplay.Save.Models;
using Leopotam.EcsLite;

namespace Code.Gameplay.Save.Systems
{
    public class SaveOnRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ISaveService _saveService;
        private EcsWorld _world;

        private EcsFilter _heroFilter;
        private EcsFilter _businessFilter;
        private EcsFilter _saveRequestFilter;

        private EcsPool<MoneyComponent> _moneyPool;
        private EcsPool<LevelComponent> _levelPool;
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<ProgressComponent> _progressPool;
        private EcsPool<IncomeCooldownComponent> _cooldownPool2;
        private EcsPool<IncomeCooldownComponent> _cooldownPool;
        private EcsPool<IncomeCooldownComponent> _cooldownLeftPool;
        private EcsPool<LevelUpPriceComponent> _levelUpPricePool;
        private EcsPool<PurchasedComponent> _purchasedPool;
        private EcsPool<BusinessModifiersComponent> _modifiersPool;
        private EcsPool<BusinessIdComponent> _businessIdPool;
        private EcsPool<BusinessComponent> _incomePool;
        private EcsPool<PurchasedComponent> _cooldownAvailablePool;
        private EcsPool<AccumulatedModifierComponent> _accumulatedModifierComponent;

        public SaveOnRequestSystem(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _heroFilter = _world.Filter<HeroComponent>().End();
            _businessFilter = _world.Filter<BusinessComponent>().End();
            _saveRequestFilter = _world.Filter<SaveRequestComponent>().End();

            _moneyPool = _world.GetPool<MoneyComponent>();
            _levelPool = _world.GetPool<LevelComponent>();
            _businessPool = _world.GetPool<BusinessComponent>();
            _progressPool = _world.GetPool<ProgressComponent>();
            _cooldownPool2 = _world.GetPool<IncomeCooldownComponent>();
            _cooldownPool = _world.GetPool<IncomeCooldownComponent>();
            _cooldownLeftPool = _world.GetPool<IncomeCooldownComponent>();
            _levelUpPricePool = _world.GetPool<LevelUpPriceComponent>();
            _purchasedPool = _world.GetPool<PurchasedComponent>();
            _modifiersPool = _world.GetPool<BusinessModifiersComponent>();
            _businessIdPool = _world.GetPool<BusinessIdComponent>();
            _incomePool = _world.GetPool<BusinessComponent>();
            _cooldownAvailablePool = _world.GetPool<PurchasedComponent>();
            _accumulatedModifierComponent = _world.GetPool<AccumulatedModifierComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var request in _saveRequestFilter)
            {
                SaveGame();
                _world.DelEntity(request);
            }
        }

        private void SaveGame()
        {
            var saveData = new GameSaveModel
            {
                Hero = new HeroSaveModel()
            };

            SaveHero(saveData);

            SaveBusinesses(saveData);

            _saveService.SaveGame(saveData);
        }

        private void SaveBusinesses(GameSaveModel saveData)
        {
            foreach (var business in _businessFilter)
            {
                float totalCooldown = _cooldownPool.Get(business).Duration;
                float currentCooldown = _cooldownPool.Get(business).TimeLeft;
                float progress = 1f - (currentCooldown / totalCooldown);

                var businessSave = new BusinessSaveModel
                {
                    Id = _businessIdPool.Get(business).Value,
                    Level = _businessPool.Get(business).Level,
                    Progress = progress,
                    Cooldown = currentCooldown,
                    Income = _incomePool.Get(business).CurrentIncome,
                    LevelUpPrice = _levelUpPricePool.Get(business).Value,
                    IsPurchased = _purchasedPool.Has(business) && _purchasedPool.Get(business).Value,
                    IsCooldownAvailable = _cooldownAvailablePool.Has(business) &&
                                          _cooldownAvailablePool.Get(business).Value
                };

                if (_accumulatedModifierComponent.Has(business))
                {
                    List<AccumulatedModifiersData> accumulatedModifiersDatas =
                        _accumulatedModifierComponent.Get(business).Modifiers;

                    for (int i = 0; i < accumulatedModifiersDatas.Count; i++)
                    {
                        if (accumulatedModifiersDatas[i].Value > 0)
                        {
                            var saveModel = new UpgradeSaveModel()
                            {
                                Id = accumulatedModifiersDatas[i].Id,
                                IncomeModifier = accumulatedModifiersDatas[i].Value,
                                Purchased = accumulatedModifiersDatas[i].Purchased
                            };

                            businessSave.Upgrades.Add(saveModel);
                        }
                    }
                }

                saveData.Businesses.Add(businessSave);
            }
        }

        private void SaveHero(GameSaveModel saveData)
        {
            foreach (var hero in _heroFilter)
            {
                saveData.Hero.Money = _moneyPool.Get(hero).Value;
            }
        }
    }
}