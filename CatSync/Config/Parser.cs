using System.Text.Json;

namespace Config
{
    internal static class Parser
    {
        internal static List<Xcvr> ParseXcvrs(string xcvrsRelativeFilePath)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string xcvrsFilePath = Path.Combine(baseDirectory, xcvrsRelativeFilePath);
            string json = File.ReadAllText(xcvrsFilePath);
            XcvrsConfig? config = JsonSerializer.Deserialize<XcvrsConfig>(json);

            if (config == null)
            {
                string message = $"Deserialization of {xcvrsFilePath} resulted in a null object.";
                Util.Log.Error(message);
                throw new ArgumentNullException(nameof(config), message);
            }

            return config.Xcvrs;
        }

        internal static void LogXcvrs(List<Xcvr> xcvrs)
        {
            foreach (Xcvr xcvr in xcvrs)
            {
                PortSettings portSettings = xcvr.PortSettings;
                Frequency frequency = xcvr.Frequency;
                Util.Log.Information($"Transceiver: {xcvr.Id}, Manufacturer: {xcvr.Manufacturer}, Model: {xcvr.Model}, Protocol: {xcvr.Protocol}, Latency: {xcvr.Latency}");
                Util.Log.Information($"Port: {portSettings.PortName}, Baudrate: {portSettings.BaudRate}, Parity: {portSettings.Parity}, DataBits: {portSettings.DataBits}, StopBits: {portSettings.StopBits}, Handshake: {portSettings.Handshake}");
                Util.Log.Information($"Offset: {frequency.Offset}");
                Util.Log.Information($"Read Command: '{frequency.ReadCommand}', Read Command Prefix: '{frequency.ReadCommandPrefix}', Read Command Sufix: '{frequency.ReadCommandSufix}'");
                Util.Log.Information($"Set Command Prefix: '{frequency.SetCommandPrefix}', Set Command Sufix: '{frequency.SetCommandSufix}'");
            }
        }

        internal static List<Xcvr> ParseXcvrsList(string xcvrsListRelativeFilePath)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string xcvrsListFilePath = Path.Combine(baseDirectory, xcvrsListRelativeFilePath);
            string json = File.ReadAllText(xcvrsListFilePath);
            XcvrsConfig? config = JsonSerializer.Deserialize<XcvrsConfig>(json);

            if (config == null)
            {
                string message = $"Deserialization of {xcvrsListFilePath} resulted in a null object.";
                Util.Log.Error(message);
                throw new ArgumentNullException(nameof(config), message);
            }

            return config.Xcvrs;
        }
    }
}