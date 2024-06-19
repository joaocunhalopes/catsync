namespace Serial
{
    public static class Control
    {
        public static void PortOpen(System.IO.Ports.SerialPort port)
        {
            Port.Open(port);
        }

        public static void WriteToPort(System.IO.Ports.SerialPort port, byte[] data, int timeout)
        {
            Port.Write(port, data, timeout);
        }

        public static byte[] WriteReadToPort(System.IO.Ports.SerialPort port, byte[] data, int timeout)
        {
            return Port.WriteRead(port, data, timeout);
        }

        public static void PortClose(System.IO.Ports.SerialPort port)
        {
                Port.Close(port);
        }
    }
}