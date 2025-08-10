using System;
using Code.Gameplay.Money;

namespace Code.UI.Money
{
    public class CurrencyScreenModel : IDisposable
    {
        private readonly CurrencyModel _currencyModel;
        
        public event Action<int> CurrencyChanged;
        public int Currency => _currencyModel.Money;

        public CurrencyScreenModel(CurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
            _currencyModel.MoneyChanged += OnMoneyChanged;
        }

        private void OnMoneyChanged(int amount)
        {
            CurrencyChanged?.Invoke(amount);
        }

        public void Dispose()
        {
            _currencyModel.MoneyChanged -= OnMoneyChanged;
        }
    }
}