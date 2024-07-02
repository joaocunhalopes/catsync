using Config;
using Serilog;

namespace Xcvr
{
    public static class Control
    {
        private const int FrequencyLowerLimit = 0;
        private const int FrequencyHigherLimit = 999999999;

        private volatile static List<Config.Xcvr> _xcvrs = new();

        public static List<Config.Xcvr> Xcvrs
        {
            get { return _xcvrs; }
            private set { _xcvrs = value; }
        }

        public static void Config()
        {
            try
            {
                Xcvrs = global::Config.Control.ReadConfig();
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                Log.Error($"{ex.StackTrace}");
                throw new ConfigException($"Error configuring application.");
            }
        }

        public static void OpenPort(Config.Xcvr xcvr)
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

        public static void SyncXcvrFrequencies(Config.Xcvr idXcvr0, Config.Xcvr idXcvr1)
        {
            Config.Xcvr xcvrMaster = idXcvr0;
            Config.Xcvr xcvrSlave = idXcvr1;

            if (idXcvr1.Switches.MasterOn)
            {
                xcvrMaster = idXcvr1;
                xcvrSlave = idXcvr0;
            }

            if (xcvrMaster.SerialPort.IsOpen)
            {
                try
                {
                    xcvrMaster.Frequency.Current = CAT.Control.ReadFrequency(xcvrMaster);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex.Message);
                    Log.Error($"{ex.StackTrace}");
                    ClosePort(xcvrMaster);
                    throw new ReadFrequencyException($"Could not read frequency for {xcvrMaster.Manufacturer} {xcvrMaster.Model}.");
                }
            }

            if (xcvrSlave.SerialPort.IsOpen && xcvrSlave.Switches.SyncOn)
            {
                int frequency = xcvrMaster.Frequency.Current;
                if (xcvrMaster.Switches.OffsetOn && xcvrMaster.Frequency.Offset != 0)
                {
                    frequency = Math.Clamp(frequency + xcvrMaster.Frequency.Offset, FrequencyLowerLimit, FrequencyHigherLimit);
                }

                try
                {
                    CAT.Control.SetFrequency(xcvrSlave, frequency);
                    xcvrSlave.Frequency.Current = frequency;
                }
                catch (Exception ex)
                {
                    Log.Warning(ex.Message);
                    Log.Error($"{ex.StackTrace}");
                    ClosePort(xcvrSlave);
                    throw new SetFrequencyException($"Could not set frequency for {xcvrSlave.Manufacturer} {xcvrSlave.Model}.");
                }
            }
        }

        public static void ClosePort(Config.Xcvr xcvr)
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

        public static void DisposePort(Config.Xcvr xcvr)
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