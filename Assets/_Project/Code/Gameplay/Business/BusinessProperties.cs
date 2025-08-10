using System;
using System.Collections.Generic;
using Code.Gameplay.Business.Configs;

namespace Code.Gameplay.Business
{
    public class BusinessProperties
    {
        public event Action<int, int> LevelChanged;
        public event Action<int, string> NameChanged;
        public event Action<int, float> ProgressChanged;
        public event Action<int, int> IncomeChanged;
        public event Action<int, int> LevelUpPriceChanged;
        public event Action<int, bool> PurchasedChanged;
        public event Action<int, int, bool> BusinessModifierStateChanged;

        private readonly Dictionary<int, int> _levels = new();
        private readonly Dictionary<int, string> _names = new();
        private readonly Dictionary<int, float> _progresses = new();
        private readonly Dictionary<int, int> _incomes = new();
        private readonly Dictionary<int, int> _levelUpPrices = new();
        private readonly Dictionary<int, bool> _purchasedStates = new();

        public int GetLevel(int id) => _levels.TryGetValue(id, out var level) ? level : 0;
        public string GetName(int id) => _names.TryGetValue(id, out var name) ? name : string.Empty;
        public float GetProgress(int id) => _progresses.TryGetValue(id, out var progress) ? progress : 0f;
        public int GetIncome(int id) => _incomes.TryGetValue(id, out var income) ? income : 0;
        public int GetLevelUpPrice(int id) => _levelUpPrices.TryGetValue(id, out var price) ? price : 0;
        public bool GetPurchased(int id) => _purchasedStates.TryGetValue(id, out var purchased) && purchased;

        public void UpdateBusinessData(int id, int level, int income, int levelUpPrice, string name,
            List<AccumulatedModifiersData> upgrades = null)
        {
            if (level > -1)
            {
                _levels[id] = level;
                LevelChanged?.Invoke(id, level);
            }

            if (income > -1)
            {
                _incomes[id] = income;
                IncomeChanged?.Invoke(id, income);
            }

            if (levelUpPrice > -1)
            {
                _levelUpPrices[id] = levelUpPrice;
                LevelUpPriceChanged?.Invoke(id, levelUpPrice);
            }

            if (!string.IsNullOrEmpty(name))
            {
                _names[id] = name;
                NameChanged?.Invoke(id, name);
            }

            if (upgrades != null)
            {
                foreach (var upgrade in upgrades)
                {
                    BusinessModifierStateChanged?.Invoke(id, upgrade.Id, upgrade.Purchased);
                }
            }

            var purchased = level > 0;
            _purchasedStates[id] = purchased;
            PurchasedChanged?.Invoke(id, purchased);
        }

        public void UpdateProgress(int id, float progress)
        {
            _progresses[id] = progress;
            ProgressChanged?.Invoke(id, progress);
        }
    }
}