using Serilog;
using System.Text;

namespace CAT
{
    internal static class KSI
    {
        internal static int ReadFrequency(Config.Xcvr xcvr)
        {
            byte[] byteCommand = HexStringToByteArray(xcvr.Frequency.ReadCommand);
            Serial.Control.WriteToPort(xcvr.SerialPort, byteCommand, xcvr.Latency);

            byte[] buffer = Serial.Control.ReadFromPort(xcvr.SerialPort);
            string bufferString = ByteArrayToHexString(buffer);
            // ---
            Log.Debug($"KSI ReadFrequency bufferString: {bufferString}");
            // ---
            return FilterBuffer(bufferString, xcvr.Frequency.ReadCommandPrefix, xcvr.Frequency.ReadCommandSufix);
        }

        internal static void SetFrequency(Config.Xcvr xcvr, int currentFrequency)
        {
            byte[] byteCommand = HexStringToByteArray(BuildCommand(xcvr.Frequency.SetCommandPrefix, currentFrequency, xcvr.Frequency.SetCommandSufix));
            Serial.Control.WriteToPort(xcvr.SerialPort, byteCommand, 0);
        }

        private static byte[] HexStringToByteArray(string command)
        {
            return Encoding.UTF8.GetBytes(command);
        }

        private static string ByteArrayToHexString(byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }

        private static int FilterBuffer(string bufferString, string prefix, string suffix)
        {
            int responseLength = bufferString.Length;
            int prefixLength = prefix.Length;
            int suffixLength = suffix.Length;

            string frequencyString = "0";
            int i = 0;
            while (i + prefixLength <= responseLength)
            {
                if (bufferString[i] == prefix[0] && bufferString.Substring(i, prefixLength) == prefix)
                {
                    i += prefixLength;

                    int start = i;
                    while (i + suffixLength <= responseLength && bufferString.Substring(i, suffixLength) != suffix)
                    {
                        i++;
                    }

                    if (i + suffixLength <= responseLength)
                    {
                        frequencyString = bufferString.Substring(start, i - start);
                        i += suffixLength;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    i++;
                }
            }
            // ---
            Log.Debug($"KSI FilterBuffer frequencyString: {frequencyString}");
            // ---
            return DecodeFrequency(frequencyString);
        }

        private static int DecodeFrequency(string frequencyString)
        {
            int frequency = 0;
            int length = frequencyString.Length;
            for (int j = 0; j < length; j++)
            {
                frequency = frequency * 10 + (frequencyString[j] - '0');
            }
            // ---
            Log.Debug($"KSI DecodeFrequency frequency: {frequency}");
            // ---
            return frequency;
        }

        private static string BuildCommand(string writePrefix, int currentFrequency, string writeSufix)
        {
            string commandString = writePrefix + EncodeFrequency(currentFrequency) + writeSufix;
            return commandString;
        }

        private static string EncodeFrequency(int frequency)
        {
            return frequency.ToString().PadLeft(11, '0');
        }
    }
}