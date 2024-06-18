namespace Config
{
    public static class Control
    {
        public static XcvrsList ReadXcvrsConfig()
        {
            return Parser.ParseJson();
            //Parser.LogXcvrList();
        }
    }
}