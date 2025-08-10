using System;

namespace Code.Gameplay.Business.Configs
{
    [Serializable]
    public class UpgradeData
    {
        public int Cost;
        public float IncomeMultiplier;
        public bool Purchased;
    }
}