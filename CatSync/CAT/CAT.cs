using System.Text;

namespace CAT
{
    internal static class CAT
    {
        internal static int ReadFrequency(Config.Xcvr xcvr)
        {
            byte[] byteCommand = Encoding.ASCII.GetBytes(xcvr.Commands.Read);
            byte[] buffer = Serial.Control.WriteReadToPort(xcvr.SerialPort, byteCommand, xcvr.Latency);
            string bufferString = Encoding.UTF8.GetString(buffer);
            string frequencyString = FilterBuffer(bufferString, xcvr.Commands.Read, xcvr.Commands.ReadPrefix, xcvr.Commands.ReadSufix);
            return int.Parse(frequencyString);
        }

        internal static void WriteFrequency(Config.Xcvr xcvr, int currentFrequency)
        {
            byte[] byteCommand = Encoding.ASCII.GetBytes(BuildCommand(xcvr.Commands.Write, currentFrequency));
            Serial.Control.WriteToPort(xcvr.SerialPort, byteCommand, xcvr.Latency);
        }

        private static string FilterBuffer(string bufferString, string command, string replyPrefix, string replySufix)
        {
            bufferString = bufferString.Replace(command, string.Empty);
            int startIndex = bufferString.IndexOf(replyPrefix, StringComparison.Ordinal);
            startIndex += replyPrefix.Length;
            int endIndex = bufferString.IndexOf(replySufix, startIndex, StringComparison.Ordinal);
            return bufferString.Substring(startIndex, endIndex - startIndex);
        }

        private static string BuildCommand(string updateFrequency, int currentFrequency)
        {
            string currentFrequencyString = currentFrequency.ToString().PadLeft(11, '0');
            string frequencyStrintPrefix = updateFrequency.Substring(0, 2);
            string frequencyStrintSufix = updateFrequency.Substring(13, 1);
            string commandString = frequencyStrintPrefix + currentFrequencyString + frequencyStrintSufix;
            return commandString;
        }
    }
}