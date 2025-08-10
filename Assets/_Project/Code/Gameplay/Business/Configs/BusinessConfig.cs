using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Business.Configs
{
    [CreateAssetMenu(menuName = "Configs/BusinessConfig")]
    public class BusinessConfig : ScriptableObject
    {
        [SerializeField] List<BusinessData> _businessDatas = new();

        public IReadOnlyList<BusinessData> GetBusinessDatas() => _businessDatas;

        [ContextMenu("Заполнить по таблице")]
        private void FillFromTable()
        {
            _businessDatas = new List<BusinessData>
            {
                new BusinessData
                {
                    Name = "Лимонадный стенд",
                    IncomeDelay = 3f,
                    BaseCost = 3,
                    BaseIncome = 3,
                    Upgrades = new[]
                    {
                        new UpgradeData { Cost = 50, IncomeMultiplier = 0.5f },
                        new UpgradeData { Cost = 400, IncomeMultiplier = 1.0f },
                    }
                },
                new BusinessData
                {
                    Name = "Пиццерия",
                    IncomeDelay = 6f,
                    BaseCost = 40,
                    BaseIncome = 40,
                    Upgrades = new[]
                    {
                        new UpgradeData { Cost = 1200, IncomeMultiplier = 1.0f },
                        new UpgradeData { Cost = 4000, IncomeMultiplier = 2.0f },
                    }
                },
                new BusinessData
                {
                    Name = "Кафе",
                    IncomeDelay = 12f,
                    BaseCost = 200,
                    BaseIncome = 200,
                    Upgrades = new[]
                    {
                        new UpgradeData { Cost = 6000, IncomeMultiplier = 1.0f },
                        new UpgradeData { Cost = 20000, IncomeMultiplier = 1.5f },
                    }
                },
                new BusinessData
                {
                    Name = "Ресторан",
                    IncomeDelay = 17f,
                    BaseCost = 1000,
                    BaseIncome = 1000,
                    Upgrades = new[]
                    {
                        new UpgradeData { Cost = 15000, IncomeMultiplier = 1.0f },
                        new UpgradeData { Cost = 50000, IncomeMultiplier = 2.0f },
                    }
                },
                new BusinessData
                {
                    Name = "Отель",
                    IncomeDelay = 30f,
                    BaseCost = 5000,
                    BaseIncome = 5000,
                    Upgrades = new[]
                    {
                        new UpgradeData { Cost = 100000, IncomeMultiplier = 2.0f },
                        new UpgradeData { Cost = 500000, IncomeMultiplier = 4.0f },
                    }
                },
            };
        }
    }
}
