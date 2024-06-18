using System.Text.Json.Serialization;

namespace Config
{
    public class XcvrsList
    {
        public List<Xcvr> Xcvrs { get; set; } = new();
    }

    public class Xcvr
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Protocol { get; set; } = string.Empty;
        public int Timeout { get; set; }
        public Commands Commands { get; set; } = new(); 
        public PortSettings PortSettings { get; set; } = new();
        public int CurrentFrequency { get; set; } = 0;
        public int PreviousFrequency { get; set; } = 0;

        [JsonIgnore]
        public System.IO.Ports.SerialPort SerialPort { get; set; } = new();
    }

    public class Commands
    {
        public string Read { get; set; } = string.Empty;
        public string ReadPrefix { get; set; } = string.Empty;
        public string ReadSufix { get; set; } = string.Empty;

        public string Write { get; set; } = string.Empty;
        public string WritePrefix { get; set; } = string.Empty;
        public string WriteSufix { get; set; } = string.Empty;
    }

    public class PortSettings
    {
        public string PortName { get; set; } = string.Empty;
        public int BaudRate { get; set; }
        public string Parity { get; set; } = string.Empty;
        public int DataBits { get; set; }
        public string StopBits { get; set; } = string.Empty;
        public string Handshake { get; set; } = string.Empty;
    }
}