using Serilog;

namespace CAT
{
    internal static class CIV
    {
        internal static int ReadFrequency(Config.Xcvr xcvr)
        {
            byte[] byteCommand = HexStringToByteArray(xcvr.Frequency.ReadCommand);
            Serial.Control.WriteToPort(xcvr.SerialPort, byteCommand, xcvr.Latency);

            byte[] buffer = Serial.Control.ReadFromPort(xcvr.SerialPort);
            string bufferString = ByteArrayToHexString(buffer);
            //Log.Debug($"CIV Buffer String: {bufferString}");
            return FilterBuffer(bufferString, xcvr.Frequency.ReadCommandPrefix, xcvr.Frequency.ReadCommandSufix);
        }

        internal static void SetFrequency(Config.Xcvr xcvr, int currentFrequency)
        {
            byte[] byteCommand = HexStringToByteArray(BuildCommand(xcvr.Frequency.SetCommandPrefix, currentFrequency, xcvr.Frequency.SetCommandSufix));
            Serial.Control.WriteToPort(xcvr.SerialPort, byteCommand, 0);
        }

        private static byte[] HexStringToByteArray(string command)
        {
            //Log.Debug($"CIV HexStringToByteArray Command: {command}");
            int numberOfChars = command.Length;
            byte[] byteCommand = new byte[numberOfChars / 2];
            for (int i = 0; i < numberOfChars; i += 2)
            {
                byteCommand[i / 2] = Convert.ToByte(command.Substring(i, 2), 16);
            }
            return byteCommand;
        }

        private static string ByteArrayToHexString(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", "");
        }

        private static int FilterBuffer(string bufferString, string readPrefix, string readSuffix)
        {
            int responseLength = bufferString.Length;
            int prefixLength = readPrefix.Length;
            int suffixLength = readSuffix.Length;

            string frequencyString = "0000000000";
            int i = 0;
            while (i + prefixLength <= responseLength)
            {
                if (bufferString[i] == readPrefix[0] && bufferString.Substring(i, prefixLength) == readPrefix)
                {
                    i += prefixLength;

                    int start = i;
                    while (i + suffixLength <= responseLength && bufferString.Substring(i, suffixLength) != readSuffix)
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
            return DecodeFrequency(frequencyString);
        }

        private static int DecodeFrequency(string frequencyString)
        {
            if (string.IsNullOrEmpty(frequencyString) || frequencyString.Length != 10)
            {
                return 0;
            }

            int frequency = 0;
            frequency += (frequencyString[8] - '0') * 1000000000; // 1 GHz
            frequency += (frequencyString[9] - '0') * 100000000;  // 100 MHz
            frequency += (frequencyString[6] - '0') * 10000000;   // 10 MHz
            frequency += (frequencyString[7] - '0') * 1000000;    // 1 MHz
            frequency += (frequencyString[4] - '0') * 100000;     // 100 kHz
            frequency += (frequencyString[5] - '0') * 10000;      // 10 kHz
            frequency += (frequencyString[2] - '0') * 1000;       // 1 kHz
            frequency += (frequencyString[3] - '0') * 100;        // 100 Hz
            frequency += (frequencyString[0] - '0') * 10;         // 10 Hz
            frequency += (frequencyString[1] - '0');              // 1 Hz
            return frequency;
        }

        private static int NewDecodeFrequency(string frequencyString)
        {
            if (string.IsNullOrEmpty(frequencyString) || frequencyString.Length != 10 || !frequencyString.All(char.IsDigit))
            {
                return 0;
            }

            int[] multipliers = { 10, 1, 1000, 100, 100000, 10000, 10000000, 1000000, 1000000000, 100000000 };
            int frequency = 0;
            for (int i = 0; i < frequencyString.Length; i++)
            {
                frequency += (frequencyString[i] - '0') * multipliers[i];
            }
            return frequency;
        }

        private static string BuildCommand(string writeSufix, int currentFrequency, string writePrefix)
        {
            string commandString = writeSufix + EncodeFrequency(currentFrequency) + writePrefix;
            //Log.Debug($"CIV BuildCommmand CommandString: {commandString}");
            return commandString;
        }

        private static string EncodeFrequency(int frequency)
        {
            char[] frequencyArray = new char[10];
            frequencyArray[8] = (char)((frequency / 1000000000) % 10 + '0'); // 1 GHz
            frequencyArray[9] = (char)((frequency / 100000000) % 10 + '0');  // 100 MHz
            frequencyArray[6] = (char)((frequency / 10000000) % 10 + '0');   // 10 MHz
            frequencyArray[7] = (char)((frequency / 1000000) % 10 + '0');    // 1 MHz
            frequencyArray[4] = (char)((frequency / 100000) % 10 + '0');     // 100 kHz
            frequencyArray[5] = (char)((frequency / 10000) % 10 + '0');      // 10 kHz
            frequencyArray[2] = (char)((frequency / 1000) % 10 + '0');       // 1 kHz
            frequencyArray[3] = (char)((frequency / 100) % 10 + '0');        // 100 Hz
            frequencyArray[0] = (char)((frequency / 10) % 10 + '0');         // 10 Hz
            frequencyArray[1] = (char)(frequency % 10 + '0');                // 1 Hz
            return new string(frequencyArray);
        }

        private static string NewEncodeFrequency(int frequency)
        {
            char[] frequencyArray = new char[10];
            int[] divisors = { 10, 1, 1000, 100, 100000, 10000, 10000000, 1000000, 1000000000, 100000000 };

            for (int i = 0; i < frequencyArray.Length; i++)
            {
                frequencyArray[i] = (char)((frequency / divisors[i]) % 10 + '0');
            }
            return new string(frequencyArray);
        }
    }
}