using Xcvr;

namespace CatSync
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource? _cancellationReadTokenSource;
        private CancellationTokenSource? _cancellationSyncTokenSource;

        public MainForm()
        {
            try
            {
                InitializeComponent();

                // Lock form resizing
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false; // Optional: Disable the maximize button

                Xcvr.Control.Config();
            }
            catch (ConfigException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetXcvr0Labels();
            SetXcvr1Labels();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            OpenXcvrPort(0);
            SetXcvrPortButtons(0, this.button1, this.button2, this.button5, button6, button7);

            OpenXcvrPort(1);
            SetXcvrPortButtons(1, this.button4, this.button3, this.button8, button9, button10);

            ReadXcvrFrequency(0, this.label65, this.button1, this.button2, this.button5, button6, button7);
            ReadXcvrFrequency(1, this.label66, this.button4, this.button3, this.button8, button9, button10);

            SyncXcvrsFrequencies();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenXcvrPort(0);
            SetXcvrPortButtons(0, this.button1, this.button2, this.button5, button6, button7);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CloseXcvrPort(0);
            SetXcvrPortButtons(0, this.button1, this.button2, this.button5, button6, button7);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenXcvrPort(1);
            SetXcvrPortButtons(1, this.button4, this.button3, this.button8, button9, button10);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CloseXcvrPort(1);
            SetXcvrPortButtons(1, this.button4, this.button3, this.button8, button9, button10);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Xcvr.Control.Xcvrs[0].Frequency.Lead = true;
            this.button5.Enabled = false;
            Xcvr.Control.Xcvrs[0].Frequency.Follow = false;
            this.button6.Enabled = true;

            Xcvr.Control.Xcvrs[1].Frequency.Lead = false;
            this.button8.Enabled = true;
            Xcvr.Control.Xcvrs[1].Frequency.Follow = true;
            this.button9.Enabled = false;

            Xcvr.Control.Xcvrs[0].Frequency.Release = false;
            this.button7.Enabled = true;
            Xcvr.Control.Xcvrs[1].Frequency.Release = false;
            this.button10.Enabled = true;

            button6.Focus();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Xcvr.Control.Xcvrs[0].Frequency.Lead = false;
            this.button5.Enabled = true;
            Xcvr.Control.Xcvrs[0].Frequency.Follow = true;
            this.button6.Enabled = false;

            Xcvr.Control.Xcvrs[1].Frequency.Lead = true;
            this.button8.Enabled = false;
            Xcvr.Control.Xcvrs[1].Frequency.Follow = false;
            this.button9.Enabled = true;

            Xcvr.Control.Xcvrs[0].Frequency.Release = false;
            this.button7.Enabled = true;
            Xcvr.Control.Xcvrs[1].Frequency.Release = false;
            this.button10.Enabled = true;

            button5.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Xcvr.Control.Xcvrs[0].Frequency.Lead = false;
            this.button5.Enabled = true;
            Xcvr.Control.Xcvrs[0].Frequency.Follow = false;
            this.button6.Enabled = true;

            Xcvr.Control.Xcvrs[0].Frequency.Release = true;
            this.button7.Enabled = false;

            button5.Focus();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Xcvr.Control.Xcvrs[1].Frequency.Lead = true;
            this.button8.Enabled = false;
            Xcvr.Control.Xcvrs[1].Frequency.Follow = false;
            this.button9.Enabled = true;

            Xcvr.Control.Xcvrs[0].Frequency.Lead = false;
            this.button5.Enabled = true;
            Xcvr.Control.Xcvrs[0].Frequency.Follow = true;
            this.button6.Enabled = false;

            Xcvr.Control.Xcvrs[1].Frequency.Release = false;
            this.button10.Enabled = true;
            Xcvr.Control.Xcvrs[0].Frequency.Release = false;
            this.button7.Enabled = true;

            button9.Focus();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Xcvr.Control.Xcvrs[1].Frequency.Lead = false;
            this.button8.Enabled = true;
            Xcvr.Control.Xcvrs[1].Frequency.Follow = true;
            this.button9.Enabled = false;

            Xcvr.Control.Xcvrs[0].Frequency.Lead = true;
            this.button5.Enabled = false;
            Xcvr.Control.Xcvrs[0].Frequency.Follow = false;
            this.button6.Enabled = true;

            Xcvr.Control.Xcvrs[1].Frequency.Release = false;
            this.button10.Enabled = true;
            Xcvr.Control.Xcvrs[0].Frequency.Release = false;
            this.button7.Enabled = true;

            button8.Focus();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Xcvr.Control.Xcvrs[1].Frequency.Lead = false;
            this.button8.Enabled = true;
            Xcvr.Control.Xcvrs[1].Frequency.Follow = false;
            this.button9.Enabled = true;

            Xcvr.Control.Xcvrs[1].Frequency.Release = true;
            this.button10.Enabled = false;

            button8.Focus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _cancellationReadTokenSource?.Cancel();
            _cancellationSyncTokenSource?.Cancel();

            CloseXcvrPort(0);
            SetXcvrPortButtons(0, this.button1, this.button2, this.button5, button6, button7);
            DisposeXcvrPort(0);
            CloseXcvrPort(1);
            SetXcvrPortButtons(1, this.button1, this.button2, this.button5, button6, button7);
            DisposeXcvrPort(1);

            Environment.Exit(1);
        }

        private void SetXcvr0Labels()
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

            this.label65.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[1].Frequency.Current);
        }

        private void SetXcvr1Labels()
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

            this.label66.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[1].Frequency.Current);
        }

        private void OpenXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.OpenPort(Xcvr.Control.Xcvrs[id]);
            }
            catch (OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Xcvr.Control.ClosePort(Xcvr.Control.Xcvrs[id]);
            }
        }

        private async void ReadXcvrFrequency(int id, Label labelFrequency, Button buttonOpen, Button buttonClose, Button buttonLead, Button buttonFollow, Button buttnRelease)
        {
            _cancellationReadTokenSource = new CancellationTokenSource();
            CancellationToken readToken = _cancellationReadTokenSource.Token;

            await Task.Run(() =>
            {
                while (!readToken.IsCancellationRequested)
                {
                    try
                    {
                        if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
                        {
                            Xcvr.Control.ReadFrequency(Xcvr.Control.Xcvrs[id]);
                            labelFrequency.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[id].Frequency.Current);
                        }
                        else
                        {
                            labelFrequency.Text = FormatFrequencyWithDots(0);
                            SetXcvrPortButtons(id, buttonOpen, buttonClose, buttonLead, buttonFollow, buttnRelease);
                        }
                    }
                    catch (Exception ex)
                    {
                        labelFrequency.Text = FormatFrequencyWithDots(0);
                        Xcvr.Control.ClosePort(Xcvr.Control.Xcvrs[id]);
                        SetXcvrPortButtons(id, buttonOpen, buttonClose, buttonLead, buttonFollow, buttnRelease);
                        MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }, readToken);
        }

        private async void SyncXcvrsFrequencies()
        {
            _cancellationSyncTokenSource = new CancellationTokenSource();
            CancellationToken syncToken = _cancellationSyncTokenSource.Token;

            await Task.Run(() =>
            {
                while (!syncToken.IsCancellationRequested)
                {
                    try
                    {
                        Xcvr.Control.SyncFrequencies();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }, syncToken);
        }

        private void CloseXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.ClosePort(Xcvr.Control.Xcvrs[id]);
            }
            catch (OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisposeXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.DisposePort(Xcvr.Control.Xcvrs[id]);
            }
            catch (OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetXcvrPortButtons(int id, Button buttonOpen, Button buttonClose, Button buttonLead, Button buttonFollow, Button buttnRelease)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                buttonOpen.Enabled = false;
                buttonClose.Enabled = true;
                buttonLead.Enabled = true;
                buttonFollow.Enabled = true;
                buttnRelease.Enabled = true;

                buttonLead.Focus();
            }
            else
            {
                buttonOpen.Enabled = true;
                buttonClose.Enabled = false;
                buttonLead.Enabled = false;
                buttonFollow.Enabled = false;
                buttnRelease.Enabled = false;

                buttonOpen.Focus();
            }
        }

        private static string FormatFrequencyWithDots(int frequency)
        {
            if (frequency == 0)
            {
                return "000.000.000 MHz";
            }
            else
            {
                string formatedFrequency = frequency.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture).Replace(",", ".");
                int formatedFrequencyLenght = formatedFrequency.Length;
                string frequencyUnit = "MHz";
                if (formatedFrequencyLenght >= 5 && formatedFrequencyLenght <= 7)
                {
                    frequencyUnit = "KHz";
                }
                else if (formatedFrequencyLenght >= 1 && formatedFrequencyLenght <= 3)
                {
                    frequencyUnit = "Hz";
                }
                return ($"{formatedFrequency} {frequencyUnit}");
            }
        }
    }
}