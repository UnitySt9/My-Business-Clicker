using System;
using Code.Gameplay.Business;

namespace Code.UI
{
    public class UpgradeBusinessScreenModel : IUIModel
    {
        public event Action<bool> PurchasedChanged;
        public event Action<float> IncomeMultiplierChanged;
        public event Action<int> PriceChanged;
        public event Action<string> NameChanged;
        public event Action<bool> PurchaseAvailableChanged;

        private readonly BusinessService _businessService;
        private bool _purchased;
        private float _incomeMultiplier;
        private int _price;
        private string _name;
        private int _id;
        private int _businessId;

        public UpgradeBusinessScreenModel(bool purchased, float income, int price, string name, int id, int businessId, BusinessService businessService)
        {
            _businessService = businessService;
            _purchased = purchased;
            _incomeMultiplier = income;
            _price = price;
            _name = name;
            _id = id;
            _businessId = businessId;

            _businessService.BusinessModifierStateChanged += OnBusinessModifierStateChanged;
            _businessService.PurchasedChanged += OnBusinessPurchasedChanged;
        }

        public bool Purchased => _purchased;
        public float IncomeMultiplier => _incomeMultiplier;
        public int Price => _price;
        public string Name => _name;
        public bool PurchaseAvailable => _businessService.GetPurchased(_businessId);

        public void OnUpgradeClicked()
        {
            if (_businessService.TryPurchaseUpgrade(_businessId, _id, _price))
            {
                _purchased = true;
                PurchasedChanged?.Invoke(true);
            }
        }

        public void Dispose()
        {
            _businessService.BusinessModifierStateChanged -= OnBusinessModifierStateChanged;
            _businessService.PurchasedChanged -= OnBusinessPurchasedChanged;
        }

        private void OnBusinessModifierStateChanged(int businessId, int modifierId, bool purchased)
        {
            if (businessId == _businessId && modifierId == _id)
            {
                _purchased = purchased;
                PurchasedChanged?.Invoke(purchased);
            }
        }

        private void OnBusinessPurchasedChanged(int businessId, bool purchased)
        {
            if (businessId == _businessId)
            {
                PurchaseAvailableChanged?.Invoke(purchased);
            }
        }
    }
}