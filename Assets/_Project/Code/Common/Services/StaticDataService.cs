using Code.Constants;
using Code.Gameplay.Business.Configs;
using Code.UI;
using UnityEngine;

namespace Code.Common.Services
{
    public class StaticDataService : IStaticDataService
    {
        private BusinessConfig _businessConfig;
        private BusinessUpgradeNamesConfig _businessUpgradeNamesConfig;
        private BusinessView _businessView;

        public BusinessConfig GetBusinessConfig() => _businessConfig;

        public BusinessUpgradeNamesConfig GetBusinessUpgradeNamesConfig() => _businessUpgradeNamesConfig;

        public BusinessView GetBusinessView => _businessView;

        public void LoadAll()
        {
            _businessConfig = Resources.Load<BusinessConfig>(AssetPath.BusinessConfig);

            _businessUpgradeNamesConfig = Resources.Load<BusinessUpgradeNamesConfig>(AssetPath.BusinessUpgradeNamesConfig);

            _businessView = Resources.Load<BusinessView>(AssetPath.BusinessView);
        }
    }
}