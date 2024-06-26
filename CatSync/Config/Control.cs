namespace Config
{
    public static class Control
    {
        public static List<Xcvr> ReadConfig()
        {
            List<Xcvr> xcvrs = Parser.ParseJson();
            Parser.LogXcvrs(xcvrs);
            return xcvrs;
        }
    }
}