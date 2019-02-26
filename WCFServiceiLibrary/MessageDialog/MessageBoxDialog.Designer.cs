namespace MessageDialog
{
    partial class MessageBoxDialog
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
            this.textBoxErrorMessage = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelAlarmTitle = new System.Windows.Forms.Label();
            this.buttonDismiss = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelErrorNo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMessageType = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxErrorMessage
            // 
            this.textBoxErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBoxErrorMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxErrorMessage.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxErrorMessage.ForeColor = System.Drawing.Color.Maroon;
            this.textBoxErrorMessage.Location = new System.Drawing.Point(108, 45);
            this.textBoxErrorMessage.Multiline = true;
            this.textBoxErrorMessage.Name = "textBoxErrorMessage";
            this.textBoxErrorMessage.ReadOnly = true;
            this.textBoxErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxErrorMessage.Size = new System.Drawing.Size(387, 119);
            this.textBoxErrorMessage.TabIndex = 8;
            this.textBoxErrorMessage.Text = "-";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelAlarmTitle);
            this.panel2.Location = new System.Drawing.Point(0, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(565, 70);
            this.panel2.TabIndex = 0;
            // 
            // labelAlarmTitle
            // 
            this.labelAlarmTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelAlarmTitle.AutoSize = true;
            this.labelAlarmTitle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlarmTitle.ForeColor = System.Drawing.Color.White;
            this.labelAlarmTitle.Location = new System.Drawing.Point(14, 19);
            this.labelAlarmTitle.Name = "labelAlarmTitle";
            this.labelAlarmTitle.Size = new System.Drawing.Size(285, 29);
            this.labelAlarmTitle.TabIndex = 0;
            this.labelAlarmTitle.Text = "ALARM INFORMATION";
            // 
            // buttonDismiss
            // 
            this.buttonDismiss.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDismiss.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDismiss.Location = new System.Drawing.Point(450, 232);
            this.buttonDismiss.Name = "buttonDismiss";
            this.buttonDismiss.Size = new System.Drawing.Size(75, 34);
            this.buttonDismiss.TabIndex = 0;
            this.buttonDismiss.Text = "&Dismiss";
            this.buttonDismiss.UseVisualStyleBackColor = true;
            this.buttonDismiss.Click += new System.EventHandler(this.ButtonDismiss_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.labelMessageType);
            this.panel3.Controls.Add(this.buttonDismiss);
            this.panel3.Location = new System.Drawing.Point(12, 68);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(541, 279);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.textBoxErrorMessage);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.labelErrorNo);
            this.panel4.Location = new System.Drawing.Point(11, 37);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(514, 178);
            this.panel4.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(13, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Error No :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Message :";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(13, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(485, 1);
            this.label1.TabIndex = 5;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelErrorNo
            // 
            this.labelErrorNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelErrorNo.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorNo.ForeColor = System.Drawing.Color.Maroon;
            this.labelErrorNo.Location = new System.Drawing.Point(108, 9);
            this.labelErrorNo.Name = "labelErrorNo";
            this.labelErrorNo.Size = new System.Drawing.Size(383, 27);
            this.labelErrorNo.TabIndex = 4;
            this.labelErrorNo.Text = "-";
            this.labelErrorNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Menu;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(565, 347);
            this.panel1.TabIndex = 2;
            // 
            // labelMessageType
            // 
            this.labelMessageType.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessageType.ForeColor = System.Drawing.Color.Black;
            this.labelMessageType.Location = new System.Drawing.Point(14, 3);
            this.labelMessageType.Name = "labelMessageType";
            this.labelMessageType.Size = new System.Drawing.Size(150, 31);
            this.labelMessageType.TabIndex = 1;
            // 
            // MessageBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 347);
            this.Controls.Add(this.panel1);
            this.Name = "MessageBoxDialog";
            this.Text = "MessageBoxDialog";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxErrorMessage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelAlarmTitle;
        private System.Windows.Forms.Button buttonDismiss;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelErrorNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelMessageType;
    }
}