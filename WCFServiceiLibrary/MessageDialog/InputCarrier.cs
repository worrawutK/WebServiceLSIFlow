using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MessageDialog
{
    public partial class InputCarrier : Form
    {
        public string FullCode { get; set; }
        int c_MaxValue;
        public InputCarrier(string header, int maxValue, Color color)
        {
            InitializeComponent();
            lbDisplay.Text = header;
            c_MaxValue = maxValue;
            this.BackColor = color;
        }
        public static DialogResult InputQrCode(string header,int maxValue,Color color)
        {
            InputCarrier inputCarrier = new InputCarrier(header, maxValue, color);
            return inputCarrier.ShowDialog();
        }
        private void ProgressBarManual(double count)
        {
            labelTextScan.Visible = false;
            Size sizeMax = panel3.Size;
            // Double sizeValue = ((sizeMax.Width + 0.0) / 100) * count;
            Point point = pictureBox2.Location;
            point.X += (int)Math.Ceiling(count);
            pictureBox2.Location = point;
            Size size = label1.Size;
            size.Width += (int)Math.Ceiling(count);
            label1.Size = size;
            if (label1.BackColor != Color.LimeGreen)
            {
                label1.BackColor = Color.LimeGreen;
            }

            if (sizeMax.Width <= label1.Size.Width)
            {
                Point point2 = pictureBox2.Location;
                point2.X += pictureBox2.Width;
                pictureBox2.Location = point2;
            }

        }

        private void TextBoxQrCodeInput_KeyPress(object sender, KeyPressEventArgs e)
        {

            ProgressBarManual((panel3.Size.Width + 0.0) / c_MaxValue);
            if (e.KeyChar == (Char)13)
            {
                if (c_MaxValue != TextBoxQrCodeInput.Text.Length)
                {
                    MessageBoxDialog.ShowMessageDialog("Read Qr Code", "ขนาดของ qr ไม่ถูกต้อง (" + TextBoxQrCodeInput.Text.Length + ")","Carrier");
                    return;
                }
                FullCode = TextBoxQrCodeInput.Text;
                this.DialogResult = DialogResult.OK;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (labelTextScan.ForeColor == Color.Orange)
            {
                labelTextScan.ForeColor = Color.YellowGreen;
                panel1.BackColor = Color.YellowGreen;
            }
            else
            {
                labelTextScan.ForeColor = Color.Orange;
                panel1.BackColor = Color.Orange;
            }


        }

        private void PictureboxQrCodeInputCheck_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void InputCarrierDialog_Load(object sender, EventArgs e)
        {
            TextBoxQrCodeInput.Focus();
        }
    }
}
