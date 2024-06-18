using Config;
using System.IO.Ports;

namespace CatSync
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Xcvr.Control.ReadXcvrsConfig();
            Xcvr.Control.OpenXcvrsPorts();

            StartBackgroundTask();
        }

        private async void StartBackgroundTask()
        {
            await Task.Run(() =>
            {
                Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();

                while (true) // Example: Continuous operation until program exit
                {
                    try
                    {
                        Xcvr.Control.ReadXcvrFrequency(xcvrsList.Xcvrs[0]);
                        Xcvr.Control.ReadXcvrFrequency(xcvrsList.Xcvrs[1]);
                        Xcvr.Control.EqualizeFrequencies();
                        this.label65.Text = FormatFrequencyWithDots(xcvrsList.Xcvrs[0].CurrentFrequency);
                        this.label66.Text = FormatFrequencyWithDots(xcvrsList.Xcvrs[1].CurrentFrequency);
                    }
                    catch (Exception ex)
                    {
                        // Handle and log exceptions
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FillXcvr1Labels();
            FillXcvr2Labels();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            SetXcvr1PortButtons();
            SetXcvr2PortButtons();
        }

        private void FillXcvr1Labels()
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            this.label17.Text = xcvrsList.Xcvrs[0].Manufacturer;
            this.label18.Text = xcvrsList.Xcvrs[0].Model;
            this.label19.Text = xcvrsList.Xcvrs[0].Protocol;
            this.label20.Text = xcvrsList.Xcvrs[0].Timeout + " ms";

            this.label21.Text = xcvrsList.Xcvrs[0].PortSettings.PortName;
            this.label22.Text = xcvrsList.Xcvrs[0].PortSettings.BaudRate.ToString() + " bps";
            this.label23.Text = xcvrsList.Xcvrs[0].PortSettings.Parity;
            this.label24.Text = xcvrsList.Xcvrs[0].PortSettings.DataBits.ToString();
            this.label25.Text = xcvrsList.Xcvrs[0].PortSettings.StopBits;
            this.label26.Text = xcvrsList.Xcvrs[0].PortSettings.Handshake;

            this.label27.Text = xcvrsList.Xcvrs[0].Commands.Read;
            this.label28.Text = xcvrsList.Xcvrs[0].Commands.ReadPrefix;
            this.label29.Text = xcvrsList.Xcvrs[0].Commands.ReadSufix;
            this.label30.Text = xcvrsList.Xcvrs[0].Commands.Write;
            this.label31.Text = xcvrsList.Xcvrs[0].Commands.WritePrefix;
            this.label32.Text = xcvrsList.Xcvrs[0].Commands.WriteSufix;

            this.label65.Text = xcvrsList.Xcvrs[0].CurrentFrequency.ToString();
        }

        private void FillXcvr2Labels()
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            this.label60.Text = xcvrsList.Xcvrs[1].Manufacturer;
            this.label59.Text = xcvrsList.Xcvrs[1].Model;
            this.label58.Text = xcvrsList.Xcvrs[1].Protocol;
            this.label57.Text = xcvrsList.Xcvrs[1].Timeout + " ms";

            this.label50.Text = xcvrsList.Xcvrs[1].PortSettings.PortName;
            this.label49.Text = xcvrsList.Xcvrs[1].PortSettings.BaudRate.ToString() + " bps";
            this.label48.Text = xcvrsList.Xcvrs[1].PortSettings.Parity;
            this.label47.Text = xcvrsList.Xcvrs[1].PortSettings.DataBits.ToString();
            this.label46.Text = xcvrsList.Xcvrs[1].PortSettings.StopBits;
            this.label45.Text = xcvrsList.Xcvrs[1].PortSettings.Handshake;

            this.label38.Text = xcvrsList.Xcvrs[1].Commands.Read;
            this.label37.Text = xcvrsList.Xcvrs[1].Commands.ReadPrefix;
            this.label36.Text = xcvrsList.Xcvrs[1].Commands.ReadSufix;
            this.label35.Text = xcvrsList.Xcvrs[1].Commands.Write;
            this.label34.Text = xcvrsList.Xcvrs[1].Commands.WritePrefix;
            this.label33.Text = xcvrsList.Xcvrs[1].Commands.WriteSufix;

            this.label66.Text = xcvrsList.Xcvrs[1].CurrentFrequency.ToString();
        }

        private void SetXcvr1PortButtons()
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            if (xcvrsList.Xcvrs[0].SerialPort.IsOpen)
            {
                this.button1.Enabled = false;
                this.button2.Enabled = true;
                button2.Focus();
            }
            else
            {
                this.button1.Enabled = true;
                this.button2.Enabled = false;
                button1.Focus();
            }
        }

        private void SetXcvr2PortButtons()
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            if (xcvrsList.Xcvrs[1].SerialPort.IsOpen)
            {
                this.button4.Enabled = false;
                this.button3.Enabled = true;
                button3.Focus();
            }
            else
            {
                this.button4.Enabled = true;
                this.button3.Enabled = false;
                button4.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            Xcvr.Control.OpenXcvrPort(xcvrsList.Xcvrs[0]);
            if (!xcvrsList.Xcvrs[0].SerialPort.IsOpen)
                MessageBox.Show($"Error connecting to {xcvrsList.Xcvrs[0].PortSettings.PortName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetXcvr1PortButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            Xcvr.Control.CloseXcvrPort(xcvrsList.Xcvrs[0]);
            if (xcvrsList.Xcvrs[0].SerialPort.IsOpen)
                MessageBox.Show($"Error disconnecting from {xcvrsList.Xcvrs[0].PortSettings.PortName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetXcvr1PortButtons();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            Xcvr.Control.OpenXcvrPort(xcvrsList.Xcvrs[1]);
            if (!xcvrsList.Xcvrs[1].SerialPort.IsOpen)
                MessageBox.Show($"Error connecting to {xcvrsList.Xcvrs[1].PortSettings.PortName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetXcvr2PortButtons();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();
            Xcvr.Control.CloseXcvrPort(xcvrsList.Xcvrs[1]);
            if (xcvrsList.Xcvrs[1].SerialPort.IsOpen)
                MessageBox.Show($"Error disconnecting from {xcvrsList.Xcvrs[1].PortSettings.PortName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetXcvr2PortButtons();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Config.XcvrsList xcvrsList = Xcvr.Control.GetXcvrsList();

            Xcvr.Control.ReadXcvrFrequency(xcvrsList.Xcvrs[0]);
            Xcvr.Control.ReadXcvrFrequency(xcvrsList.Xcvrs[1]);
            Xcvr.Control.EqualizeFrequencies();
            this.label65.Text = FormatFrequencyWithDots(xcvrsList.Xcvrs[0].CurrentFrequency);
            this.label66.Text = FormatFrequencyWithDots(xcvrsList.Xcvrs[1].CurrentFrequency);
        }

        private static string FormatFrequencyWithDots(int frequency)
        {
            string formatedFrequency = frequency.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture);
            return formatedFrequency.Replace(",", ".") + " MHz";
        }
    }
}
