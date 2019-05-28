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
    public partial class MessageBoxDialog : Form
    {
        public MessageBoxDialog()
        {
            InitializeComponent();
        }
        public string MessageType
        {
            get
            {
                return labelMessageType.Text;
            }
            set
            {
                labelMessageType.Text = value;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return textBoxErrorMessage.Text;
            }
            set
            {
                textBoxErrorMessage.Text = value;
            }
        }
        public string ErrorNo
        {
            get
            {
                return labelErrorNo.Text;
            }
            set
            {
                labelErrorNo.Text = value;
            }
        }
        public string Title
        {
            get
            {
                return labelAlarmTitle.Text;
            }
            set
            {
                labelAlarmTitle.Text = value;
            }
        }

        private void ButtonDismiss_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// ใช้ในกรณีที่มี ErrorNo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="errorNo"></param>
        public static DialogResult ShowMessageDialog(string title, string message, string messageType, string errorNo = "00"
            , int? x = null, int? y = null)
        {
            using (MessageBoxDialog frm = new MessageBoxDialog())
            {
                frm.Title = title;
                frm.ErrorMessage = message;
                frm.MessageType = messageType;
                frm.TopMost = true;
                if (x.HasValue && y.HasValue)
                    frm.Location = new Point(x.Value, y.Value);
                else
                    frm.StartPosition = FormStartPosition.CenterScreen;
                
                if (errorNo == "00" || errorNo == "")
                    frm.ErrorNo = "-";
                else
                    frm.ErrorNo = errorNo;
               
                return frm.ShowDialog();
            }
        }

        public static void ShowMessage(string title, string message, string messageType, string errorNo = "00")
        {
            MessageBoxDialog frm = new MessageBoxDialog();
            frm.Title = title;
            frm.ErrorMessage = message;
            frm.MessageType = messageType;
            if (errorNo == "00" || errorNo == "")
                frm.ErrorNo = "-";
            else
                frm.ErrorNo = errorNo;
            frm.Show();
            frm.TopMost = true;
            frm.StartPosition = FormStartPosition.CenterScreen;
        }
        /// <summary>
        /// ใช้ในกรณีที่ไม่มี ErrorNo
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="mcNo"></param>
        /// <param name="lotNo"></param>
        /// <param name="message"></param>
        public static void ShowMessageDialog(string eventName, string mcNo, string lotNo, string message,string messageType ,int? x = null,int? y =null)
        {
            ShowMessageDialog(eventName, "MCNo : " + mcNo + ", LotNo : " + lotNo + " | " + message, messageType,"00",x.Value,y.Value);
        }
    }
}
