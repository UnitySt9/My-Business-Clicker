using TMPro;
using UnityEngine;

namespace Code.UI.Money
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private CurrencyScreenModel _currencyScreenModel;

        public void Initialize(CurrencyScreenModel currencyScreenModel)
        {
            _currencyScreenModel = currencyScreenModel;
            _text.text = $"Баланс: {_currencyScreenModel.Currency}$";
            _currencyScreenModel.CurrencyChanged += OnCurrencyChanged;
        }

        private void OnCurrencyChanged(int newValue)
        {
            _text.text = $"Баланс: {newValue}$";
        }

        private void OnDestroy()
        {
            if (_currencyScreenModel != null)
            {
                _currencyScreenModel.CurrencyChanged -= OnCurrencyChanged;
            }
        }
    }
}