using Util;

namespace CAT
{
    public static class Control
    {
        public static int ReadFrequency(Config.Xcvr xcvr)
        {
            int frequency = 0;
            switch (xcvr.Protocol)
            {
                case "KSI":
                    return (KSI.ReadFrequency(xcvr));
                case "CIV":
                    return (CIV.ReadFrequency(xcvr));
                default:
                    Log.Warning("Unknown protocol in CAT Read Frequency.");
                    return frequency;
            }
        }

        public static void WriteFrequency(Config.Xcvr xcvr, int currentFrequency)
        {
            switch (xcvr.Protocol)
            {
                case "KSI":
                    KSI.WriteFrequency(xcvr, currentFrequency);
                    break;
                case "CIV":
                    CIV.WriteFrequency(xcvr, currentFrequency);
                    break;
                default:
                    Log.Warning("Unknown protocol in CAT Read Frequency.");
                    break;
            }
        }
    }
}