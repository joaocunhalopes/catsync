namespace Util
{
    public static class Formater
    {
        public static string FormatFrequencyWithDots(int frequency)
        {
            if (frequency == 0)
            {
                return ("000.000.000 MHz");
            }
            string formatedFrequency = frequency.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture).Replace(",", ".");
            int formatedFrequencyLenght = formatedFrequency.Length;
            string frequencyUnit = "MHz";
            if (formatedFrequencyLenght >= 5 && formatedFrequencyLenght <= 7)
            {
                frequencyUnit = "KHz";
            }
            else if (formatedFrequencyLenght >= 1 && formatedFrequencyLenght <= 3)
            {
                frequencyUnit = "Hz";
            }
            return ($"{formatedFrequency} {frequencyUnit}");
        }
    }
}