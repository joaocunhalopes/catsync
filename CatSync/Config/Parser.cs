using System.Text.Json;

namespace Config
{
    internal static class Parser
    {
        private const string XcvrRelativeFilePath = "Xcvrs\\Xcvrs.json";

        internal static List<Xcvr> ParseJson()
        {
            try
            {
                string baseDirectory = AppContext.BaseDirectory;
                string xcvrsFilePath = Path.Combine(baseDirectory, XcvrRelativeFilePath);
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
            catch (FileNotFoundException ex)
            {
                Util.Log.Warning("File Xcvrs.json not found.");
                Util.Log.Error(ex.Message);
                throw;
            }
            catch (JsonException ex)
            { 
                Util.Log.Warning("JSON deserialization error.");
                Util.Log.Error(ex.Message);
                throw;  
            }
            catch (Exception ex)
            {
                Util.Log.Warning("Could not parse Xcvrs.json.");
                Util.Log.Error(ex.Message);
                throw;
            }
        }

        internal static void LogXcvrs(List<Xcvr> xcvrs)
        {
            try
            {
                foreach (Xcvr xcvr in xcvrs)
                {
                    PortSettings portSettings = xcvr.PortSettings;
                    Commands commands = xcvr.Commands;
                    Frequency frequency = xcvr.Frequency;
                    Util.Log.Information($"Transceiver: {xcvr.Id}, Manufacturer: {xcvr.Manufacturer}, Model: {xcvr.Model}, Protocol: {xcvr.Protocol}, Timeout: {xcvr.Timeout}");
                    Util.Log.Information($"Read Command: '{commands.Read}', Read Prefix: '{commands.ReadPrefix}', Read Sufix: '{commands.ReadSufix}'");
                    Util.Log.Information($"Write Command: '{commands.Write}', Write Prefix: '{commands.WritePrefix}, Write Sufix: '{commands.WriteSufix}'");
                    Util.Log.Information($"Port: {portSettings.PortName}, Baudrate: {portSettings.BaudRate}, Parity: {portSettings.Parity}, DataBits: {portSettings.DataBits}, StopBits: {portSettings.StopBits}, Handshake: {portSettings.Handshake}");
                    Util.Log.Information($"Offset: {frequency.Offset}");
                }
            }
            catch (Exception ex)
            {
                Util.Log.Warning("Could not log Xcvrs.json.");
                Util.Log.Error(ex.Message);
                throw;
            }
        }
    }
}