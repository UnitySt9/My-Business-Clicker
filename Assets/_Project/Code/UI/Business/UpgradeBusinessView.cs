using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class UpgradeBusinessView : MonoBehaviour
    {
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _incomeMultiplierText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _purchasedText;

        private UpgradeBusinessScreenModel _model;

        public void Initialize(UpgradeBusinessScreenModel model)
        {
            _model = model;
            
            _nameText.text = model.Name;
            _incomeMultiplierText.text = $"Доход: + {model.IncomeMultiplier * 100:F0}%"; // Изменено здесь
            
            if (_priceText.gameObject.activeSelf)
                _priceText.text = $"Цена: {model.Price}$";

            SetupAfterPurchasing(model.Purchased);
            
            _model.PurchasedChanged += SetupAfterPurchasing;
            _model.PurchaseAvailableChanged += OnPurchaseAvailableChanged;

            _upgradeButton.onClick.AddListener(model.OnUpgradeClicked);
            
            UpdateButtonInteractable();
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.PurchasedChanged -= SetupAfterPurchasing;
                _model.PurchaseAvailableChanged -= OnPurchaseAvailableChanged;
                
                _upgradeButton.onClick.RemoveAllListeners();
            }
        }

        private void SetupAfterPurchasing(bool purchased)
        {
            _priceText.gameObject.SetActive(!purchased);
            _purchasedText.gameObject.SetActive(purchased);
            UpdateButtonInteractable();
        }

        private void OnPurchaseAvailableChanged(bool available)
        {
            UpdateButtonInteractable();
        }

        private void UpdateButtonInteractable()
        {
            _upgradeButton.interactable = _model.PurchaseAvailable && !_model.Purchased;
        }
    }
}