using System.Collections.Generic;
using System.Linq;
using Code.Common.Components;
using Code.Common.Services;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Factory
{
    public class BusinessFactory
    {
        private readonly IIdentifierService _identifierService;
        private readonly EcsWorld _world;

        private readonly EcsPool<BusinessComponent> _businessPool;
        private readonly EcsPool<IncomeCooldownComponent> _cooldownPool;
        private readonly EcsPool<BusinessModifiersComponent> _modifiersPool;
        private readonly EcsPool<AccumulatedModifierComponent> _accumulatedModifierPool;
        private readonly EcsPool<BusinessIdComponent> _businessIdPool;
        private readonly EcsPool<IdComponent> _idPool;
        private readonly EcsPool<OwnerIdComponent> _ownerIdPool;
        private readonly EcsPool<LevelUpPriceComponent> _levelUpPricePool;
        private readonly EcsPool<PurchasedComponent> _purchasedPool;

        public BusinessFactory(EcsWorld world, IIdentifierService identifierService)
        {
            _world = world;
            _identifierService = identifierService;

            _businessPool = world.GetPool<BusinessComponent>();
            _cooldownPool = world.GetPool<IncomeCooldownComponent>();
            _modifiersPool = world.GetPool<BusinessModifiersComponent>();
            _accumulatedModifierPool = world.GetPool<AccumulatedModifierComponent>();
            _businessIdPool = world.GetPool<BusinessIdComponent>();
            _idPool = world.GetPool<IdComponent>();
            _ownerIdPool = world.GetPool<OwnerIdComponent>();
            _levelUpPricePool = world.GetPool<LevelUpPriceComponent>();
            _purchasedPool = world.GetPool<PurchasedComponent>();
        }

        public int CreateBusiness(BusinessData businessData, BusinessUpgradeNameData businessNameData,
            int businessIndex, int ownerId)
        {
            int entity = _world.NewEntity();

            // Создаем основной компонент бизнеса с объединенными данными
            ref var business = ref _businessPool.Add(entity);
            business.Id = businessIndex;
            business.Name = businessData.Name ?? businessNameData.Name;
            business.Level = businessIndex == 0 ? 1 : 0;
            business.BaseCost = businessData.BaseCost;
            business.BaseIncome = businessData.BaseIncome;
            business.CurrentIncome = businessData.BaseIncome;
            business.TotalIncome = 0;
            business.Progress = 0f;

            // Создаем компонент таймера
            ref var cooldown = ref _cooldownPool.Add(entity);
            cooldown.Duration = businessData.IncomeDelay;
            cooldown.TimeLeft = businessData.IncomeDelay;
            cooldown.IsAvailable = business.Level > 0;
            cooldown.IsCompleted = false;

            // Создаем компонент модификаторов
            ref var modifiers = ref _modifiersPool.Add(entity);
            modifiers.TotalMultiplier = 1.0f;
            modifiers.AccumulatedModifiers = new List<AccumulatedModifiersData>(2);
            modifiers.UpdateModifiers = new List<UpgradeData>(businessData.Upgrades.ToList());

            // Добавляем общие компоненты
            ref var id = ref _idPool.Add(entity);
            id.Value = _identifierService.Next();

            ref var ownerIdComponent = ref _ownerIdPool.Add(entity);
            ownerIdComponent.Value = ownerId;

            ref var levelUpPrice = ref _levelUpPricePool.Add(entity);
            levelUpPrice.Value = (business.Level + 1) * business.BaseCost;

            ref var purchased = ref _purchasedPool.Add(entity);
            purchased.Value = business.Level > 0;

            // Создаем компонент накопленных модификаторов
            ref var accumulatedModifier = ref _accumulatedModifierPool.Add(entity);
            accumulatedModifier.Value = 1.0f;
            accumulatedModifier.Modifiers = new List<AccumulatedModifiersData>();

            // Создаем компонент ID бизнеса
            ref var businessId = ref _businessIdPool.Add(entity);
            businessId.Value = businessIndex;

            return entity;
        }
    }
}