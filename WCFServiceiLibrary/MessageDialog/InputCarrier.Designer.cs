namespace MessageDialog
{
    partial class InputCarrier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelTextScan = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TextBoxQrCodeInput = new System.Windows.Forms.TextBox();
            this.lbDisplay = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.PictureboxQrCodeInput = new System.Windows.Forms.PictureBox();
            this.PictureboxQrCodeInputCheck = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureboxQrCodeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureboxQrCodeInputCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTextScan
            // 
            this.labelTextScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTextScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTextScan.ForeColor = System.Drawing.Color.DarkOrange;
            this.labelTextScan.Location = new System.Drawing.Point(0, 0);
            this.labelTextScan.Name = "labelTextScan";
            this.labelTextScan.Size = new System.Drawing.Size(365, 56);
            this.labelTextScan.TabIndex = 63;
            this.labelTextScan.Text = "Please scan qr code";
            this.labelTextScan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(24, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(58, 57);
            this.panel1.TabIndex = 67;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.PictureboxQrCodeInput);
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(42, 42);
            this.panel2.TabIndex = 59;
            // 
            // TextBoxQrCodeInput
            // 
            this.TextBoxQrCodeInput.Location = new System.Drawing.Point(22, 166);
            this.TextBoxQrCodeInput.Name = "TextBoxQrCodeInput";
            this.TextBoxQrCodeInput.Size = new System.Drawing.Size(60, 20);
            this.TextBoxQrCodeInput.TabIndex = 66;
            this.TextBoxQrCodeInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxQrCodeInput_KeyPress);
            // 
            // lbDisplay
            // 
            this.lbDisplay.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDisplay.ForeColor = System.Drawing.Color.Black;
            this.lbDisplay.Location = new System.Drawing.Point(117, 25);
            this.lbDisplay.Name = "lbDisplay";
            this.lbDisplay.Size = new System.Drawing.Size(343, 31);
            this.lbDisplay.TabIndex = 68;
            this.lbDisplay.Text = "-";
            this.lbDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(88, 162);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(365, 52);
            this.panel3.TabIndex = 71;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 52);
            this.label1.TabIndex = 63;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelTextScan);
            this.panel4.Controls.Add(this.pictureBox2);
            this.panel4.Location = new System.Drawing.Point(88, 100);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(365, 56);
            this.panel4.TabIndex = 72;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // PictureboxQrCodeInput
            // 
            this.PictureboxQrCodeInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureboxQrCodeInput.Image = global::MessageDialog.Properties.Resources.PictureBoxQrCode;
            this.PictureboxQrCodeInput.Location = new System.Drawing.Point(0, 0);
            this.PictureboxQrCodeInput.Name = "PictureboxQrCodeInput";
            this.PictureboxQrCodeInput.Size = new System.Drawing.Size(42, 42);
            this.PictureboxQrCodeInput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureboxQrCodeInput.TabIndex = 55;
            this.PictureboxQrCodeInput.TabStop = false;
            // 
            // PictureboxQrCodeInputCheck
            // 
            this.PictureboxQrCodeInputCheck.Image = global::MessageDialog.Properties.Resources.PictureBoxWrong;
            this.PictureboxQrCodeInputCheck.Location = new System.Drawing.Point(466, 25);
            this.PictureboxQrCodeInputCheck.Name = "PictureboxQrCodeInputCheck";
            this.PictureboxQrCodeInputCheck.Size = new System.Drawing.Size(40, 40);
            this.PictureboxQrCodeInputCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureboxQrCodeInputCheck.TabIndex = 70;
            this.PictureboxQrCodeInputCheck.TabStop = false;
            this.PictureboxQrCodeInputCheck.Click += new System.EventHandler(this.PictureboxQrCodeInputCheck_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MessageDialog.Properties.Resources.PicturBoxRohmLogo;
            this.pictureBox1.Location = new System.Drawing.Point(22, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(77, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 69;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(-70, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(70, 53);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 62;
            this.pictureBox2.TabStop = false;
            // 
            // InputCarrier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 240);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TextBoxQrCodeInput);
            this.Controls.Add(this.lbDisplay);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.PictureboxQrCodeInputCheck);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel4);
            this.Name = "InputCarrier";
            this.Text = "InputCarrier";
            this.Load += new System.EventHandler(this.InputCarrierDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureboxQrCodeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureboxQrCodeInputCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTextScan;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox PictureboxQrCodeInput;
        private System.Windows.Forms.TextBox TextBoxQrCodeInput;
        internal System.Windows.Forms.Label lbDisplay;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox PictureboxQrCodeInputCheck;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Timer timer1;
    }
}