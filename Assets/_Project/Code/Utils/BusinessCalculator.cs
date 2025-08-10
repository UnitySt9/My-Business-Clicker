namespace Code.Utils
{
    public static class BusinessCalculator
    {
        /// <summary>
        /// Вычисляет общий доход бизнеса с учётом уровня и множителей от улучшений.
        /// </summary>
        /// <param name="level">Текущий уровень бизнеса</param>
        /// <param name="baseIncome">Базовый доход за уровень</param>
        /// <param name="upgradeModifier1">Множитель от первого улучшения</param>
        /// <param name="upgradeModifier2">Множитель от второго улучшения</param>
        public static float CalculateIncome(int level, float baseIncome, float upgradeModifier1, float upgradeModifier2)
        {
            return level * baseIncome * (1 + upgradeModifier1 + upgradeModifier2);
        }

        /// <summary>
        /// Вычисляет цену повышения уровня.
        /// </summary>
        /// <param name="level">Текущий уровень бизнеса</param>
        /// <param name="basePrice">Базовая стоимость повышения уровня</param>
        public static int CalculateLevelUpPrice(int level, int basePrice)
        {
            return (level + 1) * basePrice;
        }
    }
}