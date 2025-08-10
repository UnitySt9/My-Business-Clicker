using System.Collections.Generic;
using Code.Gameplay.Business.Components;
using Code.Gameplay.Business.Configs;

namespace Code.Gameplay.Business.Utils
{
    public static class BusinessModifierUtils
    {
        public static (float firstModifier, float secondModifier) GetModifiers(List<AccumulatedModifiersData> modifiers)
        {
            float firstModifier = 0;
            float secondModifier = 0;

            foreach (var modifier in modifiers)
            {
                switch (modifier.Id)
                {
                    case 0:
                        firstModifier = modifier.Value;
                        break;
                    case 1:
                        secondModifier = modifier.Value;
                        break;
                }
            }

            return (firstModifier, secondModifier);
        }
    }
} 