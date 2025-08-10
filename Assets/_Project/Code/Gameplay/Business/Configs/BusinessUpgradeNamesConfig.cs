using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Business.Configs
{
    [CreateAssetMenu(menuName = "Configs/BusinessUpgradeNamesConfig")]
    public class BusinessUpgradeNamesConfig : ScriptableObject
    {
        [SerializeField] private List<BusinessUpgradeNameData> _businessUpgradeNameDatas = new List<BusinessUpgradeNameData>();

        public IReadOnlyList<BusinessUpgradeNameData> BusinessUpgradeNameDatas => _businessUpgradeNameDatas;
    }
}