namespace Xcvr
{
    internal static class FrequencyRange
    {
        private const int FrequencyLowerLimit = 0;
        private const int FrequencyHigherLimit = 999999999;

        public static int LowerLimit { get; } = FrequencyLowerLimit;
        public static int UpperLimit { get; } = FrequencyHigherLimit;

        public static bool Contains(int value)
        {
            return value > LowerLimit && value < UpperLimit;
        }
    }
}
