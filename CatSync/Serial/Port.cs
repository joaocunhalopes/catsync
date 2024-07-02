using Util;

namespace Serial
{
    internal static class Port
    {
        internal static void Open(System.IO.Ports.SerialPort port)
        {
            if (port.IsOpen)
            {
                Log.Warning($"Port {port.PortName} is already open.");
            }
            else
            {
                port.Open();
                Log.Information($"Opened port {port.PortName}.");
            }
        }

        internal static byte[] Read(System.IO.Ports.SerialPort port)
        {
            byte[] buffer = new byte[4096]; // Adjust buffer site if needed.
            int bytesRead = port.Read(buffer, 0, buffer.Length);
            byte[] responseBytes = new byte[bytesRead];
            Buffer.BlockCopy(buffer, 0, responseBytes, 0, bytesRead);
            return responseBytes;
        }

        internal static void Write(System.IO.Ports.SerialPort port, byte[] command, int latency)
        {
            port.Write(command, 0, command.Length);
            Thread.Sleep(latency);
        }

        internal static void Close(System.IO.Ports.SerialPort port)
        {
            if (port.IsOpen)
            {
                port.Close();
                Log.Information($"Closed port {port.PortName}.");
            }
            else
            {
                Log.Warning($"Port {port.PortName} is already closed.");
            }
        }

        internal static void Dispose(System.IO.Ports.SerialPort port)
        {
            if (port.IsOpen)
            {
                port.Dispose();
                Log.Information($"Disposed port {port.PortName}.");
            }
            else
            {
                Log.Warning($"Port {port.PortName} is already disposed.");
            }
        }
    }
}