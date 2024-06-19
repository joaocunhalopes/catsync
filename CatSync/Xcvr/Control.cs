using Serilog;

namespace Xcvr
{
    public static class Control
    {
        private static List<Config.Xcvr> _xcvrs = new();

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
                throw new ConfigException(ex.Message);
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
                throw new OpenPortException(ex.Message);
            }
        }

        public static void ReadFrequency(Config.Xcvr xcvr)
        {
            try
            {
                int frequency = CAT.Control.ReadFrequency(xcvr);
                xcvr.PreviousFrequency = xcvr.CurrentFrequency;
                xcvr.CurrentFrequency = frequency;
            }
            catch (Exception ex)
            {
                throw new ReadFrequencyException(ex.Message);
            }
        }

        public static void EqualizeFrequencies()
        {
            if (Xcvrs[0].CurrentFrequency != Xcvrs[1].CurrentFrequency)
            {
                if (Xcvrs[0].CurrentFrequency != Xcvrs[0].PreviousFrequency)
                {
                    CAT.Control.WriteFrequency(Xcvrs[1], Xcvrs[0].CurrentFrequency);
                    Xcvrs[1].CurrentFrequency = Xcvrs[0].CurrentFrequency;
                }
                else if (Xcvrs[1].CurrentFrequency != Xcvrs[1].PreviousFrequency)
                {
                    CAT.Control.WriteFrequency(Xcvrs[0], Xcvrs[1].CurrentFrequency);
                    Xcvrs[0].CurrentFrequency = Xcvrs[1].CurrentFrequency;
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
                throw new OpenPortException(ex.Message);
            }
        }
    }
}