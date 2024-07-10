using Serilog;

namespace Xcvr
{
    public static class Control
    {
        private volatile static List<Config.Xcvr> _xcvrs = new();
        private volatile static List<Config.Xcvr> _xcvrsList = new();

        public static List<Config.Xcvr> Xcvrs
        {
            get { return _xcvrs; }
            private set { _xcvrs = value; }
        }

        public static List<Config.Xcvr> XcvrsList
        {
            get { return _xcvrsList; }
            private set { _xcvrsList = value; }
        }

        public static void ReadXcvrsConfig()
        {
            try
            {
                Xcvrs = Config.Control.ReadXcvrsConfig();
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                Log.Error($"{ex.StackTrace}");
                throw new ConfigException($"Error configuring application.");
            }
        }

        public static void ReadXcvrsListConfig()
        {
            try
            {
                XcvrsList = Config.Control.ReadXcvrsListConfig();
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                Log.Error($"{ex.StackTrace}");
                throw new ConfigException($"Error configuring application.");
            }
        }

        public static void OpenXcvrPort(Config.Xcvr xcvr)
        {
            try
            {
                System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort
                {
                    PortName = xcvr.PortSettings.PortName,
                    BaudRate = xcvr.PortSettings.BaudRate,
                    Parity = Enum.Parse<System.IO.Ports.Parity>(xcvr.PortSettings.Parity, true),
                    DataBits = xcvr.PortSettings.DataBits,
                    StopBits = Enum.Parse<System.IO.Ports.StopBits>(xcvr.PortSettings.StopBits, true),
                    Handshake = Enum.Parse<System.IO.Ports.Handshake>(xcvr.PortSettings.Handshake, true)
                };
                xcvr.SerialPort = port;

                Serial.Control.PortOpen(xcvr.SerialPort);
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                Log.Error($"{ex.StackTrace}");
                throw new OpenPortException($"Could not open port {xcvr.PortSettings.PortName}.");
            }
        }

        public static void SyncXcvrsFrequency()
        {
            // Select Master and Slave based on the MasterOn switch position.
            Config.Xcvr xcvrMaster = Xcvrs[0].Switches.MasterOn ? Xcvrs[0] : Xcvrs[1];
            Config.Xcvr xcvrSlave = Xcvrs[0].Switches.MasterOn ? Xcvrs[1] : Xcvrs[0];

            // Master will read it's frequency, unless port is closed.
            if (xcvrMaster.SerialPort.IsOpen)
            {
                try
                {
                    int masterFrequency = CAT.Control.ReadFrequency(xcvrMaster);
                    if (FrequencyRange.Contains(masterFrequency)) // This assures only in range frequencies are updated.
                    {
                        xcvrMaster.Frequency.Current = masterFrequency;
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex.Message);
                    Log.Error($"{ex.StackTrace}");
                    CloseXcvrPort(xcvrMaster);
                    throw new ReadFrequencyException($"Could not read frequency for {xcvrMaster.Manufacturer} {xcvrMaster.Model}.");
                }
            }

            // Slave will follow Master, as long as the SyncOn switch is ON and the port is open.
            if (xcvrSlave.SerialPort.IsOpen && xcvrSlave.Switches.SyncOn)
            {
                int masterFrequency = xcvrMaster.Frequency.Current;
                if (xcvrMaster.Switches.OffsetOn && xcvrMaster.Frequency.Offset != 0) // Include offset in frequency calculation.
                {
                    masterFrequency = Math.Clamp(masterFrequency + xcvrMaster.Frequency.Offset, FrequencyRange.LowerLimit, FrequencyRange.UpperLimit);
                }

                if (FrequencyRange.Contains(masterFrequency)) // This assures only in range frequencies are updated.
                {
                    try
                    {
                        CAT.Control.SetFrequency(xcvrSlave, masterFrequency);
                        xcvrSlave.Frequency.Current = masterFrequency;
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex.Message);
                        Log.Error($"{ex.StackTrace}");
                        CloseXcvrPort(xcvrSlave);
                        throw new SetFrequencyException($"Could not set frequency for {xcvrSlave.Manufacturer} {xcvrSlave.Model}.");
                    }
                }
            }
            // Slave will read it's frequency, as long as the SyncOn switch is OFF and the port is open.
            else if (xcvrSlave.SerialPort.IsOpen && !xcvrSlave.Switches.SyncOn)
            {
                try
                {
                    int slaveFrequency = CAT.Control.ReadFrequency(xcvrSlave);
                    if (FrequencyRange.Contains(slaveFrequency)) // This assures only in range frequencies are updated.
                    {
                        xcvrSlave.Frequency.Current = slaveFrequency;
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex.Message);
                    Log.Error($"{ex.StackTrace}");
                    CloseXcvrPort(xcvrSlave);
                    throw new ReadFrequencyException($"Could not read frequency for {xcvrSlave.Manufacturer} {xcvrSlave.Model}.");
                }
            }
        }

        public static void CloseXcvrPort(Config.Xcvr xcvr)
        {
            try
            {
                Serial.Control.PortClose(xcvr.SerialPort);
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                Log.Error($"{ex.StackTrace}");
                throw new ClosePortException($"Could not close port {xcvr.PortSettings.PortName}.");
            }
        }

        public static void DisposeXcvrPort(Config.Xcvr xcvr)
        {
            try
            {
                Serial.Control.PortDispose(xcvr.SerialPort);
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                Log.Error($"{ex.StackTrace}");
                throw new DisposePortException($"Could not dispose port {xcvr.PortSettings.PortName}.");
            }
        }
    }
}