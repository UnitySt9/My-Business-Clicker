using System;
using System.Collections.Generic;

namespace Code.Gameplay.Save.Models
{
    [Serializable]
    public class GameSaveModel
    {
        public HeroSaveModel Hero = new();
        public List<BusinessSaveModel> Businesses = new List<BusinessSaveModel>();
    }
} 