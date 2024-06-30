using Util;

namespace CAT
{
    public static class Control
    {
        public static int ReadFrequency(Config.Xcvr xcvr)
        {
            switch (xcvr.Protocol)
            {
                case "KSI":
                    return (KSI.ReadFrequency(xcvr));
                case "CIV":
                    return (CIV.ReadFrequency(xcvr));
                case "CAT":
                    return (CAT.ReadFrequency(xcvr));
                default:
                    Log.Warning("Unknown protocol in CAT Read Frequency.");
                    return 0;
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
                case "CAT":
                    CAT.WriteFrequency(xcvr, currentFrequency);
                    break;
                default:
                    Log.Warning("Unknown protocol in CAT Write Frequency.");
                    break;
            }
        }
    }
}