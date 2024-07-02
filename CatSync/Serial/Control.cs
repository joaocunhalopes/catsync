namespace Serial
{
    public static class Control
    {
        public static void PortOpen(System.IO.Ports.SerialPort port)
        {
            Port.Open(port);
        }

        public static byte[] ReadFromPort(System.IO.Ports.SerialPort port)
        {
            return Port.Read(port);
        }

        public static void WriteToPort(System.IO.Ports.SerialPort port, byte[] data, int latency)
        {
            Port.Write(port, data, latency);
        }

        public static void PortClose(System.IO.Ports.SerialPort port)
        {
                Port.Close(port);
        }

        public static void PortDispose(System.IO.Ports.SerialPort port)
        {
            Port.Dispose(port);
        }
    }
}