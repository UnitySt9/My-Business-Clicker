using System;
using System.Collections.Generic;

namespace Code.Gameplay.Save.Models
{
    [Serializable]
    public class BusinessSaveModel
    {
        public int Id;
        public int Level;
        public float Progress;
        public float Cooldown;
        public int Income;
        public int LevelUpPrice;
        public bool IsPurchased;
        public bool IsCooldownAvailable;
        public List<UpgradeSaveModel> Upgrades = new List<UpgradeSaveModel>();
    }

    [Serializable]
    public class UpgradeSaveModel
    {
        public int Id;
        public float IncomeModifier;
        public bool Purchased;
    }
} 