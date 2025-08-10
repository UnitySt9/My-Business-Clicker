using System;
using System.Collections.Generic;

namespace Code.Gameplay.Business.Configs
{
    [Serializable]
    public class BusinessUpgradeNameData
    {
        public string Name;

        public List<string> UpgradeNames = new();
    }
}