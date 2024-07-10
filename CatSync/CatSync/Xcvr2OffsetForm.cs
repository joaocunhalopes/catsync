namespace CatSync
{
    public partial class Xcvr2OffsetForm : Form
    {
        public Xcvr2OffsetForm()
        {
            InitializeComponent();

            // TODO: Encapsulate form UI otions on a seperate static method.
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Lock form resizing.
            this.MaximizeBox = false; // Optional: Disable the maximize button.
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            SetOffsetValue(Xcvr.Control.Xcvrs[1]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateOffsetValue(Xcvr.Control.Xcvrs[1]);
        }

        private void SetOffsetValue(Config.Xcvr xcvr)
        {
            this.label4.Text = Util.Formater.FormatFrequencyWithDots(xcvr.Frequency.Offset);
            this.label5.Text = Util.Formater.FormatFrequencyWithDots(xcvr.Frequency.Offset);
            this.label5.ForeColor = Color.DarkGreen;
            this.numericUpDown1.Value = xcvr.Frequency.Offset;
        }

        private void UpdateOffsetValue(Config.Xcvr xcvr)
        {
            DialogResult result = MessageBox.Show("Offset will be updated. Do you want to proceed?", "CatSync", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                xcvr.Frequency.Offset = (int)this.numericUpDown1.Value;
                this.Close();
            }
        }

        private void numericUpDown1TextBox_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(numericUpDown1.Text, out decimal currentValue))
            {
                // Enforce the lower limit
                if (currentValue < numericUpDown1.Minimum)
                {
                    numericUpDown1.Value = numericUpDown1.Minimum;
                }
                // Enforce the upper limit
                else if (currentValue > numericUpDown1.Maximum)
                {
                    numericUpDown1.Value = numericUpDown1.Maximum;
                }
                else
                {
                    numericUpDown1.Value = currentValue;
                }

                // Perform actions with the current value
                this.label5.Text = Util.Formater.FormatFrequencyWithDots((int)numericUpDown1.Value);
            }
        }
    }
}