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
                    Log.Warning($"Port {port.PortName} is already open.");
                }
                else
                {
                    port.Open();
                    Log.Information($"Opened port {port.PortName}.");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Warning($"Access denied to port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (IOException ex)
            {
                Log.Warning($"I/O error opening port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning($"Invalid operation on port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Warning($"Unexpected error opening port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
        }

        internal static void Write(System.IO.Ports.SerialPort port, byte[] command, int latency)
        {
            try
            {
                port.Write(command, 0, command.Length);
                Thread.Sleep(latency);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Warning($"Access denied to port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (IOException ex)
            {
                Log.Warning($"I/O error sending data to port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning($"Invalid operation on port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Warning($"Unexpected error sending data to port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
        }

        internal static byte[] WriteRead(System.IO.Ports.SerialPort port, byte[] command, int latency)
        {
            try
            {
                port.Write(command, 0, command.Length);
                Thread.Sleep(latency);
                return ReadBytes(port);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Warning($"Access denied to port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (IOException ex)
            {
                Log.Warning($"I/O error sending data to port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning($"Invalid operation on port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Warning($"Unexpected error sending/reading data to/from port {port.PortName}.");
                Log.Error(ex.Message);
                throw;
            }
        }

        internal static void Close(System.IO.Ports.SerialPort port)
        {
            try
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

        internal static void Dispose(System.IO.Ports.SerialPort port)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    port.Dispose();
                    Log.Information($"Closed port {port.PortName}.");
                }
                else
                {
                    Log.Warning($"Port {port.PortName} is already closed.");
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
                Log.Warning($"Unexpected error disposing port {port.PortName}.");
            }
        }

        private static byte[] ReadBytes(System.IO.Ports.SerialPort serialPort)
        {
            try
            {
                byte[] buffer = new byte[4096]; // BUffer size 4Kb.
                int bytesRead = serialPort.Read(buffer, 0, buffer.Length);
                byte[] responseBytes = new byte[bytesRead];
                Array.Copy(buffer, responseBytes, bytesRead);
                return responseBytes;
            }
            catch (Exception ex)
            {
                Log.Warning("Error reading data from port.");
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}