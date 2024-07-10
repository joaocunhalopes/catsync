using System.Text.Json;

namespace Config
{
    internal class Serializer
    {
        internal static void SerializeJson(XcvrsConfig config, string xcvrsRelativeFilePath)
        {
            if (config == null)
            {
                string message = $"Cannot serialize a null config object.";
                Util.Log.Error(message);
                throw new ArgumentNullException(nameof(config), message);
            }

            string baseDirectory = AppContext.BaseDirectory;
            string xcvrsFilePath = Path.Combine(baseDirectory, xcvrsRelativeFilePath);

            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(xcvrsFilePath, json);
        }
    }
}