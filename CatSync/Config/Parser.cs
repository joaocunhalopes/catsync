using System.Text.Json;
using Util;

namespace Config
{
    internal static class Parser
    {
        private const string XcvrRelativeFilePath = "Xcvrs\\Xcvrs.json";

        internal static XcvrsList ParseJson()
        {
            XcvrsList xcvrsList = new XcvrsList();

            try
            {
                var baseDirectory = AppContext.BaseDirectory;
                var xcvrsFilePath = Path.Combine(baseDirectory, XcvrRelativeFilePath);
                var json = File.ReadAllText(xcvrsFilePath);
                var parsedXcvrsList = JsonSerializer.Deserialize<XcvrsList>(json);

                if (parsedXcvrsList == null)
                {
                    throw new InvalidOperationException("Deserialization of Xcvrs.json resulted in a null object.");
                }

                xcvrsList = parsedXcvrsList;
                return xcvrsList;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex.Message);
                Log.Warning("File Xcvrs.json not found.");
            }
            catch (JsonException ex)
            {
                Log.Error(ex.Message);
                Log.Warning("JSON deserialization error.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning("Could not parse Xcvrs.json.");
            }

            return xcvrsList;
        }

        internal static void LogXcvrList(XcvrsList xcvrsList)
        {
            try
            {
                foreach (var xcvr in xcvrsList.Xcvrs)
                {
                    var portSettings = xcvr.PortSettings;
                    var commands = xcvr.Commands;
                    Log.Information($"Transceiver {xcvr.Id}, Manufacturer: {xcvr.Manufacturer}, Model: {xcvr.Model}, Protocol: {xcvr.Protocol}, Timeout: {xcvr.Timeout}");
                    Log.Information($"Read Command: '{commands.Read}', Read Prefix: '{commands.ReadPrefix}', Read Sufix: '{commands.ReadSufix}'");
                    Log.Information($"Write Command: '{commands.Write}', Write Prefix: '{commands.WritePrefix}, Write Sufix: '{commands.WriteSufix}'");
                    Log.Information($"Port {portSettings.PortName}, Baudrate: {portSettings.BaudRate}, Parity: {portSettings.Parity}, DataBits: {portSettings.DataBits}, StopBits: {portSettings.StopBits}, Handshake: {portSettings.Handshake}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning("Could not log Xcvrs.json.");
            }
        }
    }
}