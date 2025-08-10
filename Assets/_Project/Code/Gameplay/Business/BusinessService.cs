using System;
using System.Collections.Generic;
using Code.Common;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Money;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business
{
    public class BusinessService : IInitializable
    {
        public event Action<int, int> LevelChanged;
        public event Action<int, string> NameChanged;
        public event Action<int, float> ProgressChanged;
        public event Action<int, int> IncomeChanged;
        public event Action<int, int> LevelUpPriceChanged;
        public event Action<int, bool> PurchasedChanged;
        public event Action<int, int, bool> BusinessModifierStateChanged;

        private readonly BusinessProperties _properties;
        private readonly BusinessRequestCreator _requestCreator;

        public BusinessService(EcsWorld ecsWorld, IMoneyService moneyService)
        {
            _properties = new BusinessProperties();
            _properties.LevelChanged += OnLevelChanged;
            _properties.NameChanged += OnNameChanged;
            _properties.ProgressChanged += OnProgressChanged;
            _properties.IncomeChanged += OnIncomeChanged;
            _properties.LevelUpPriceChanged += OnLevelUpPriceChanged;
            _properties.PurchasedChanged += OnPurchasedChanged;
            _properties.BusinessModifierStateChanged += OnBusinessModifierStateChanged;
            
            _requestCreator = new BusinessRequestCreator(ecsWorld, moneyService);
        }

        public void Initialize()
        {
            _requestCreator.Initialize();
        }

        public int GetLevel(int id) => _properties.GetLevel(id);
        public string GetName(int id) => _properties.GetName(id);
        public float GetProgress(int id) => _properties.GetProgress(id);
        public int GetIncome(int id) => _properties.GetIncome(id);
        public int GetLevelUpPrice(int id) => _properties.GetLevelUpPrice(id);
        public bool GetPurchased(int id) => _properties.GetPurchased(id);

        public bool TryPurchaseLevelUp(int id, int levelPrice, int level)
        {
            return _requestCreator.TryPurchaseLevelUp(id, levelPrice, level);
        }

        public void UpdateBusinessProgress(int id, float progress)
        {
            _properties.UpdateProgress(id, progress);
        }

        public void NotifyBusinessDataUpdated(int id, int level, int income, int levelUpPrice, string name, List<AccumulatedModifiersData> upgrades = null)
        {
            _properties.UpdateBusinessData(id, level, income, levelUpPrice, name, upgrades);
        }

        public bool TryPurchaseUpgrade(int businessId, int upgradeId, int price)
        {
            return _requestCreator.TryPurchaseUpgrade(businessId, upgradeId, price);
        }

        private void OnLevelChanged(int id, int level) => LevelChanged?.Invoke(id, level);
        private void OnNameChanged(int id, string name) => NameChanged?.Invoke(id, name);
        private void OnProgressChanged(int id, float progress) => ProgressChanged?.Invoke(id, progress);
        private void OnIncomeChanged(int id, int income) => IncomeChanged?.Invoke(id, income);
        private void OnLevelUpPriceChanged(int id, int price) => LevelUpPriceChanged?.Invoke(id, price);
        private void OnPurchasedChanged(int id, bool purchased) => PurchasedChanged?.Invoke(id, purchased);
        private void OnBusinessModifierStateChanged(int businessId, int modifierId, bool purchased) => 
            BusinessModifierStateChanged?.Invoke(businessId, modifierId, purchased);
    }
}