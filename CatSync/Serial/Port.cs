using Util;

namespace Serial
{
    internal static class Port
    {
        internal static void Open(System.IO.Ports.SerialPort port)
        {
            try
            {
                if (port.IsOpen)
                {
                    Log.Debug($"Port {port.PortName} is already open.");
                }
                else
                {
                    port.Open();
                    Log.Debug($"Opened port {port.PortName}.");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Access denied to port {port.PortName}.");
            }
            catch (IOException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"I/O error opening port {port.PortName}.");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Invalid operation on port {port.PortName}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Unexpected error opening port {port.PortName}.");
            }
        }

        internal static void Write(System.IO.Ports.SerialPort port, byte[] command, int timeout)
        {
            try
            {
                port.Write(command, 0, command.Length);
                Thread.Sleep(timeout);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Access denied to port {port.PortName}.");
            }
            catch (IOException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"I/O error sending data to port {port.PortName}.");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Invalid operation on port {port.PortName}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Unexpected error sending data to port {port.PortName}.");
            }
        }

        internal static byte[] WriteRead(System.IO.Ports.SerialPort port, byte[] command, int timeout)
        {
            byte[] buffer = Array.Empty<byte>();
            try
            {
                port.Write(command, 0, command.Length);
                Thread.Sleep(timeout);
                buffer = ReadBytes(port);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Access denied to port {port.PortName}.");
            }
            catch (IOException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"I/O error sending data to port {port.PortName}.");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Invalid operation on port {port.PortName}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Unexpected error sending data to port {port.PortName}.");
            }
            return buffer;
        }

        internal static void Close(System.IO.Ports.SerialPort port)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    Log.Debug($"Closed port {port.PortName}.");
                }
                else
                {
                    Log.Debug($"Port {port.PortName} is already closed.");
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"I/O error closing port {port.PortName}.");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Invalid operation on port {port.PortName}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning($"Unexpected error closing port {port.PortName}.");
            }
        }

        private static byte[] ReadBytes(System.IO.Ports.SerialPort serialPort)
        {
            byte[] buffer = new byte[2048]; // BUffer size = 2048
            int bytesRead = serialPort.Read(buffer, 0, buffer.Length);
            byte[] responseBytes = new byte[bytesRead];
            Array.Copy(buffer, responseBytes, bytesRead);
            return responseBytes;
        }
    }
}