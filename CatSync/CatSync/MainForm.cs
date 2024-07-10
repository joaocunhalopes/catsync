namespace CatSync
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource _setInformationCancellationTokenSource = new();
        private CancellationTokenSource _syncXcvrsCancellationTokenSource = new();

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

                Xcvr.Control.ReadXcvrsConfig();
            }
            catch (Xcvr.ConfigException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetXcvr0Labels(Xcvr.Control.Xcvrs[0]);
            SetXcvr1Labels(Xcvr.Control.Xcvrs[1]);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            SetXcvrsInformation(_setInformationCancellationTokenSource.Token);

            OpenXcvrPort(0);
            OpenXcvrPort(1);

            SyncXcvrsFrequency(_syncXcvrsCancellationTokenSource.Token);
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
                Xcvr.Control.Xcvrs[0].Switches.MasterOn = true;
                Xcvr.Control.Xcvrs[1].Switches.MasterOn = false;
            }
            else
            {
                Xcvr.Control.Xcvrs[0].Switches.MasterOn = false;
                Xcvr.Control.Xcvrs[1].Switches.MasterOn = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_button4Default)
            {
                Xcvr.Control.Xcvrs[1].Switches.MasterOn = true;
                Xcvr.Control.Xcvrs[0].Switches.MasterOn = false;
            }
            else
            {
                Xcvr.Control.Xcvrs[1].Switches.MasterOn = false;
                Xcvr.Control.Xcvrs[0].Switches.MasterOn = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_button5Default)
            {
                Xcvr.Control.Xcvrs[0].Switches.SyncOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[0].Switches.SyncOn = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (_button6Default)
            {
                Xcvr.Control.Xcvrs[1].Switches.SyncOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[1].Switches.SyncOn = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (_button7Default)
            {
                Xcvr.Control.Xcvrs[0].Switches.OffsetOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[0].Switches.OffsetOn = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (_button8Default)
            {
                Xcvr.Control.Xcvrs[1].Switches.OffsetOn = true;
            }
            else
            {
                Xcvr.Control.Xcvrs[1].Switches.OffsetOn = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Xcvr1OffsetForm offsetForm = new Xcvr1OffsetForm();
            offsetForm.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Xcvr2OffsetForm offsetForm = new Xcvr2OffsetForm();
            offsetForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _syncXcvrsCancellationTokenSource.Cancel();
            _setInformationCancellationTokenSource.Cancel();

            DisposeXcvrPort(0);
            DisposeXcvrPort(1);

            Environment.Exit(1);
        }

        private void transceiverReceiver1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xcvr1ConfigForm configForm = new Xcvr1ConfigForm();
            configForm.ShowDialog();
        }

        private void transceiverReceiver2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xcvr2ConfigForm configForm = new Xcvr2ConfigForm();
            configForm.ShowDialog();
        }

        private void transceiverReceiver1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Xcvr1OffsetForm offsetForm = new Xcvr1OffsetForm();
            offsetForm.ShowDialog();
        }

        private void transceiverReceiver2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Xcvr2OffsetForm offsetForm = new Xcvr2OffsetForm();
            offsetForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _syncXcvrsCancellationTokenSource.Cancel();
            _setInformationCancellationTokenSource.Cancel();
        }

        private void SetXcvr0Labels(Config.Xcvr xcvr)
        {
            this.label17.Text = xcvr.Manufacturer;
            this.label18.Text = xcvr.Model;
            this.label19.Text = xcvr.Protocol;
            this.label20.Text = xcvr.Latency + " ms";

            this.label21.Text = xcvr.PortSettings.PortName;
            this.label22.Text = xcvr.PortSettings.BaudRate.ToString() + " bps";
            this.label23.Text = xcvr.PortSettings.Parity;
            this.label24.Text = xcvr.PortSettings.DataBits.ToString();
            this.label25.Text = xcvr.PortSettings.StopBits;
            this.label26.Text = xcvr.PortSettings.Handshake;

            this.label27.Text = xcvr.Frequency.ReadCommand;
            this.label28.Text = xcvr.Frequency.ReadCommandPrefix;
            this.label29.Text = xcvr.Frequency.ReadCommandSufix;
            this.label31.Text = xcvr.Frequency.SetCommandPrefix;
            this.label32.Text = xcvr.Frequency.SetCommandSufix;
        }

        private void SetXcvr1Labels(Config.Xcvr xcvr)
        {
            this.label60.Text = xcvr.Manufacturer;
            this.label59.Text = xcvr.Model;
            this.label58.Text = xcvr.Protocol;
            this.label57.Text = xcvr.Latency + " ms";

            this.label50.Text = xcvr.PortSettings.PortName;
            this.label49.Text = xcvr.PortSettings.BaudRate.ToString() + " bps";
            this.label48.Text = xcvr.PortSettings.Parity;
            this.label47.Text = xcvr.PortSettings.DataBits.ToString();
            this.label46.Text = xcvr.PortSettings.StopBits;
            this.label45.Text = xcvr.PortSettings.Handshake;

            this.label38.Text = xcvr.Frequency.ReadCommand;
            this.label37.Text = xcvr.Frequency.ReadCommandPrefix;
            this.label36.Text = xcvr.Frequency.ReadCommandSufix;
            this.label34.Text = xcvr.Frequency.SetCommandPrefix;
            this.label33.Text = xcvr.Frequency.SetCommandSufix;
        }

        private void OpenXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.OpenXcvrPort(Xcvr.Control.Xcvrs[id]);
            }
            catch (Xcvr.OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Xcvr.Control.CloseXcvrPort(Xcvr.Control.Xcvrs[id]);
            }
        }

        private async void SetXcvrsInformation(CancellationToken setInformationCancellationToken)
        {
            await Task.Run(() =>
            {
                while (!setInformationCancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        SetXcvr0Labels(Xcvr.Control.Xcvrs[0]);
                        SetXcvr1Labels(Xcvr.Control.Xcvrs[1]);

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
            }, setInformationCancellationToken);
        }

        private async void SyncXcvrsFrequency(CancellationToken syncXcvrsCancellationToken)
        {
            await Task.Run(() =>
            {
                while (!syncXcvrsCancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        Xcvr.Control.SyncXcvrsFrequency();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }, syncXcvrsCancellationToken);
        }

        private void CloseXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.CloseXcvrPort(Xcvr.Control.Xcvrs[id]);
            }
            catch (Xcvr.OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisposeXcvrPort(int id)
        {
            try
            {
                Xcvr.Control.DisposeXcvrPort(Xcvr.Control.Xcvrs[id]);
            }
            catch (Xcvr.OpenPortException ex)
            {
                MessageBox.Show($"{ex.Message}", "CatSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetXcvrFrequencyInformation(int id, Label frequencyLabel, Label offsetLabel)
        {
            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                frequencyLabel.ForeColor = Color.Black;
                if (Xcvr.Control.Xcvrs[id].Switches.OffsetOn)
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
            frequencyLabel.Text = Util.Formater.FormatFrequencyWithDots(Xcvr.Control.Xcvrs[id].Frequency.Current);
            offsetLabel.Text = Util.Formater.FormatFrequencyWithDots(Xcvr.Control.Xcvrs[id].Frequency.Offset);
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
            if (Xcvr.Control.Xcvrs[0].Switches.MasterOn == Xcvr.Control.Xcvrs[1].Switches.MasterOn)
            {
                Xcvr.Control.Xcvrs[0].Switches.MasterOn = true;
                Xcvr.Control.Xcvrs[1].Switches.MasterOn = false;
            }

            if (Xcvr.Control.Xcvrs[id].SerialPort.IsOpen)
            {
                if (Xcvr.Control.Xcvrs[id].Switches.MasterOn)
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
                if (Xcvr.Control.Xcvrs[id].Switches.SyncOn)
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
                if (Xcvr.Control.Xcvrs[id].Switches.OffsetOn)
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

            if (Xcvr.Control.Xcvrs[id].Switches.MasterOn)
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

            if (Xcvr.Control.Xcvrs[id].Switches.SyncOn)
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

            if (Xcvr.Control.Xcvrs[id].Switches.OffsetOn)
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