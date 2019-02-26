using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestService.ServiceReference1;
using MessageDialog;
//using Message;
namespace TestService
{
    public partial class Form1 : Form
    {
        private ServiceiLibraryClient c_ILibraryClient;
        public Form1()
        {
            InitializeComponent();
            c_ILibraryClient = new ServiceiLibraryClient();
        }
        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void buttonMcOff_Click(object sender, EventArgs e)
        {
            MachineOnlineStateResult result = c_ILibraryClient.MachineOnlineState(textBoxMCNo.Text, MachineOnline.Offline);
            
        }
        private void buttonMcOn_Click(object sender, EventArgs e)
        {
            //c_ILibraryClient.UpdateMachineState(textBoxMCNo.Text, MachineProcessingState.Execute);
            MachineOnlineStateResult result = c_ILibraryClient.MachineOnlineState(textBoxMCNo.Text, MachineOnline.Online);
            int? aa = 2;
            int? b = null;
            b = 5;
            b = int.Parse((aa ?? 3).ToString());
            b = aa ?? 3;
            b = xxx(aa);
        }
        private int xxx(int? aa)
        {
            return aa ?? 3;
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            SetupLotResult result = c_ILibraryClient.SetupLot(textBoxLotNo.Text, textBoxMCNo.Text,
               textBoxOPNo.Text, textBoxProcess.Text,"");

           
            if (result.IsPass == SetupLotResult.Status.NotPass)
            {
                MessageBoxDialog.ShowMessageDialog(result.FunctionName , result.Cause, result.Type.ToString(), result.ErrorNo);
                return;
            }
            else if(result.IsPass == SetupLotResult.Status.Warning)
            {
                MessageBoxDialog.ShowMessageDialog(result.FunctionName, result.Cause,result.Type.ToString(), result.ErrorNo);
            }
            textBoxRecipe.Text = result.Recipe;

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartLotResult result = c_ILibraryClient.StartLot(textBoxLotNo.Text, textBoxMCNo.Text, 
                textBoxOPNo.Text, textBoxRecipe.Text);
            if (!result.IsPass)
            {
                MessageBox.Show(result.Cause);
            }
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            UpdateFirstinspectionResult result = c_ILibraryClient.UpdateFirstinspection(textBoxLotNo.Text, 
                textBoxOPNo.Text, Judge.OK, textBoxMCNo.Text);
            if (!result.IsPass)
            {
                MessageBox.Show(result.Cause);
            }
        }

        private void buttonFinal_Click(object sender, EventArgs e)
        {
            UpdateFinalinspectionResult result = c_ILibraryClient.UpdateFinalinspection(textBoxLotNo.Text, 
                textBoxOPNo.Text, Judge.OK, textBoxMCNo.Text);
            if (!result.IsPass)
            {
                MessageBox.Show(result.Cause);
            }

        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            EndLotResult result = c_ILibraryClient.EndLot(textBoxLotNo.Text, textBoxMCNo.Text,
                textBoxOPNo.Text, int.Parse(textBoxGood.Text), int.Parse(textBoxNg.Text));
            if (!result.IsPass)
            {
                MessageBox.Show(result.Cause);
            }
        }

        private void buttonReinput_Click(object sender, EventArgs e)
        { 
            ReinputResult result =  c_ILibraryClient.Reinput(textBoxLotNo.Text, textBoxMCNo.Text, textBoxOPNo.Text, int.Parse(textBoxGood.Text), int.Parse(textBoxNg.Text),EndMode.AbnormalEndAccumulate);
            MessageBox.Show(result.IsPass.ToString());
            //c_ILibraryClient.MachineAlarm()
        }

        private void buttonCancelLot_Click(object sender, EventArgs e)
        {
            CancelLotResult result = c_ILibraryClient.CancelLot(textBoxMCNo.Text, textBoxLotNo.Text, textBoxOPNo.Text);
            MessageBox.Show(result.IsPass.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string AlarmNo = "A097";
            if (int.TryParse(AlarmNo, out int alarmNo))
            {
                AlarmNo = alarmNo.ToString();
            }
            else
            {
                AlarmNo = AlarmNo;
            }
        }
    }
}
