using Xcvr;

namespace CatSync
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource? _readInformationCancellationTokenSource;
        private CancellationTokenSource? _readFrequencyCancellationTokenSource;
        private CancellationTokenSource? _syncCancellationTokenSource;

        // Connect
        private bool _button1Default = true;
        private bool _button2Default = true;

        // Master
        private bool _button3Default = true;
        private bool _button4Default = true;

        // Sync
        private bool _button5Default = true;
        private bool _button6Default = true;

        // Offset
        private bool _button7Default = true;
        private bool _button8Default = true;

        public MainForm()
        {
            try
            {
                InitializeComponent();

                // TODO: Encapsulate form UI otions on a seperate static method.
                this.FormBorderStyle = FormBorderStyle.FixedSingle; // Lock form resizing.
                this.MaximizeBox = false; // Optional: Disable the maximize button.

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
            ReadXcvrsIntormation();

            OpenXcvrPort(0);
            OpenXcvrPort(1);

            ReadXcvrFrequency(0);
            ReadXcvrFrequency(1);

            SyncXcvrsFrequencies();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_button1Default)
            {
                OpenXcvrPort(0);
            }
            else
            {
                CloseXcvrPort(0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_button2Default)
            {
                OpenXcvrPort(1);
            }
            else
            {
                CloseXcvrPort(1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_button3Default)
            {
                Xcvr.Control.Xcvrs[0].Frequency.Master = true;
                Xcvr.Control.Xcvrs[1].Frequency.Master = false;
            }
            else
            {
                Xcvr.Control.Xcvrs[0].Frequency.Master = false;
                Xcvr.Control.Xcvrs[1].Frequency.Master = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_button4Default)
            {
                Xcvr.Control.Xcvrs[1].Frequency.Master = true;
                Xcvr.Control.Xcvrs[0].Frequency.Master = false;
            }
            else
            {
                Xcvr.Control.Xcvrs[1].Frequency.Master = false;
                Xcvr.Control.Xcvrs[0].Frequency.Master = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_button5Default)
            {
                Xcvr.Control.Xcvrs[0].Frequency.SyncOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[0].Frequency.SyncOn = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (_button6Default)
            {
                Xcvr.Control.Xcvrs[1].Frequency.SyncOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[1].Frequency.SyncOn = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (_button7Default)
            {
                Xcvr.Control.Xcvrs[0].Frequency.OffsetOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[0].Frequency.OffsetOn = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (_button8Default)
            {
                Xcvr.Control.Xcvrs[1].Frequency.OffsetOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[1].Frequency.OffsetOn = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _syncCancellationTokenSource?.Cancel();
            _readFrequencyCancellationTokenSource?.Cancel();
            _readInformationCancellationTokenSource?.Cancel();

            CloseXcvrPort(0);
            DisposeXcvrPort(0);

            CloseXcvrPort(1);
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

        private async void ReadXcvrsIntormation()
        {
            _readInformationCancellationTokenSource = new CancellationTokenSource();
            CancellationToken readInformationCancellationToken = _readInformationCancellationTokenSource.Token;

            await Task.Run(() =>
            {
                while (!readInformationCancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        SetXcvrFrequencyInformation(0, this.label65, this.label67);
                        SetXcvrFrequencyInformation(1, this.label66, this.label68);

                        SetXcvrConnectButtonInformation(0, this.button1, ref _button1Default);
                        SetXcvrConnectButtonInformation(1, this.button2, ref _button2Default);

                        SetXcvrMasterButtonInformation(0, this.button3, ref _button3Default);
                        SetXcvrMasterButtonInformation(1, this.button4, ref _button4Default);

                        SetXcvrSyncButtonInformation(0, this.button5, ref _button5Default);
                        SetXcvrSyncButtonInformation(1, this.button6, ref _button6Default);

                        SetXcvrOffsetButtonInformation(0, this.button7, ref _button7Default);
                        SetXcvrOffsetButtonInformation(1, this.button8, ref _button8Default);

                        SetXcvrFrequencyStatus(0, this.label69, this.label70, this.label71, this.label72);
                        SetXcvrFrequencyStatus(1, this.label73, this.label74, this.label75, this.label76);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }, readInformationCancellationToken);
        }

        private async void ReadXcvrFrequency(int id)
        {
            _readFrequencyCancellationTokenSource = new CancellationTokenSource();
            CancellationToken readFrequencyCancelationToken = _readFrequencyCancellationTokenSource.Token;

            await Task.Run(() =>
            {
                while (!readFrequencyCancelationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
                        {
                            Xcvr.Control.ReadFrequency(Xcvr.Control.Xcvrs[id]);
                        }
                        else
                        {
                            Xcvr.Control.Xcvrs[id].Frequency.Current = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Xcvr.Control.Xcvrs[id].Frequency.Current = 0;
                        Xcvr.Control.ClosePort(Xcvr.Control.Xcvrs[id]); // Check if this is needed.
                        MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }, readFrequencyCancelationToken);
        }

        private async void SyncXcvrsFrequencies()
        {
            _syncCancellationTokenSource = new CancellationTokenSource();
            CancellationToken syncCancellationToken = _syncCancellationTokenSource.Token;

            await Task.Run(() =>
            {
                while (!syncCancellationToken.IsCancellationRequested)
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
            }, syncCancellationToken);
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

        private void SetXcvrFrequencyInformation(int id, Label frequencyLabel, Label offsetLabel)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                frequencyLabel.ForeColor = Color.Black;
                if (Xcvr.Control.Xcvrs[id].Frequency.OffsetOn)
                {
                    offsetLabel.ForeColor = Color.Black;
                }
                else
                {
                    offsetLabel.ForeColor = Color.Gray;
                }
            }
            else
            {
                frequencyLabel.ForeColor = Color.Gray;
                offsetLabel.ForeColor = Color.Gray;
            }
            frequencyLabel.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[id].Frequency.Current);
            offsetLabel.Text = FormatFrequencyWithDots(Xcvr.Control.Xcvrs[id].Frequency.Offset);
        }

        private void SetXcvrConnectButtonInformation(int id, Button connect, ref bool buttonDefault)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                connect.Text = "Disconnect";
                buttonDefault = false;
            }
            else
            {
                connect.Text = "Connect";
                buttonDefault = true;
            }
        }

        private void SetXcvrMasterButtonInformation(int id, Button masterButton, ref bool masterButtonDefault)
        {
            if (Xcvr.Control.Xcvrs[0].Frequency.Master == Xcvr.Control.Xcvrs[1].Frequency.Master)
            {
                Xcvr.Control.Xcvrs[0].Frequency.Master = true;
                Xcvr.Control.Xcvrs[1].Frequency.Master = false;
            }

            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                if (Xcvr.Control.Xcvrs[id].Frequency.Master)
                {
                    masterButton.Text = "Slave";
                    masterButtonDefault = false;
                }
                else
                {
                    masterButton.Text = "Master";
                    masterButtonDefault = true;
                }
                masterButton.Enabled = true;
            }
            else
            {
                masterButton.Enabled = false;
            }
        }

        private void SetXcvrSyncButtonInformation(int id, Button syncButton, ref bool syncButtonDefault)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                if (Xcvr.Control.Xcvrs[id].Frequency.SyncOn)
                {
                    syncButton.Text = "Sync Off";
                    syncButtonDefault = false;
                }
                else
                {
                    syncButton.Text = "Sync On";
                    syncButtonDefault = true;
                }
                syncButton.Enabled = true;
            }
            else
            {
                syncButton.Enabled = false;
            }
        }

        private void SetXcvrOffsetButtonInformation(int id, Button offsetButton, ref bool offsetButtonDefault)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                if (Xcvr.Control.Xcvrs[id].Frequency.OffsetOn)
                {
                    offsetButton.Text = "Offset Off";
                    offsetButtonDefault = false;
                }
                else
                {
                    offsetButton.Text = "Offset On";
                    offsetButtonDefault = true;
                }
                offsetButton.Enabled = true;
            }
            else
            {
                offsetButton.Enabled = false;
            }
        }

        private void SetXcvrFrequencyStatus(int id, Label conectedLabel, Label leadingLabel, Label syncOnLabel, Label offsetOnLabel)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                conectedLabel.Text = "Connected";
                conectedLabel.ForeColor = Color.White;
                conectedLabel.BackColor = Color.Green;
            }
            else
            { 
                conectedLabel.Text = "Disconnected";
                conectedLabel.ForeColor = Color.White;
                conectedLabel.BackColor = Color.Red;
            }

            if (Xcvr.Control.Xcvrs[id].Frequency.Master)
            {
                leadingLabel.Text = "Master";
                leadingLabel.ForeColor = Color.White;
                leadingLabel.BackColor = Color.Black;
            }
            else
            { 
                leadingLabel.Text = "Slave";
                leadingLabel.ForeColor = Color.White;
                leadingLabel.BackColor = Color.Gray;
            }

            if (Xcvr.Control.Xcvrs[id].Frequency.SyncOn)
            { 
                syncOnLabel.Text = "Sync is On";
                syncOnLabel.ForeColor = Color.White;
                syncOnLabel.BackColor = Color.Green;
            }
            else
            { 
                syncOnLabel.Text = "Sync is Off";
                syncOnLabel.ForeColor = Color.White;
                syncOnLabel.BackColor = Color.Red;
            }

            if (Xcvr.Control.Xcvrs[id].Frequency.OffsetOn)
            { 
                offsetOnLabel.Text = "Offset is On";
                offsetOnLabel.ForeColor = Color.White;
                offsetOnLabel.BackColor = Color.Green;
            }
            else
            { 
                offsetOnLabel.Text = "Offset is Off";
                offsetOnLabel.ForeColor = Color.White;
                offsetOnLabel.BackColor = Color.Red;
            }
        }
    }
}