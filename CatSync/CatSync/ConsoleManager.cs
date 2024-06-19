namespace CatSync
{
    internal static class ConsoleManager
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public static void ShowConsole()
        {
            AllocConsole();
        }

        public static void HideConsole()
        {
            FreeConsole();
        }
    }
}
