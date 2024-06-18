namespace Xcvr
{
    public static class Control
    {
        internal static Config.XcvrsList XcvrsList = new();

        public static void ReadXcvrsConfig()
        {
            XcvrsList = Config.Control.ReadXcvrsConfig();
        }

        public static Config.XcvrsList GetXcvrsList()
        {
            return XcvrsList;
        }

        public static void OpenXcvrsPorts()
        {
            foreach (var xcvr in XcvrsList.Xcvrs)
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
                Serial.Control.OpenPort(xcvr.SerialPort);
            }
        }

        public static void OpenXcvrPort(Config.Xcvr xcvr)
        {
            Serial.Control.OpenPort(xcvr.SerialPort);
        }

        public static void ReadXcvrFrequency(Config.Xcvr xcvr)
        {
            int frequency = CAT.Control.ReadFrequency(xcvr);
            xcvr.PreviousFrequency = xcvr.CurrentFrequency;
            xcvr.CurrentFrequency = frequency;
        }

        public static void EqualizeFrequencies()
        {
            if (XcvrsList.Xcvrs[0].CurrentFrequency != XcvrsList.Xcvrs[1].CurrentFrequency)
            {
                if (XcvrsList.Xcvrs[0].CurrentFrequency != XcvrsList.Xcvrs[0].PreviousFrequency)
                {
                    CAT.Control.WriteFrequency(XcvrsList.Xcvrs[1], XcvrsList.Xcvrs[0].CurrentFrequency);
                    XcvrsList.Xcvrs[1].CurrentFrequency = XcvrsList.Xcvrs[0].CurrentFrequency;
                }
                else if (XcvrsList.Xcvrs[1].CurrentFrequency != XcvrsList.Xcvrs[1].PreviousFrequency)
                {
                    CAT.Control.WriteFrequency(XcvrsList.Xcvrs[0], XcvrsList.Xcvrs[1].CurrentFrequency);
                    XcvrsList.Xcvrs[0].CurrentFrequency = XcvrsList.Xcvrs[1].CurrentFrequency;
                }
            }
        }

        public static void CloseXcvrsPorts()
        {
            foreach (var xcvr in XcvrsList.Xcvrs)
            {
                Serial.Control.ClosePort(xcvr.SerialPort);
            }
        }

        public static void CloseXcvrPort(Config.Xcvr xcvr)
        {
            Serial.Control.ClosePort(xcvr.SerialPort);
        }
    }
}