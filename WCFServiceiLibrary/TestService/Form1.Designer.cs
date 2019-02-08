namespace TestService
{
    partial class Form1
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
            this.buttonSetup = new System.Windows.Forms.Button();
            this.textBoxLotNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.buttonMcOn = new System.Windows.Forms.Button();
            this.buttonMcOff = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonReinput = new System.Windows.Forms.Button();
            this.buttonCancelLot = new System.Windows.Forms.Button();
            this.textBoxNg = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxGood = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxRecipe = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonFinal = new System.Windows.Forms.Button();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.textBoxProcess = new System.Windows.Forms.TextBox();
            this.textBoxPackage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOPNo = new System.Windows.Forms.TextBox();
            this.textBoxMCNo = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSetup
            // 
            this.buttonSetup.Location = new System.Drawing.Point(207, 47);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonSetup.TabIndex = 1;
            this.buttonSetup.Text = "Setup";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // textBoxLotNo
            // 
            this.textBoxLotNo.Location = new System.Drawing.Point(60, 25);
            this.textBoxLotNo.Name = "textBoxLotNo";
            this.textBoxLotNo.Size = new System.Drawing.Size(128, 20);
            this.textBoxLotNo.TabIndex = 2;
            this.textBoxLotNo.Text = "9999A0004V";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "LotNo";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(207, 79);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.TabStop = false;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonEnd
            // 
            this.buttonEnd.Location = new System.Drawing.Point(207, 170);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(75, 23);
            this.buttonEnd.TabIndex = 5;
            this.buttonEnd.Text = "End";
            this.buttonEnd.UseVisualStyleBackColor = true;
            this.buttonEnd.Click += new System.EventHandler(this.buttonEnd_Click);
            // 
            // buttonMcOn
            // 
            this.buttonMcOn.Location = new System.Drawing.Point(207, 18);
            this.buttonMcOn.Name = "buttonMcOn";
            this.buttonMcOn.Size = new System.Drawing.Size(75, 23);
            this.buttonMcOn.TabIndex = 8;
            this.buttonMcOn.Text = "McOn";
            this.buttonMcOn.UseVisualStyleBackColor = true;
            this.buttonMcOn.Click += new System.EventHandler(this.buttonMcOn_Click);
            // 
            // buttonMcOff
            // 
            this.buttonMcOff.Location = new System.Drawing.Point(207, 199);
            this.buttonMcOff.Name = "buttonMcOff";
            this.buttonMcOff.Size = new System.Drawing.Size(75, 23);
            this.buttonMcOff.TabIndex = 9;
            this.buttonMcOff.Text = "McOff";
            this.buttonMcOff.UseVisualStyleBackColor = true;
            this.buttonMcOff.Click += new System.EventHandler(this.buttonMcOff_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonFirst);
            this.groupBox2.Controls.Add(this.buttonReinput);
            this.groupBox2.Controls.Add(this.buttonCancelLot);
            this.groupBox2.Controls.Add(this.textBoxNg);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBoxGood);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBoxRecipe);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonMcOff);
            this.groupBox2.Controls.Add(this.buttonMcOn);
            this.groupBox2.Controls.Add(this.buttonFinal);
            this.groupBox2.Controls.Add(this.textBoxProcess);
            this.groupBox2.Controls.Add(this.textBoxPackage);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonSetup);
            this.groupBox2.Controls.Add(this.textBoxOPNo);
            this.groupBox2.Controls.Add(this.textBoxMCNo);
            this.groupBox2.Controls.Add(this.textBoxLotNo);
            this.groupBox2.Controls.Add(this.buttonStart);
            this.groupBox2.Controls.Add(this.buttonEnd);
            this.groupBox2.Location = new System.Drawing.Point(28, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 332);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Example";
            // 
            // buttonReinput
            // 
            this.buttonReinput.Location = new System.Drawing.Point(207, 280);
            this.buttonReinput.Name = "buttonReinput";
            this.buttonReinput.Size = new System.Drawing.Size(75, 23);
            this.buttonReinput.TabIndex = 13;
            this.buttonReinput.Text = "Reinput";
            this.buttonReinput.UseVisualStyleBackColor = true;
            this.buttonReinput.Click += new System.EventHandler(this.buttonReinput_Click);
            // 
            // buttonCancelLot
            // 
            this.buttonCancelLot.Location = new System.Drawing.Point(207, 228);
            this.buttonCancelLot.Name = "buttonCancelLot";
            this.buttonCancelLot.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelLot.TabIndex = 12;
            this.buttonCancelLot.Text = "CancelLot";
            this.buttonCancelLot.UseVisualStyleBackColor = true;
            this.buttonCancelLot.Click += new System.EventHandler(this.buttonCancelLot_Click);
            // 
            // textBoxNg
            // 
            this.textBoxNg.Location = new System.Drawing.Point(60, 238);
            this.textBoxNg.Name = "textBoxNg";
            this.textBoxNg.Size = new System.Drawing.Size(128, 20);
            this.textBoxNg.TabIndex = 11;
            this.textBoxNg.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 241);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Ng";
            // 
            // textBoxGood
            // 
            this.textBoxGood.Location = new System.Drawing.Point(60, 206);
            this.textBoxGood.Name = "textBoxGood";
            this.textBoxGood.Size = new System.Drawing.Size(128, 20);
            this.textBoxGood.TabIndex = 11;
            this.textBoxGood.Text = "6000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Good";
            // 
            // textBoxRecipe
            // 
            this.textBoxRecipe.Location = new System.Drawing.Point(60, 174);
            this.textBoxRecipe.Name = "textBoxRecipe";
            this.textBoxRecipe.Size = new System.Drawing.Size(128, 20);
            this.textBoxRecipe.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Recipe";
            // 
            // buttonFinal
            // 
            this.buttonFinal.Location = new System.Drawing.Point(207, 141);
            this.buttonFinal.Name = "buttonFinal";
            this.buttonFinal.Size = new System.Drawing.Size(75, 23);
            this.buttonFinal.TabIndex = 6;
            this.buttonFinal.Text = "Final";
            this.buttonFinal.UseVisualStyleBackColor = true;
            this.buttonFinal.Click += new System.EventHandler(this.buttonFinal_Click);
            // 
            // buttonFirst
            // 
            this.buttonFirst.Location = new System.Drawing.Point(207, 111);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(75, 23);
            this.buttonFirst.TabIndex = 6;
            this.buttonFirst.Text = "First";
            this.buttonFirst.UseVisualStyleBackColor = true;
            this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
            // 
            // textBoxProcess
            // 
            this.textBoxProcess.Location = new System.Drawing.Point(60, 141);
            this.textBoxProcess.Name = "textBoxProcess";
            this.textBoxProcess.Size = new System.Drawing.Size(128, 20);
            this.textBoxProcess.TabIndex = 0;
            this.textBoxProcess.Text = "MP";
            // 
            // textBoxPackage
            // 
            this.textBoxPackage.Location = new System.Drawing.Point(60, 114);
            this.textBoxPackage.Name = "textBoxPackage";
            this.textBoxPackage.Size = new System.Drawing.Size(128, 20);
            this.textBoxPackage.TabIndex = 0;
            this.textBoxPackage.Text = "SSOP-B20W";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Process";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Package";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "OPNo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "MCNo";
            // 
            // textBoxOPNo
            // 
            this.textBoxOPNo.Location = new System.Drawing.Point(60, 83);
            this.textBoxOPNo.Name = "textBoxOPNo";
            this.textBoxOPNo.Size = new System.Drawing.Size(128, 20);
            this.textBoxOPNo.TabIndex = 2;
            this.textBoxOPNo.Text = "007567";
            // 
            // textBoxMCNo
            // 
            this.textBoxMCNo.Location = new System.Drawing.Point(60, 54);
            this.textBoxMCNo.Name = "textBoxMCNo";
            this.textBoxMCNo.Size = new System.Drawing.Size(128, 20);
            this.textBoxMCNo.TabIndex = 2;
            this.textBoxMCNo.Text = "MP-TWE-00";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 368);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.TextBox textBoxLotNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Button buttonMcOn;
        private System.Windows.Forms.Button buttonMcOff;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonFinal;
        private System.Windows.Forms.Button buttonFirst;
        private System.Windows.Forms.TextBox textBoxPackage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOPNo;
        private System.Windows.Forms.TextBox textBoxMCNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxProcess;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRecipe;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxNg;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxGood;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonReinput;
        private System.Windows.Forms.Button buttonCancelLot;
    }
}

