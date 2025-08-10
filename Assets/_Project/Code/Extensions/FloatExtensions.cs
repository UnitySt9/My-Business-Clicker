namespace Code.Extensions
{
    public static class FloatExtensions
    {
        public static int ToPercentage(this float value) => (int)(value * 100);
    }
}