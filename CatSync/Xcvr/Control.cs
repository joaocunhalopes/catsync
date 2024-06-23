using Config;

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
            catch
            {
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
            catch
            {
                throw new OpenPortException($"Could not open port {xcvr.PortSettings.PortName}.");
            }
        }

        public static void ReadFrequency(Config.Xcvr xcvr)
        {
            try
            {
                int frequency = CAT.Control.ReadFrequency(xcvr);
                xcvr.Frequency.Current = frequency;
            }
            catch
            {
                throw new ReadFrequencyException($"Could not read frequency for {xcvr.Manufacturer} {xcvr.Model}.");
            }
        }

        public static void SyncFrequencies()
        {
            int id = 1;

            try
            {
                if (Xcvrs[0].Frequency.Lead == true && Xcvrs[1].Frequency.Release == false)
                {
                    if (Xcvrs[1].Frequency.Current != Xcvrs[0].Frequency.Current && Xcvrs[1].SerialPort.IsOpen)
                    {
                        id = 1;
                        CAT.Control.WriteFrequency(Xcvrs[1], Xcvrs[0].Frequency.Current);
                    }
                }
                else if (Xcvrs[1].Frequency.Lead == true && Xcvrs[0].Frequency.Release == false && Xcvrs[0].SerialPort.IsOpen)
                {
                    if (Xcvrs[0].Frequency.Current != Xcvrs[1].Frequency.Current)
                    {
                        id = 0;
                        CAT.Control.WriteFrequency(Xcvrs[0], Xcvrs[1].Frequency.Current);
                    }
                }
            }
            catch
            {
                throw new WriteFrequencyException($"Could not write frequency on {Xcvrs[id].Manufacturer} {Xcvrs[id].Model}.");
            }
        }

        public static void ClosePort(Config.Xcvr xcvr)
        {
            try
            {
                Serial.Control.PortClose(xcvr.SerialPort);
            }
            catch
            {
                throw new ClosePortException($"Could not close port {xcvr.PortSettings.PortName}.");
            }
        }

        public static void DisposePort(Config.Xcvr xcvr)
        {
            try
            {
                Serial.Control.PortDispose(xcvr.SerialPort);
            }
            catch
            {
                throw new DisposePortException($"Could not dispose port {xcvr.PortSettings.PortName}.");
            }
        }
    }
}