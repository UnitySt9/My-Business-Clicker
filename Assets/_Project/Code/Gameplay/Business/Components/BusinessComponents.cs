using System;
using System.Collections.Generic;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Business.Requests;

namespace Code.Gameplay.Business.Components
{
    public struct BusinessComponent
    {
        public int Id;
        public string Name;
        public int Level;
        public int BaseCost;
        public int BaseIncome;
        public int CurrentIncome;
        public int TotalIncome;
        public float Progress;
    }
    
    public struct IncomeCooldownComponent
    {
        public float Duration;
        public float TimeLeft;
        public bool IsAvailable;
        public bool IsCompleted;
    }
    
    public struct BusinessModifiersComponent
    {
        public float TotalMultiplier;
        public List<AccumulatedModifiersData> AccumulatedModifiers;
        public List<UpgradeData> UpdateModifiers;
    }
    
    public struct UpgradeModifierComponent
    {
        public int Id;
        public float Multiplier;
        public float Value;
    }
    
    public struct LevelUpRequestComponent
    {
        public LevelUpRequest Value;
    }
    
    public struct UpgradePurchasedRequestComponent
    {
        public UpgradePurchasedRequest Value;
    }
    
    [Serializable]
    public struct BusinessSaveComponent
    {
        public int BusinessId;
        public int Level;
        public float Progress;
        public float Cooldown;
    }
    
    public struct BusinessIdComponent
    {
        public int Value;
    }
    
    public struct UpdateBusinessModifiersComponent
    {
        public List<UpgradeData> Value;
    }
    
    public struct UpgradeModifierIdComponent
    {
        public int Value;
    }
    
    public struct AccumulatedModifierComponent
    {
        public float Value;
        public List<AccumulatedModifiersData> Modifiers;
    }
}
