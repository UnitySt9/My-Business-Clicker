using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class BusinessView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private TMP_Text _levelUpPriceText;
        [SerializeField] private List<UpgradeBusinessView> _upgradeBusinessViews;

        private BusinessScreenModel _model;

        public void Initialize(BusinessScreenModel model)
        {
            _model = model;

            // Initial setup
            _levelText.text = $"LVL: \n{model.Level}";
            _nameText.text = model.Name;
            _progressBar.value = model.Progress;
            _levelUpPriceText.text = $"LVL UP: \n{model.LevelUpPrice}$";
            _incomeText.text = $"ДОХОД: \n{model.Income}$";

            // Subscribe to events
            _model.LevelChanged += OnLevelChanged;
            _model.NameChanged += OnNameChanged;
            _model.ProgressChanged += OnProgressChanged;
            _model.IncomeChanged += OnIncomeChanged;
            _model.LevelUpPriceChanged += OnLevelUpPriceChanged;

            _levelUpButton.onClick.AddListener(model.OnLevelUpButtonClicked);

            InitUpgradeViews(model);
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.LevelChanged -= OnLevelChanged;
                _model.NameChanged -= OnNameChanged;
                _model.ProgressChanged -= OnProgressChanged;
                _model.IncomeChanged -= OnIncomeChanged;
                _model.LevelUpPriceChanged -= OnLevelUpPriceChanged;
                
                _levelUpButton.onClick.RemoveAllListeners();
            }
        }

        private void OnLevelChanged(int level) => _levelText.text = $"LVL: \n{level}";
        private void OnNameChanged(string name) => _nameText.text = name;
        private void OnProgressChanged(float progress) => _progressBar.value = progress;
        private void OnIncomeChanged(int income) => _incomeText.text = $"ДОХОД: \n{income}$";
        private void OnLevelUpPriceChanged(int price) => _levelUpPriceText.text = $"LVL UP: \n{price}$";

        private void InitUpgradeViews(BusinessScreenModel model)
        {
            for (int i = 0; i < _upgradeBusinessViews.Count; i++)
            {
                if (i < model.UpgradeBusinessScreenModels.Count)
                {
                    _upgradeBusinessViews[i].Initialize(model.UpgradeBusinessScreenModels[i]);
                }
            }
        }
    }
}