using Xcvr;

namespace CatSync
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            try
            {
                InitializeComponent();

                Xcvr.Control.Config();
            }
            catch (ConfigException ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FillXcvr0Labels();
            FillXcvr1Labels();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            OpenXcvrPort(0);
            SetXcvrPortButtons(0, this.button1, this.button2);

            OpenXcvrPort(1);
            SetXcvrPortButtons(1, this.button4, this.button3);

            ReadFrequencyXcvr0();
            ReadFrequencyXcvr1();
        }

        private async void ReadFrequencyXcvr0()
        {
            await Task.Run(() =>
            {
                while (true) // Example: Continuous operation until program exit
                {
                    try
                    {
                        if (Xcvr.Control.Xcvrs[0].SerialPort.IsOpen)
                        {
                            Xcvr.Control.ReadFrequency(Xcvr.Control.Xcvrs[0]);
                            this.label65.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[0].CurrentFrequency);
                        }
                        else
                        {
                            this.label65.Text = FormatFrequencyWithDots(0);
                            SetXcvrPortButtons(0, this.button1, this.button2);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle and log exceptions
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }

        private async void ReadFrequencyXcvr1()
        {
            await Task.Run(() =>
            {
                while (true) // Example: Continuous operation until program exit
                {
                    try
                    {
                        if (Xcvr.Control.Xcvrs[1].SerialPort.IsOpen)
                        {
                            Xcvr.Control.ReadFrequency(Xcvr.Control.Xcvrs[1]);
                            this.label66.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[1].CurrentFrequency);
                        }
                        else
                        {
                            this.label66.Text = FormatFrequencyWithDots(0);
                            SetXcvrPortButtons(1, this.button4, this.button3);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle and log exceptions
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenXcvrPort(0);
            SetXcvrPortButtons(0, this.button1, this.button2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CloseXcvrPort(0);
            SetXcvrPortButtons(0, this.button1, this.button2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenXcvrPort(1);
            SetXcvrPortButtons(1, this.button4, this.button3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CloseXcvrPort(1);
            SetXcvrPortButtons(1, this.button4, this.button3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Xcvr.Control.ReadFrequency(Xcvr.Control.Xcvrs[0]);
            Xcvr.Control.ReadFrequency(Xcvr.Control.Xcvrs[1]);
            Xcvr.Control.EqualizeFrequencies();
            this.label65.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[0].CurrentFrequency);
            this.label66.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[1].CurrentFrequency);
        }

        private void FillXcvr0Labels()
        {
            this.label17.Text = Xcvr.Control.Xcvrs[0].Manufacturer;
            this.label18.Text = Xcvr.Control.Xcvrs[0].Model;
            this.label19.Text = Xcvr.Control.Xcvrs[0].Protocol;
            this.label20.Text = Xcvr.Control.Xcvrs[0].Timeout + " ms";

            this.label21.Text = Xcvr.Control.Xcvrs[0].PortSettings.PortName;
            this.label22.Text = Xcvr.Control.Xcvrs[0].PortSettings.BaudRate.ToString() + " bps";
            this.label23.Text = Xcvr.Control.Xcvrs[0].PortSettings.Parity;
            this.label24.Text = Xcvr.Control.Xcvrs[0].PortSettings.DataBits.ToString();
            this.label25.Text = Xcvr.Control.Xcvrs[0].PortSettings.StopBits;
            this.label26.Text = Xcvr.Control.Xcvrs[0].PortSettings.Handshake;

            this.label27.Text = Xcvr.Control.Xcvrs[0].Commands.Read;
            this.label28.Text = Xcvr.Control.Xcvrs[0].Commands.ReadPrefix;
            this.label29.Text = Xcvr.Control.Xcvrs[0].Commands.ReadSufix;
            this.label30.Text = Xcvr.Control.Xcvrs[0].Commands.Write;
            this.label31.Text = Xcvr.Control.Xcvrs[0].Commands.WritePrefix;
            this.label32.Text = Xcvr.Control.Xcvrs[0].Commands.WriteSufix;

            this.label65.Text = Xcvr.Control.Xcvrs[0].CurrentFrequency.ToString();
        }

        private void FillXcvr1Labels()
        {
            this.label60.Text = Xcvr.Control.Xcvrs[1].Manufacturer;
            this.label59.Text = Xcvr.Control.Xcvrs[1].Model;
            this.label58.Text = Xcvr.Control.Xcvrs[1].Protocol;
            this.label57.Text = Xcvr.Control.Xcvrs[1].Timeout + " ms";

            this.label50.Text = Xcvr.Control.Xcvrs[1].PortSettings.PortName;
            this.label49.Text = Xcvr.Control.Xcvrs[1].PortSettings.BaudRate.ToString() + " bps";
            this.label48.Text = Xcvr.Control.Xcvrs[1].PortSettings.Parity;
            this.label47.Text = Xcvr.Control.Xcvrs[1].PortSettings.DataBits.ToString();
            this.label46.Text = Xcvr.Control.Xcvrs[1].PortSettings.StopBits;
            this.label45.Text = Xcvr.Control.Xcvrs[1].PortSettings.Handshake;

            this.label38.Text = Xcvr.Control.Xcvrs[1].Commands.Read;
            this.label37.Text = Xcvr.Control.Xcvrs[1].Commands.ReadPrefix;
            this.label36.Text = Xcvr.Control.Xcvrs[1].Commands.ReadSufix;
            this.label35.Text = Xcvr.Control.Xcvrs[1].Commands.Write;
            this.label34.Text = Xcvr.Control.Xcvrs[1].Commands.WritePrefix;
            this.label33.Text = Xcvr.Control.Xcvrs[1].Commands.WriteSufix;

            this.label66.Text = Xcvr.Control.Xcvrs[1].CurrentFrequency.ToString();
        }

        private void OpenXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.OpenPort(Xcvr.Control.Xcvrs[id]);
            }
            catch (OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CloseXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.ClosePort(Xcvr.Control.Xcvrs[id]);
            }
            catch (OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetXcvrPortButtons(int id, Button buttonOpen, Button buttonClose)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                buttonOpen.Enabled = false;
                buttonClose.Enabled = true;
                buttonClose.Focus();
            }
            else
            {
                buttonOpen.Enabled = true;
                buttonClose.Enabled = false;
                buttonOpen.Focus();
            }
        }

        private static string FormatFrequencyWithDots(int frequency)
        {
            string formatedFrequency = frequency.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture);
            return formatedFrequency.Replace(",", ".") + " MHz";
        }
    }
}
