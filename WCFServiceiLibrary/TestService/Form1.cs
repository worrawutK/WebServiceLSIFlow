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
using System.Threading;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
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
            var xxx = InputCarrier.InputQrCode("Tranfer Carrier No.", 11, Color.SpringGreen);
            //CarrierInfo carrierInfo = new CarrierInfo()
            //{
            //    LoadCarrier = CarrierInfo.Status.No_Use,
            //    RegisterCarrierNo = "xx"
                
            //};
            //DataTest dataTest = new DataTest()
            //{
            //    MyProperty = 5
            //};
            //WriteDataToXmlFile(dataTest, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString()) + @"\c_DataCommonList.xml");

        }
        public class DataTest
        {
            public int MyProperty { get; set; }
            public CarrierInfo CarrierInfo { get; set; }
        }
        protected void WriteDataToXmlFile(object data, string xmlPath)
        {
            using (StreamWriter sw = new StreamWriter(xmlPath, false))
            {
                var bs = new XmlSerializer(data.GetType());
                bs.Serialize(sw, data);
            }
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
            CarrierInfo carrierInfo = c_ILibraryClient.GetCarrierInfo(textBoxMCNo.Text, textBoxLotNo.Text,
               textBoxOPNo.Text);
            SetupLotSpecialParametersEventArgs setupLotSpecial = new SetupLotSpecialParametersEventArgs()
            {
                LayerNoApcs = ""
            };
            var sss = c_ILibraryClient.SetupLotNoCheckLicenser(textBoxLotNo.Text, textBoxMCNo.Text,
              textBoxOPNo.Text, textBoxProcess.Text, "");
            SetupLotResult result = c_ILibraryClient.SetupLotPhase2(textBoxLotNo.Text, textBoxMCNo.Text,
              textBoxOPNo.Text, textBoxProcess.Text,Licenser.NoCheck, carrierInfo, setupLotSpecial);


            StartLotSpecialParametersEventArgs startLotSpecial = new StartLotSpecialParametersEventArgs()
            {
                RunModeApcs = RunMode.Normal
            };
            StartLotResult startLotResult = c_ILibraryClient.StartLotPhase2(textBoxLotNo.Text, textBoxMCNo.Text,
              textBoxOPNo.Text, "", carrierInfo, startLotSpecial);


            //EndLotSpecialParametersEventArgs endLotSpecial = new EndLotSpecialParametersEventArgs()
            //{
            //    McNoOvenApcs = ""
            //};
            EndLotResult endLotResult = c_ILibraryClient.EndLotPhase2(textBoxLotNo.Text, textBoxMCNo.Text,
              textBoxOPNo.Text, 1000, 0, Licenser.NoCheck, carrierInfo, null);

            EndLotResult endLotResult2 = c_ILibraryClient.EndLot(textBoxLotNo.Text, textBoxMCNo.Text,
             textBoxOPNo.Text, 1000, 0);
            //      SetupLotResult result = c_ILibraryClient.SetupLotOven(textBoxLotNo.Text, textBoxMCNo.Text, textBoxMCNoOv.Text,
            //textBoxOPNo.Text, textBoxProcess.Text, "");
            //SetupLotResult result = c_ILibraryClient.SetupLotPhase2(textBoxLotNo.Text, textBoxMCNo.Text,
            //  textBoxOPNo.Text, textBoxProcess.Text, "",0,result2,Licenser.NoCheck,RunMode.Normal);
            if (result.IsPass == SetupLotResult.Status.NotPass)
            {
                MessageBoxDialog.ShowMessageDialog(result.FunctionName, result.Cause, result.Type.ToString(), result.ErrorNo);
                return;
            }
            else if (result.IsPass == SetupLotResult.Status.Warning)
            {
                MessageBoxDialog.ShowMessageDialog(result.FunctionName, result.Cause, result.Type.ToString(), result.ErrorNo);
            }
            textBoxRecipe.Text = result.Recipe;

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
           var carrier =  c_ILibraryClient.GetCarrierInfo(textBoxMCNo.Text, textBoxLotNo.Text, textBoxOPNo.Text);
            StartLotResult result = c_ILibraryClient.StartLotPhase2(textBoxLotNo.Text, textBoxMCNo.Text,
               textBoxOPNo.Text, textBoxRecipe.Text, carrier,null);
            //StartLotResult result = c_ILibraryClient.StartLot(textBoxLotNo.Text, textBoxMCNo.Text,
            //    textBoxOPNo.Text, textBoxRecipe.Text);

            //StartLotResult result = c_ILibraryClient.StartLotOven(textBoxLotNo.Text, textBoxMCNo.Text, textBoxMCNoOv.Text,
            //  textBoxOPNo.Text, textBoxRecipe.Text);
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
        public class Lotdata
        {
            public string LotNo { get; set; }
            public string MachineNo { get; set; }
            public string OpNo { get; set; }
        }
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            EndLotResult result = c_ILibraryClient.EndLotOven(textBoxLotNo.Text, textBoxMCNo.Text, textBoxMCNoOv.Text,
             textBoxOPNo.Text, int.Parse(textBoxGood.Text), int.Parse(textBoxNg.Text));
            //EndLotResult result = c_ILibraryClient.EndLot(textBoxLotNo.Text, textBoxMCNo.Text,
            //    textBoxOPNo.Text, int.Parse(textBoxGood.Text), int.Parse(textBoxNg.Text));
            if (!result.IsPass)
            {
                MessageBox.Show(result.Cause);
            }
            if (!string.IsNullOrEmpty(result.NextFlow))
            {
                MessageBox.Show(result.NextFlow);
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
            //string AlarmNo = "A097";
            //if (int.TryParse(AlarmNo, out int alarmNo))
            //{
            //    AlarmNo = alarmNo.ToString();
            //}
            //else
            //{
            //    AlarmNo = AlarmNo;
            //}
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                SetupLotResult result = c_ILibraryClient.SetupLotNoCheckLicenser(textBoxLotNo.Text, textBoxMCNo.Text,
                textBoxOPNo.Text, textBoxProcess.Text, "");

                StartLotResult result2 = c_ILibraryClient.StartLot(textBoxLotNo.Text, textBoxMCNo.Text,
                   textBoxOPNo.Text, textBoxRecipe.Text);
            }
      
        
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Test();
            //backgroundWorker1.RunWorkerAsync();
            //Thread thread = new Thread(new ThreadStart(Test));
            //thread.Start();
            //Thread thread2 = new Thread(new ThreadStart(Test));
            //thread2.Start();
            //Thread thread3 = new Thread(new ThreadStart(Test));
            //thread3.Start();
            //Test();
            //Test();

        }
        private void Test()
        {
            for (int i = 0; i < 5; i++)
            {
                SetupLotResult result = c_ILibraryClient.SetupLotNoCheckLicenser(textBoxLotNo.Text, textBoxMCNo.Text,
               textBoxOPNo.Text, textBoxProcess.Text, "");

                StartLotResult result2 = c_ILibraryClient.StartLot(textBoxLotNo.Text, textBoxMCNo.Text,
                   textBoxOPNo.Text, textBoxRecipe.Text);
            }
               

        }

        private void Button3_Click(object sender, EventArgs e)
        {
          //iReportResponse response =  c_ILibraryClient.IRePortCheck(textBoxMCNo.Text);
          //  if (response.HasError)
          //      MessageBoxDialog.ShowMessageDialog("iReport", response.ErrorMessage, "");
        }
    }
}
