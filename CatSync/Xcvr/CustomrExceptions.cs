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

    public class SetFrequencyException : Exception
    {
        public SetFrequencyException() : base() { }
        public SetFrequencyException(string message) : base(message) { }
        public SetFrequencyException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ClosePortException : Exception
    {
        public ClosePortException() : base() { }
        public ClosePortException(string message) : base(message) { }
        public ClosePortException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class DisposePortException : Exception
    {
        public DisposePortException() : base() { }
        public DisposePortException(string message) : base(message) { }
        public DisposePortException(string message, Exception innerException) : base(message, innerException) { }
    }
}
