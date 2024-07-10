namespace Config
{
    public static class Control
    {
        private const string XcvrsRelativeFilePath = "Xcvrs\\Xcvrs.json";
        private const string XcvrsListRelativeFilePath = "Xcvrs\\XcvrsList.json";

        public static List<Xcvr> ReadXcvrsConfig()
        {
            List<Xcvr> xcvrs = Parser.ParseXcvrs(XcvrsRelativeFilePath);
            Parser.LogXcvrs(xcvrs);
            return xcvrs;
        }

        public static List<Xcvr> ReadXcvrsListConfig()
        {
            List<Xcvr> xcvrs = Parser.ParseXcvrsList(XcvrsListRelativeFilePath);
            Parser.LogXcvrs(xcvrs);
            return xcvrs;
        }

        public static void WriteXcvrsConfig(List<Xcvr> xcvrs)
        {
            XcvrsConfig xcvrsConfig = new(xcvrs);
            Serializer.SerializeJson(xcvrsConfig, XcvrsRelativeFilePath);
        }
    }
}