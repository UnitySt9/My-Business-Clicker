using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Code.Gameplay.Business;

namespace Code.UI
{
    public class BusinessScreenModel : IUIModel
    {
        public event Action<int> LevelChanged;
        public event Action<string> NameChanged;
        public event Action<float> ProgressChanged;
        public event Action<int> IncomeChanged;
        public event Action<int> LevelUpPriceChanged;
        
        private readonly BusinessService _businessService;
        private readonly int _businessId;
        private readonly ObservableCollection<UpgradeBusinessScreenModel> _upgradeBusinessScreenModels;

        public BusinessScreenModel(BusinessService businessService, int businessId, List<UpgradeBusinessScreenModel> upgradeBusinessScreenModels)
        {
            _businessService = businessService;
            _businessId = businessId;
            _upgradeBusinessScreenModels = new ObservableCollection<UpgradeBusinessScreenModel>(upgradeBusinessScreenModels);

            _businessService.LevelChanged += OnLevelChanged;
            _businessService.NameChanged += OnNameChanged;
            _businessService.ProgressChanged += OnProgressChanged;
            _businessService.IncomeChanged += OnIncomeChanged;
            _businessService.LevelUpPriceChanged += OnLevelUpPriceChanged;
        }

        public int Level => _businessService.GetLevel(_businessId);
        public string Name => _businessService.GetName(_businessId);
        public float Progress => _businessService.GetProgress(_businessId);
        public int Income => _businessService.GetIncome(_businessId);
        public int LevelUpPrice => _businessService.GetLevelUpPrice(_businessId);
        
        public ReadOnlyObservableCollection<UpgradeBusinessScreenModel> UpgradeBusinessScreenModels => 
            new ReadOnlyObservableCollection<UpgradeBusinessScreenModel>(_upgradeBusinessScreenModels);
        
        public void OnLevelUpButtonClicked()
        {
            _businessService.TryPurchaseLevelUp(_businessId, LevelUpPrice, 1);
        }

        public void Dispose()
        {
            _businessService.LevelChanged -= OnLevelChanged;
            _businessService.NameChanged -= OnNameChanged;
            _businessService.ProgressChanged -= OnProgressChanged;
            _businessService.IncomeChanged -= OnIncomeChanged;
            _businessService.LevelUpPriceChanged -= OnLevelUpPriceChanged;

            foreach (var model in _upgradeBusinessScreenModels)
            {
                model.Dispose();
            }
        }

        private void OnLevelChanged(int id, int level)
        {
            if (id == _businessId)
                LevelChanged?.Invoke(level);
        }

        private void OnNameChanged(int id, string name)
        {
            if (id == _businessId)
                NameChanged?.Invoke(name);
        }

        private void OnProgressChanged(int id, float progress)
        {
            if (id == _businessId)
                ProgressChanged?.Invoke(progress);
        }

        private void OnIncomeChanged(int id, int income)
        {
            if (id == _businessId)
                IncomeChanged?.Invoke(income);
        }

        private void OnLevelUpPriceChanged(int id, int price)
        {
            if (id == _businessId)
                LevelUpPriceChanged?.Invoke(price);
        }
    }
}