using System.Text.Json.Serialization;

namespace Config
{
    public class XcvrsConfig
    {
        public List<Xcvr> Xcvrs { get; set; } = new();
    }

    public class Xcvr   
    {
        public int Id { get; set; }

        public string Manufacturer { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Protocol { get; set; } = string.Empty;

        public int Latency { get; set; } = 200;

        public PortSettings PortSettings { get; set; } = new();

        public Frequency Frequency { get; set; } = new();

        [JsonIgnore]
        public Switches Switches { get; set; } = new();

        [JsonIgnore]
        public System.IO.Ports.SerialPort SerialPort { get; set; } = new();
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

    public class Frequency
    {
        [JsonIgnore]
        public int Current { get; set; } = 0;

        [JsonIgnore]
        public int Previous { get; set; } = 0;

        public int Offset { get; set; } = 0;

        public string ReadCommand { get; set; } = string.Empty;

        public string ReadCommandPrefix { get; set; } = string.Empty;

        public string ReadCommandSufix { get; set; } = string.Empty;

        public string SetCommandPrefix { get; set; } = string.Empty;

        public string SetCommandSufix { get; set; } = string.Empty;
    }

    public class Switches
    {
        public bool MasterOn { get; set; } = true;

        public bool SyncOn { get; set; } = true;

        public bool OffsetOn { get; set; } = false;
    }
}