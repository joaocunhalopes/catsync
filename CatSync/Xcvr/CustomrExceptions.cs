namespace Xcvr
{
    public class ConfigException : Exception
    {
        public ConfigException() : base() { }
        public ConfigException(string message) : base(message) { }
        public ConfigException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class OpenPortException : Exception
    {
        public OpenPortException() : base() { }
        public OpenPortException(string message) : base(message) { }
        public OpenPortException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ReadFrequencyException : Exception
    {
        public ReadFrequencyException() : base() { }
        public ReadFrequencyException(string message) : base(message) { }
        public ReadFrequencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
