namespace CatSync
{
    public partial class Xcvr1ConfigForm : Form
    {
        public Xcvr1ConfigForm()
        {
            InitializeComponent();

            // TODO: Encapsulate form UI otions on a seperate static method.
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Lock form resizing.
            this.MaximizeBox = false; // Optional: Disable the maximize button.

            Xcvr.Control.ReadXcvrsListConfig();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            PopulateComboBox();
            SetXcvrValues(Xcvr.Control.Xcvrs[0]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                int selected = (int)comboBox1.SelectedValue;
                SetXcvrValues(Xcvr.Control.XcvrsList[selected - 1]);
            }
            else
            {
                MessageBox.Show("Please select an Transceiver/Receiver.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetXcvrValues(Xcvr.Control.Xcvrs[0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Configuration will be saved. Do you want to proceed?", "CatSync", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                Xcvr.Control.CloseXcvrPort(Xcvr.Control.Xcvrs[0]);
                UpdateXcvrValues(Xcvr.Control.Xcvrs[0]);
                Config.Control.WriteXcvrsConfig(Xcvr.Control.Xcvrs);
                this.Close();
            }
        }

        private void PopulateComboBox()
        {
            comboBox1.DisplayMember = "DisplayName";
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = Xcvr.Control.XcvrsList;
        }

        private void SetXcvrValues(Config.Xcvr xcvr)
        {
            this.textBox1.Text = xcvr.Manufacturer;
            this.textBox2.Text = xcvr.Model;
            this.comboBox2.Text = xcvr.Protocol;
            this.numericUpDown1.Value = xcvr.Latency;
            this.numericUpDown2.Value = xcvr.Frequency.Offset;

            this.comboBox3.Text = xcvr.PortSettings.PortName;
            this.comboBox4.Text = xcvr.PortSettings.BaudRate.ToString();
            this.comboBox5.Text = xcvr.PortSettings.Parity;
            this.comboBox6.Text = xcvr.PortSettings.DataBits.ToString();
            this.comboBox7.Text = xcvr.PortSettings.StopBits;
            this.comboBox8.Text = xcvr.PortSettings.Handshake;

            this.textBox3.Text = xcvr.Frequency.ReadCommand;
            this.textBox4.Text = xcvr.Frequency.ReadCommandPrefix;
            this.textBox5.Text = xcvr.Frequency.ReadCommandSufix;
            this.textBox6.Text = xcvr.Frequency.SetCommandPrefix;
            this.textBox7.Text = xcvr.Frequency.SetCommandSufix;
        }

        private void UpdateXcvrValues(Config.Xcvr xcvr)
        {
            xcvr.Manufacturer = this.textBox1.Text;
            xcvr.Model = this.textBox2.Text;
            xcvr.Protocol = this.comboBox2.Text;
            xcvr.Latency = (int)this.numericUpDown1.Value;
            xcvr.Frequency.Offset = (int)this.numericUpDown2.Value;

            xcvr.PortSettings.PortName = this.comboBox3.Text;
            xcvr.PortSettings.BaudRate = int.Parse(this.comboBox4.Text);
            xcvr.PortSettings.Parity = this.comboBox5.Text;
            xcvr.PortSettings.DataBits = int.Parse(this.comboBox6.Text);
            xcvr.PortSettings.StopBits = this.comboBox7.Text;
            xcvr.PortSettings.Handshake = this.comboBox8.Text;

            xcvr.Frequency.ReadCommand = this.textBox3.Text;
            xcvr.Frequency.ReadCommandPrefix = this.textBox4.Text;
            xcvr.Frequency.ReadCommandSufix = this.textBox5.Text;
            xcvr.Frequency.SetCommandPrefix = this.textBox6.Text;
            xcvr.Frequency.SetCommandSufix = this.textBox7.Text;
        }
    }
}