namespace Config
{
    public static class Control
    {
        public static List<Xcvr> ReadConfig()
        {
            return Parser.ParseJson();
        }
    }
}