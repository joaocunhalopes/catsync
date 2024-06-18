namespace Serial
{
    public static class Control
    {
        public static void OpenPort(System.IO.Ports.SerialPort port)
        {
            Port.Open(port);
        }

        public static void WriteToPort(System.IO.Ports.SerialPort port, byte[] data, int timeout)
        {
            Port.Write(port, data, timeout);
        }

        public static byte[] WriteReadToPort(System.IO.Ports.SerialPort port, byte[] data, int timeout)
        {
            byte[] buffer = Array.Empty<byte>();
            return Port.WriteRead(port, data, timeout);
        }

        public static void ClosePort(System.IO.Ports.SerialPort port)
        {
                Port.Close(port);
        }
    }
}