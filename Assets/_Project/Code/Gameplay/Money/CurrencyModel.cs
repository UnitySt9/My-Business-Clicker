using System;

namespace Code.Gameplay.Money
{
    public class CurrencyModel
    {
        private int _money;
        
        public event Action<int> MoneyChanged;

        public int Money
        {
            get => _money;
            set
            {
                if (_money != value)
                {
                    _money = value;
                    MoneyChanged?.Invoke(_money);
                }
            }
        }

        public CurrencyModel(int value = 0)
        {
            _money = value;
        }
    }
}