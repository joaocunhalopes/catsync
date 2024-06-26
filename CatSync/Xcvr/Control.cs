﻿using Config;
using Serilog;

namespace Xcvr
{
    public static class Control
    {
        private const int FrequencyLowerLimit = 0;
        private const int FrequencyHigherLimit = 999999999;

        private static List<Config.Xcvr> _xcvrs = new();

        public static List<Config.Xcvr> Xcvrs
        {
            get { return _xcvrs; }
            private set { _xcvrs = value; }
        }

        public static void Config()
        {
            try
            {
                Xcvrs = global::Config.Control.ReadConfig();
            }
            catch
            {
                throw new ConfigException($"Error configuring application.");
            }
        }

        public static void OpenPort(Config.Xcvr xcvr)
        {
            try
            {
                System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort
                {
                    PortName = xcvr.PortSettings.PortName,
                    BaudRate = xcvr.PortSettings.BaudRate,
                    Parity = Enum.Parse<System.IO.Ports.Parity>(xcvr.PortSettings.Parity, true),
                    DataBits = xcvr.PortSettings.DataBits,
                    StopBits = Enum.Parse<System.IO.Ports.StopBits>(xcvr.PortSettings.StopBits, true),
                    Handshake = Enum.Parse<System.IO.Ports.Handshake>(xcvr.PortSettings.Handshake, true)
                };
                xcvr.SerialPort = port;

                Serial.Control.PortOpen(xcvr.SerialPort);
            }
            catch
            {
                throw new OpenPortException($"Could not open port {xcvr.PortSettings.PortName}.");
            }
        }

        public static void ReadFrequency(Config.Xcvr xcvr)
        {
            try
            {
                int frequency = CAT.Control.ReadFrequency(xcvr);
                xcvr.Frequency.Current = frequency;
            }
            catch
            {
                throw new ReadFrequencyException($"Could not read frequency for {xcvr.Manufacturer} {xcvr.Model}.");
            }
        }

        public static void SyncFrequencies()
        {
            int frequency;

            if (Xcvrs[0].Frequency.Master && Xcvrs[1].SerialPort.IsOpen && Xcvrs[1].Frequency.SyncOn)
            {
                frequency = Xcvrs[0].Frequency.Current;

                if (Xcvrs[0].Frequency.OffsetOn && Xcvrs[0].Frequency.Offset != 0)
                {
                    frequency = Math.Clamp(frequency + Xcvrs[0].Frequency.Offset, FrequencyLowerLimit, FrequencyHigherLimit);
                }

                if (Xcvrs[1].Frequency.Current != frequency)
                {
                    try
                    {
                        CAT.Control.WriteFrequency(Xcvrs[1], frequency);
                        Xcvrs[1].Frequency.Current = frequency;
                    }
                    catch (Exception ex)
                    {
                        throw new WriteFrequencyException($"Could not write frequency on {Xcvrs[1].Manufacturer} {Xcvrs[1].Model}.");
                    }
                }
            }
            else
            if (Xcvrs[1].Frequency.Master && Xcvrs[0].SerialPort.IsOpen && Xcvrs[0].Frequency.SyncOn)
            {
                frequency = Xcvrs[1].Frequency.Current;

                if (Xcvrs[1].Frequency.OffsetOn && Xcvrs[1].Frequency.Offset != 0)
                {
                    frequency = Math.Clamp(frequency + Xcvrs[1].Frequency.Offset, FrequencyLowerLimit, FrequencyHigherLimit);
                }

                if (Xcvrs[0].Frequency.Current != frequency)
                {
                    try
                    {
                        CAT.Control.WriteFrequency(Xcvrs[0], frequency);
                        Xcvrs[0].Frequency.Current = frequency;
                    }
                    catch
                    {
                        throw new WriteFrequencyException($"Could not write frequency on {Xcvrs[0].Manufacturer} {Xcvrs[0].Model}.");
                    }
                }
            }
        }

        public static void ClosePort(Config.Xcvr xcvr)
        {
            try
            {
                Serial.Control.PortClose(xcvr.SerialPort);
            }
            catch
            {
                throw new ClosePortException($"Could not close port {xcvr.PortSettings.PortName}.");
            }
        }

        public static void DisposePort(Config.Xcvr xcvr)
        {
            try
            {
                Serial.Control.PortDispose(xcvr.SerialPort);
            }
            catch
            {
                throw new DisposePortException($"Could not dispose port {xcvr.PortSettings.PortName}.");
            }
        }
    }
}