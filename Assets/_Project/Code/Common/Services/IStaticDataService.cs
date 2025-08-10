using Code.Gameplay.Business.Configs;
using Code.UI;

namespace Code.Common.Services
{
    public interface IStaticDataService
    {
        BusinessConfig GetBusinessConfig();
        void LoadAll();
        BusinessUpgradeNamesConfig GetBusinessUpgradeNamesConfig();
        BusinessView GetBusinessView { get; }
    }
} 