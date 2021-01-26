using iLibrary;
//using Rohm.Apcs.Tdc;
using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web;
//using TDCService;
using IReport;
using LotInfo = iLibrary.LotInfo;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceiLibrary" in code, svc and config file together.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServiceiLibrary : IServiceiLibrary
{
    private ApcsProService c_ApcsProService;
    //private TdcService c_TdcService;
    public int count;
    private string c_PahtLogFile;
    private const string c_LogVersion = "1.0.0";
    private const string c_PathFolderBackupTdc = "backupTDC";
    private const string c_FileTdcBackupName = "TDC_before.csv";
    private const string c_ApcsProDisable = "TRUE";
    private const string c_ApplicationName = "CellController";
    public ServiceiLibrary()
    {
        c_ApcsProService = new ApcsProService();
        c_ApcsProService.Set_ApplicationName("WCF_Service_iLibrary");
        AppSettingHelper.Set_ApplicationName("WCF_Service");
        //c_TdcService =  TdcService.GetInstance();
        //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
        string testMode =  AppSettingHelper.GetAppSettingsValue("TestMode");
        if (testMode.Trim().ToUpper() == "TRUE")
        {
            c_PahtLogFile = HttpContext.Current.Server.MapPath(@"~\\Log");
        }
        else
        {
            c_PahtLogFile = @"\\172.16.0.115\NewCenterPoint\iLibraryService\Log";//HttpContext.Current.Server.MapPath(@"~F:\bin\WCFLog");
        }
    }
    #region Setup
    
    public SetupLotResult SetupLot(string lotNo, string mcNo, string opNo, string processName, string layerNo)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "", "",
        //      "", MethodBase.GetCurrentMethod().Name, log);
        SetupLotEventArgs setupEventArgs = new SetupLotEventArgs()
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Process = processName,
            LayerNo = layerNo,
            RunMode = RunMode.Normal,
            FunctionName = MethodBase.GetCurrentMethod().Name,
            IsCheckLicenser = Licenser.NoCheck,
            IsOnline = 1
        };
        return SetupLotCommon(setupEventArgs);
    }
    
    public SetupLotResult SetupLotNoCheckLicenser(string lotNo, string mcNo, string opNo, string processName, string layerNo)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "", "",
        //      "", MethodBase.GetCurrentMethod().Name, log);
        SetupLotEventArgs setupEventArgs = new SetupLotEventArgs()
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Process = processName,
            LayerNo = layerNo,
            RunMode = RunMode.Normal,
            IsCheckLicenser = Licenser.NoCheck,
            FunctionName = MethodBase.GetCurrentMethod().Name,
            IsOnline = 1
        };
        return SetupLotCommon(setupEventArgs);
    }

    public SetupLotResult SetupLotCustomMode(string lotNo, string mcNo, string opNo, string processName, string layerNo, RunMode runMode)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "", "",
        //      "", MethodBase.GetCurrentMethod().Name, log);
        SetupLotEventArgs setupEventArgs = new SetupLotEventArgs()
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Process = processName,
            LayerNo = layerNo,
            RunMode = runMode,
            FunctionName = MethodBase.GetCurrentMethod().Name,
            IsCheckLicenser = Licenser.Check,
            IsOnline = 1
        };
        return SetupLotCommon(setupEventArgs);
        //return SetupLotCommon(lotNo, mcNo, opNo, processName, layerNo, runMode, MethodBase.GetCurrentMethod().Name);
    }
    public SetupLotResult SetupLotCustomModeNoCheckLicenser(string lotNo, string mcNo, string opNo, string processName, string layerNo, RunMode runMode)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "","",
        //      "", MethodBase.GetCurrentMethod().Name, log);
        SetupLotEventArgs setupEventArgs = new SetupLotEventArgs()
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Process = processName,
            LayerNo = layerNo,
            RunMode = runMode,
            FunctionName = MethodBase.GetCurrentMethod().Name,
            IsCheckLicenser = Licenser.NoCheck,
            IsOnline = 1
        };
        return SetupLotCommon(setupEventArgs);
    }
     
    //Oven พี่อาร์ม
    public SetupLotResult SetupLotOven(string lotNo, string mcNoApcsPro, string mcNoApcs, string opNo, string processName, string layerNo)
    {
        //Logger log = new Logger(c_LogVersion, mcNoApcsPro, c_PahtLogFile);
        //return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNoApcsPro, "", "",
        //      "", MethodBase.GetCurrentMethod().Name, log);
        SetupLotEventArgs setupEventArgs = new SetupLotEventArgs()
        {
            LotNo = lotNo,
            MachineNo = mcNoApcsPro,
            OperatorNo = opNo,
            Process = processName,
            LayerNo = layerNo,
            RunMode = RunMode.Normal,
            FunctionName = MethodBase.GetCurrentMethod().Name,
            IsCheckLicenser = Licenser.NoCheck,
            MachineOven = mcNoApcs,
            IsOnline = 1
        };
        return SetupLotCommon(setupEventArgs);

    }

    public SetupLotResult SetupLotPhase2(string lotNo, string mcNo, string opNo, string processName,Licenser licenser, CarrierInfo carrierInfo,
        SetupLotSpecialParametersEventArgs specialParametersEventArgs) 
    {
        string layerNo = "";
        RunMode runMode = RunMode.Normal;
        int frameIn = 0;
        string mcNoOven = "";
        int isOnline = 1;
        if (specialParametersEventArgs != null)
        {
            if (!string.IsNullOrEmpty(specialParametersEventArgs.LayerNoApcs)) layerNo = specialParametersEventArgs.LayerNoApcs;
            runMode = specialParametersEventArgs.RunModeApcs;
            frameIn = specialParametersEventArgs.FrameIn;
            if (!string.IsNullOrEmpty(specialParametersEventArgs.McNoOvenApcs)) mcNoOven = specialParametersEventArgs.McNoOvenApcs;

            if (specialParametersEventArgs.IsOffline)
                isOnline = 0;
            else
                isOnline = 1;
        }
        SetupLotEventArgs setupLotEventArgs = new SetupLotEventArgs()
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Process = processName,
            LayerNo = layerNo,
            FunctionName = MethodBase.GetCurrentMethod().Name,
            IsCheckLicenser = licenser,
            RunMode = runMode,
            FrameIn = frameIn,
            CarrierInfo = carrierInfo,
            MachineOven = mcNoOven,
            IsOnline = isOnline
        };
        return  SetupLotCommon(setupLotEventArgs);
    }

    private SetupLotResult SetupLotCommon(SetupLotEventArgs setupEventArgs)
    {
        string mcNoToApcs;
        if (!string.IsNullOrEmpty(setupEventArgs.MachineOven))
        {
            mcNoToApcs = setupEventArgs.MachineOven;
        }
        else
        {
            mcNoToApcs = setupEventArgs.MachineNo;
        }
        Logger log = new Logger(c_LogVersion, setupEventArgs.MachineNo, c_PahtLogFile);
        try
        {
            DateTime dateTime = DateTime.Now;
            LotInfo lotInfo = null;
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
            {
               goto apcsProDisa;
            }
            dateTime = c_ApcsProService.Get_DateTimeInfo(log).Datetime;
            lotInfo = c_ApcsProService.GetLotInfo(setupEventArgs.LotNo, log, dateTime);
        apcsProDisa:
            if (lotInfo == null || IsSkipped(setupEventArgs.MachineNo))
            {
                if (lotInfo == null)
                {
                    string packageName = GetPackageName(setupEventArgs.LotNo);
                    if (string.IsNullOrEmpty(packageName))
                    {
                        return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Unknown, "lotNo:" + setupEventArgs.LotNo + " ไม่พบข้อมูลในระบบ",
                          "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo, "", "",
                          "CheckLotApcsPro", setupEventArgs.FunctionName, log);
                    }
                    CheckLotApcsProResult checkLot = CheckLotApcsPro(setupEventArgs.LotNo, packageName, log);
                    if (checkLot.IsPass)
                    {
                        return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, "lotNo:" + setupEventArgs.LotNo + " รอการ Import date เข้าระบบ APCS Pro",
                            "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo, "", "",
                            "CheckLotApcsPro", setupEventArgs.FunctionName, log);
                    }
                }
                //รอการแก้บัค เนื่องจากคิดว่าทุก lot จะมีใน db ของ ApcsPro แต่ไม่ใช่จะมีแค่ device slip ที่ลงทะเบียนแล้ว ถึงจะนำ lot เข้ามาในระบบ Apcs Pro (SetUp Start End)
                //TdcLotRequestResult requestResult = TdcLotRequest(mcNoToApcs, setupEventArgs.LotNo, (RunModeType)setupEventArgs.RunMode, log);
                //if (!requestResult.IsPass)
                //{
                //    return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Apcs, requestResult.Cause,
                //              "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo, "", requestResult.ErrorNo,
                //              "CheckLotApcsPro", setupEventArgs.FunctionName, log);
                //}
                return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Apcs, "lotInfo is null", "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo, "", "",
                    "TdcLotRequest", setupEventArgs.FunctionName, log);
            
            }
            //Check frame and setDefult
            if (setupEventArgs.FrameIn == 0)
            {
                setupEventArgs.FrameIn = lotInfo.FrameIn;
            }
            //Check package and Lot  
            CheckLotApcsProResult proResult = CheckLotApcsPro(setupEventArgs.LotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                //TDC priority
                //TdcLotRequestResult requestResult = TdcLotRequest(mcNoToApcs, setupEventArgs.LotNo, (RunModeType)setupEventArgs.RunMode, log);
                //if (!requestResult.IsPass)
                //{
                //    return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Apcs, requestResult.Cause + 
                //        Environment.NewLine + "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo + " package:" + lotInfo.Package.Name,
                //              "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo + " package:" + lotInfo.Package.Name, "", requestResult.ErrorNo,
                //              "CheckLotApcsPro", setupEventArgs.FunctionName, log);
                //}
                return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Apcs, proResult.Cause, "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo, "", "",
                    "CheckLotApcsPro", setupEventArgs.FunctionName, log);
            }
            else
            {

                UserInfo userInfo = c_ApcsProService.GetUserInfo(setupEventArgs.OperatorNo, log, dateTime, 30);
                if (userInfo == null)
                {
                    return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + setupEventArgs.OperatorNo,
                        "","","","GetUserInfo",MethodBase.GetCurrentMethod().Name,log);
                }
                string warningMessage = "";
                if (userInfo.License != null)
                {
                    //foreach (License item in userInfo.License)
                    //{
                    //    if (item.Is_Expired)
                    //    {

                    //    }else if (item.Is_Warning)
                    //    {

                    //    }
                    //}
                    if (userInfo.License[0].Is_Expired)
                    {
                        warningMessage = "แจ้งเตือน!! รหัส :" + userInfo.Code + Environment.NewLine + "License " + userInfo.License[0].Name +
                            Environment.NewLine + " หมดอายุ กรุณาต่ออายุ License ที่ ETG " + Environment.NewLine + "วันหมดอายุ " +
                            userInfo.License[0].ExpireDate.ToString("yyyy/MM/dd");
                    }
                    else if (userInfo.License[0].Is_Warning)
                    {
                        warningMessage = "แจ้งเตือน!! รหัส :" + userInfo.Code + Environment.NewLine + "License " + userInfo.License[0].Name +
                            Environment.NewLine + " ใกล้หมดอายุ กรุณาต่ออายุ License ที่ ETG " + Environment.NewLine + "วันหมดอายุ " +
                            userInfo.License[0].ExpireDate.ToString("yyyy/MM/dd");
                    }
                }
                if (setupEventArgs.IsOnline == 1)
                {
                    //Check Permission
                    var checkPermissionResult = OnCheckPermissionApplication(setupEventArgs.MachineNo, userInfo, c_ApplicationName, setupEventArgs.Process + "-SetupLot", dateTime);
                    if (!checkPermissionResult.IsPass)
                    {
                        return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, checkPermissionResult.Reason,
                                             "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo + " processName:" + setupEventArgs.Process, "", checkPermissionResult.ErrorNo,
                                             "CheckUserPermission", setupEventArgs.FunctionName, log);
                    }
                    //CheckUserPermissionResult permissionResult = c_ApcsProService.CheckUserPermission(userInfo, "CellController",
                    //    setupEventArgs.Process + "-SetupLot", log, dateTime);
                    //if (!permissionResult.IsPass)
                    //    return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, permissionResult.ErrorMessage,
                    //        "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo + " processName:" + setupEventArgs.Process, "", permissionResult.ErrorNo.ToString(), 
                    //        "CheckUserPermission", setupEventArgs.FunctionName, log);

                    if (setupEventArgs.IsCheckLicenser == Licenser.Check)
                    {
                        ResultBase resultByLMS = OnCheckPermissionMachineByLMS(setupEventArgs.MachineNo, userInfo, log);
                        if (!resultByLMS.IsPass)
                        {
                            return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, resultByLMS.Reason, "", "", "",
                                 "Check_PermissionMachinesByLMS", setupEventArgs.FunctionName, log);
                        }
                        ResultBase resultLotAutoMotive = OnCheckPermissionUserLotAutoMotive(userInfo, lotInfo, log);
                        if (!resultLotAutoMotive.IsPass)
                        {
                            return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, resultLotAutoMotive.Reason, "", "", "",
                                 "Check_UserLotAutoMotive", setupEventArgs.FunctionName, log);
                        }
                    }
                }
               

                MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(setupEventArgs.MachineNo, log, dateTime);
                if (machineInfo == null)
                    return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, "ไม่พบ MCNo :" + setupEventArgs.MachineNo + " ในระบบ", 
                        "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo + " mcNo" + setupEventArgs.MachineNo, "","", "GetMachineInfo",
                        setupEventArgs.FunctionName, log);

                //Carrier Control
                string loadCarrierNo = "";
                string registerCarrierNo = "";
                string transferCarrierNo = "";
                if (setupEventArgs.CarrierInfo != null && setupEventArgs.CarrierInfo.EnabledControlCarrier == CarrierInfo.CarrierStatus.Use &&
                    setupEventArgs.CarrierInfo.InControlCarrier == CarrierInfo.CarrierStatus.Use)
                {
                    loadCarrierNo = setupEventArgs.CarrierInfo.LoadCarrierNo.Trim().ToUpper();
                    registerCarrierNo = setupEventArgs.CarrierInfo.RegisterCarrierNo.Trim().ToUpper();
                    transferCarrierNo = setupEventArgs.CarrierInfo.TransferCarrierNo.Trim().ToUpper();
                    if (setupEventArgs.CarrierInfo.LoadCarrier == CarrierInfo.CarrierStatus.Use)
                    {//setupEventArgs.CarrierInfo.LoadCarrierNo
                        ResultBase resultBase = SetAutoCarrier(lotInfo.Name, loadCarrierNo, machineInfo.Name, CarrierStatue.Load,log);
                        if (!resultBase.IsPass)
                        {
                            return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, resultBase.Reason,
                              "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo + " mcNo:" + setupEventArgs.MachineNo + " LoadCarrierNo:" + loadCarrierNo, "",
                              "0","AutoRegisterCarrier", setupEventArgs.FunctionName, log);
                        }
                        CarrierControlResult resultLoad = c_ApcsProService.VerificationLoadCarrier(machineInfo.Id, lotInfo.Id, loadCarrierNo, userInfo.Id, log);
                        if (!resultLoad.IsPass)
                        {
                            return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, resultLoad.ErrorMessageDetail.Error_Message,
                                "mcNo:" + setupEventArgs.MachineNo + " ,lotNo:" + setupEventArgs.LotNo +
                                " ,LoadCarrierNo:" + loadCarrierNo + " ,opNo:" + setupEventArgs.OperatorNo
                                , "", resultLoad.ErrorMessageDetail.Error_No.ToString(), "VerificationLoadCarrier", setupEventArgs.FunctionName, log);
                        }
                    }

                    if (setupEventArgs.CarrierInfo.RegisterCarrier == CarrierInfo.CarrierStatus.Use)
                    {
                        CarrierControlResult resultRegister = c_ApcsProService.CheckAndRegisterCurrentCarrier(machineInfo.Id, lotInfo.Id, registerCarrierNo, userInfo.Id, log);
                        if (!resultRegister.IsPass)
                        {
                            return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, resultRegister.ErrorMessageDetail.Error_Message,
                                "mcNo:" + setupEventArgs.MachineNo + " ,lotNo:" + setupEventArgs.LotNo +
                                " ,RegisterCarrierNo:" + registerCarrierNo + " ,opNo:" + setupEventArgs.OperatorNo
                                , "", resultRegister.ErrorMessageDetail.Error_No.ToString(), "CheckAndRegisterCurrentCarrier", setupEventArgs.FunctionName, log);
                        }
                    }

                    if (setupEventArgs.CarrierInfo.TransferCarrier == CarrierInfo.CarrierStatus.Use)
                    {
                        CarrierControlResult resultTransfer = c_ApcsProService.CheckAndRegisterNextCarrier(machineInfo.Id, lotInfo.Id,
                            transferCarrierNo, userInfo.Id, log);
                        if (!resultTransfer.IsPass)
                        {
                            return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro,resultTransfer.ErrorMessageDetail.Error_Message,
                                     "mcNo:" + setupEventArgs.MachineNo + " ,lotNo:" + setupEventArgs.LotNo +
                                " ,TransferCarrierNo:" + transferCarrierNo + " ,opNo:" + setupEventArgs.OperatorNo
                                , "", resultTransfer.ErrorMessageDetail.Error_No.ToString(), "CheckAndRegisterNextCarrier", setupEventArgs.FunctionName, log);
                        }
                    }
                }


                LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotSetup(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", setupEventArgs.IsOnline, dateTime, log, setupEventArgs.FrameIn);
                if (!lotUpdateInfo.IsOk)
                {
                    string jobName;
                    if (lotInfo.IsSpecialFlow)
                    {
                        jobName = lotInfo.SpJob.Name;
                    }
                    else
                    {
                        jobName = lotInfo.Job.Name;
                    }
                     return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, lotUpdateInfo.ErrorMessage + Environment.NewLine
                        + "MCNo:" + machineInfo.Name + Environment.NewLine
                        + "LotNo:" + lotInfo.Name + " || OPID:" + userInfo.Code + Environment.NewLine
                        + "เป็นงานของ:" + jobName, "IsOnline:" + setupEventArgs.IsOnline.ToString()
                       , "", lotUpdateInfo.ErrorNo.ToString(), "LotSetup",
                        setupEventArgs.FunctionName, log);
                }
                   
                //TDC Move
                //TdcMove(mcNoToApcs, setupEventArgs.LotNo, setupEventArgs.OperatorNo, setupEventArgs.LayerNo, log);

                if (warningMessage != "")
                {
                    return new SetupLotResult(SetupLotResult.Status.Warning,MessageType.ApcsPro, warningMessage, 
                        "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo + " IsOnline:" + setupEventArgs.IsOnline.ToString(), lotUpdateInfo.Recipe1, "","GetUserInfo",
                        setupEventArgs.FunctionName, log, lotInfo.Quantity.Pass, lotInfo.Quantity.LastFail);
                }
                else
                {
                    return new SetupLotResult(SetupLotResult.Status.Pass,MessageType.ApcsPro, "", "LotNo:" + setupEventArgs.LotNo + 
                        " opNo:" + setupEventArgs.OperatorNo + " loadCarrierNo:" + loadCarrierNo + " registerCarrierNo:" + registerCarrierNo + 
                        " transferCarrierNo:" + transferCarrierNo + " IsOnline:" + setupEventArgs.IsOnline.ToString(), lotUpdateInfo.Recipe1, "", "",
                        setupEventArgs.FunctionName, log, lotInfo.Quantity.Pass, lotInfo.Quantity.LastFail);
                }
            }
           
        }
        catch (Exception ex)
        {
            return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.Unknown, ex.Message.ToString(), "LotNo:" + setupEventArgs.LotNo + " opNo:" + setupEventArgs.OperatorNo, "", "", "Exception",
                setupEventArgs.FunctionName, log);
        }
    }

    #endregion

    #region Start
    public StartLotResult StartLot(string lotNo, string mcNo, string opNo, string recipe)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new StartLotResult(false, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
        //      "", MethodBase.GetCurrentMethod().Name, log);
       // return StartLotCommon(lotNo, mcNo, opNo, recipe, MethodBase.GetCurrentMethod().Name, RunMode.Normal);
        StartLotEventArgs startLotEventArgs = new StartLotEventArgs()
        {
            LotNo = lotNo,
            OpNo = opNo,
            Recipe = recipe,
            MachineNo = mcNo,
            LoadCarrierNo = "",
            TransfersCarrierNo = "",
            IsOnline = 1,
            FunctionName = MethodBase.GetCurrentMethod().Name
        };
        return StartLotCommon(startLotEventArgs);
    }
    public StartLotResult StartLotCustomMode(string lotNo, string mcNo, string opNo, string recipe,RunMode runMode)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new StartLotResult(false, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
        //      "", MethodBase.GetCurrentMethod().Name, log);
        //return StartLotCommon(lotNo, mcNo, opNo, recipe, MethodBase.GetCurrentMethod().Name, runMode);
        StartLotEventArgs startLotEventArgs = new StartLotEventArgs()
        {
            LotNo = lotNo,
            OpNo = opNo,
            Recipe = recipe,
            MachineNo = mcNo,
            LoadCarrierNo = "",
            TransfersCarrierNo = "",
            IsOnline = 1,
            FunctionName = MethodBase.GetCurrentMethod().Name
        };
        return StartLotCommon(startLotEventArgs);
    }
    public StartLotResult StartLotOven(string lotNo, string mcNoApcsPro ,string mcNoApcs, string opNo, string recipe)
    {
        //Logger log = new Logger(c_LogVersion, mcNoApcsPro, c_PahtLogFile);
        //return new StartLotResult(false, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNoApcsPro,
        //      "", MethodBase.GetCurrentMethod().Name, log);
        StartLotEventArgs startLotEventArgs = new StartLotEventArgs()
        {
            LotNo = lotNo,
            OpNo = opNo,
            Recipe = recipe,
            MachineNo = mcNoApcsPro,
            LoadCarrierNo = "",
            TransfersCarrierNo = "",
            IsOnline = 1,
            FunctionName = MethodBase.GetCurrentMethod().Name
        };
        return StartLotCommon(startLotEventArgs);
    }
    public  StartLotResult StartLotPhase2(string lotNo,string mcNo,string opNo,string recipe,CarrierInfo carrierInfo,
        StartLotSpecialParametersEventArgs specialParametersEventArgs)
    {
        //RunMode runMode = RunMode.Normal;
        //string mcNoOven = "";
        //int locationNum = 1;
        //int actPassQty = -1;
        string loadCarrierNo = "";
        string transferCarrierNo = "";
        int isOnline = 1;
        if (specialParametersEventArgs != null)
        {
            //if (!string.IsNullOrEmpty(specialParametersEventArgs.McNoOvenApcs)) mcNoOven = specialParametersEventArgs.McNoOvenApcs;
            //runMode = specialParametersEventArgs.RunModeApcs;
            if (specialParametersEventArgs.IsOffline)
                isOnline = 0;
            else
                isOnline = 1;
        }
        if (carrierInfo != null)
        {
            if (!string.IsNullOrEmpty(carrierInfo.RegisterCarrierNo))
            {
                loadCarrierNo = carrierInfo.RegisterCarrierNo;
            }
            else
            {
                loadCarrierNo = carrierInfo.LoadCarrierNo;
            }
            if (carrierInfo.TransferCarrierNo != carrierInfo.LoadCarrierNo)
            {
                transferCarrierNo = carrierInfo.TransferCarrierNo;
            }
        }
        StartLotEventArgs startLotEventArgs = new StartLotEventArgs()
        {
            LotNo = lotNo,
            OpNo = opNo,
            Recipe = recipe,
            MachineNo = mcNo,
            LoadCarrierNo = loadCarrierNo,
            TransfersCarrierNo = transferCarrierNo,
            IsOnline = isOnline,
            FunctionName = MethodBase.GetCurrentMethod().Name
        };
        return StartLotCommon(startLotEventArgs);
    }
    //private StartLotResult StartLotCommon(string lotNo, string mcNo, string opNo, string recipe,string functionName,RunMode runMode,string mcNoApcs = "",int locationNum = 1,int actPassQty = -1,string loadCarrierNo = "",string transferCarrierNo = "",int isOnline = 1)
    //{
    //    Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
    //    try
    //    {
    //        string mcNoToApcs;
    //        if (!string.IsNullOrEmpty(mcNoApcs))
    //        {
    //            mcNoToApcs = mcNoApcs;
    //        }
    //        else
    //        {
    //            mcNoToApcs = mcNo;
    //        }
    //        //  DateTimeInfo dateTimeInfo = c_,ApcsProService.Get_DateTimeInfo(log);
    //        DateTime dateTime = DateTime.Now;
    //        LotInfo lotInfo = null;
    //        //var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
    //        //if (apcsProDisable == c_ApcsProDisable)
    //        //{
    //        //    //TDC LotSet
    //        //    TdcLotSetResult tdcLotSetResult = TdcLotSet(mcNoToApcs, lotNo, opNo, (RunModeType)runMode, dateTime, log);
    //        //    if (!tdcLotSetResult.IsPass)
    //        //    {
    //        //        //
    //        //        // TODO: Add constructor logic here
    //        //        //
    //        //    }
    //        //    return new StartLotResult(true, MessageType.Apcs, "apcsProDisable:" + apcsProDisable, "LotNo:" + lotNo + " opNo:" + opNo,
    //        //        "apcsProDisable", functionName, log);
    //        //}

    //        dateTime = c_ApcsProService.Get_DateTimeInfo(log).Datetime;       
    //        lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTime);
    //        if (lotInfo == null)
    //        {
    //            ////TDC LotSet
    //            //TdcLotSetResult tdcLotSetResult = TdcLotSet(mcNoToApcs, lotNo, opNo, (RunModeType)runMode, dateTime, log);
    //            //if (!tdcLotSetResult.IsPass)
    //            //{
    //            //    //
    //            //    // TODO: Add constructor logic here
    //            //    //
    //            //}
    //            return new StartLotResult(false, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
    //          "GetLotInfo", functionName, log);
    //        }

              
    //        //Check package and Lot Pro
    //        CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
    //        if (!proResult.IsPass)
    //        {
    //            ////TDC LotSet
    //            //TdcLotSetResult tdcLotSetResult = TdcLotSet(mcNoToApcs, lotNo, opNo, (RunModeType)runMode, dateTime, log);
    //            //if (!tdcLotSetResult.IsPass)
    //            //{
    //            //    //
    //            //    // TODO: Add constructor logic here
    //            //    //
    //            //}
    //            return new StartLotResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
    //                "CheckLotApcsPro", functionName, log);
    //        }
    //        else
    //        {
    //            if (IsSkipped(mcNo))
    //            {
    //                //TdcLotSet(mcNoToApcs, lotNo, opNo, (RunModeType)runMode, dateTime, log);
    //                return new StartLotResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", functionName, log);
    //            }
    //            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTime, 30);
    //            if (userInfo == null)
    //            {
    //                return new StartLotResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
    //                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
    //            }
    //            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTime);
    //            if (machineInfo == null)
    //                return new StartLotResult(false,MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
    //                    "GetMachineInfo", functionName, log);

    //            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotStart(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", isOnline, recipe, log,
    //                locationNum, actPassQty,loadCarrierNo,transferCarrierNo);
    //            if (!lotUpdateInfo.IsOk)
    //                return new StartLotResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo +
    //                    " mcNo:" + mcNo + " locationNum:" + locationNum.ToString() + " actPassQty:" + actPassQty.ToString() + " loadCarrierNo:" + loadCarrierNo
    //                    + " unloadCarrierNo:" + transferCarrierNo, "LotStart", functionName, log);

    //            //TdcLotSet(mcNoToApcs, lotNo, opNo, (RunModeType)runMode, dateTime, log);
    //        }
           
    //        return new StartLotResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo + " loadCarrierNo:" + loadCarrierNo + 
    //            " transferCarrierNo:" + transferCarrierNo, "", functionName, log);
    //    }
    //    catch (Exception ex)
    //    {
    //        return new StartLotResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
    //            "Exception", functionName, log);
    //    }
    //}
    private StartLotResult StartLotCommon(StartLotEventArgs startLotEventArgs)
    {
        Logger log = new Logger(c_LogVersion, startLotEventArgs.MachineNo, c_PahtLogFile);
        string mcNo = startLotEventArgs.MachineNo;
        DateTime dateTime = DateTime.Now;
        LotInfo lotInfo = null;
        string lotNo = startLotEventArgs.LotNo;
        string functionName = startLotEventArgs.FunctionName;
        string opNo = startLotEventArgs.OpNo;
        string loadCarrierNo = startLotEventArgs.LoadCarrierNo;
        string transferCarrierNo = startLotEventArgs.TransfersCarrierNo;
        try
        {
            dateTime = c_ApcsProService.Get_DateTimeInfo(log).Datetime;
            lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTime);
           
            if (lotInfo == null)
            {
                return new StartLotResult(false, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
              "GetLotInfo", functionName, log);
            }


            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new StartLotResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
                    "CheckLotApcsPro", functionName, log);
            }
            else
            {
                if (IsSkipped(mcNo))
                {
                    //TdcLotSet(mcNoToApcs, lotNo, opNo, (RunModeType)runMode, dateTime, log);
                    return new StartLotResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", functionName, log);
                }
                UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTime, 30);
                if (userInfo == null)
                {
                    return new StartLotResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                        "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
                }
                MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTime);
                if (machineInfo == null)
                    return new StartLotResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                        "GetMachineInfo", functionName, log);

                LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotStart(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", startLotEventArgs.IsOnline, startLotEventArgs.Recipe, log,
                    1, -1, loadCarrierNo, transferCarrierNo);
                if (!lotUpdateInfo.IsOk)
                    return new StartLotResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo +
                        " mcNo:" + mcNo + " loadCarrierNo:" + loadCarrierNo
                        + " unloadCarrierNo:" + transferCarrierNo, "LotStart", functionName, log);

            }

            return new StartLotResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo + " loadCarrierNo:" + loadCarrierNo +
                " transferCarrierNo:" + transferCarrierNo, "", functionName, log);
        }
        catch (Exception ex)
        {
            return new StartLotResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                "Exception", functionName, log);
        }
    }

    public OnlineStartResult OnlineStart(string lotNo, string mcNo, string opNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new OnlineStartResult(true, MessageType.Apcs, "Only support Apcs Pro","LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new OnlineStartResult(false, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo,
                    "GetLotInfo", MethodBase.GetCurrentMethod().Name, log);

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new OnlineStartResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
                    "CheckLotApcsPro", MethodBase.GetCurrentMethod().Name, log);
            }
            if (IsSkipped(mcNo))
            {
                return new OnlineStartResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", MethodBase.GetCurrentMethod().Name, log);
            }
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new OnlineStartResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new OnlineStartResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo" + mcNo,
                    "GetMachineInfo", MethodBase.GetCurrentMethod().Name, log);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.OnlineStart(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new OnlineStartResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo +
                    " mcNo:" + mcNo, "OnlineStart", MethodBase.GetCurrentMethod().Name, log);

            return new OnlineStartResult(true, MessageType.ApcsPro, "LotNo:" + lotNo + " opNo:" + opNo, "", "", MethodBase.GetCurrentMethod().Name, log);
        }
        catch (Exception ex)
        {
            return new OnlineStartResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "Exception",
                MethodBase.GetCurrentMethod().Name, log);
        }
    }

  
    public UpdateFirstinspectionResult UpdateFirstinspection(string lotNo, string opNo, Judge judge, string mcNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new UpdateFirstinspectionResult(true, MessageType.Apcs, "Only support Apcs Pro", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new UpdateFirstinspectionResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                    "GetLotInfo", MethodBase.GetCurrentMethod().Name, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new UpdateFirstinspectionResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
                    "CheckLotApcsPro", MethodBase.GetCurrentMethod().Name, log);
            }
            if (IsSkipped(mcNo))
            {
                return new UpdateFirstinspectionResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", MethodBase.GetCurrentMethod().Name, log);
            }
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new UpdateFirstinspectionResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.Update_Firstinspection(lotInfo.Id, (int)judge, userInfo.Id, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new UpdateFirstinspectionResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo +
                    " opNo:" + opNo + " mcNo:" + mcNo, "Update_Firstinspection", MethodBase.GetCurrentMethod().Name, log);

            return new UpdateFirstinspectionResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);
        }
        catch (Exception ex)
        {
            return new UpdateFirstinspectionResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
              "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
    }
    #endregion
    
    #region End
    public OnlineEndResult OnlineEnd(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new OnlineEndResult(true, MessageType.Apcs, "Only support Apcs Pro", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new OnlineEndResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo,
                    "GetLotInfo", MethodBase.GetCurrentMethod().Name, log);

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new OnlineEndResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
                    "CheckLotApcsPro", MethodBase.GetCurrentMethod().Name, log);
            }
            if (IsSkipped(mcNo))
            {
                return new OnlineEndResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", MethodBase.GetCurrentMethod().Name, log);
            }
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new OnlineEndResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new OnlineEndResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo" + mcNo,
                    "GetMachineInfo", MethodBase.GetCurrentMethod().Name, log);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.OnlineEnd(lotInfo.Id, machineInfo.Id, userInfo.Id, false, good, ng, 0, "",
                1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new OnlineEndResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo +
                    " mcNo:" + mcNo + " good:" + good + " ng:" + ng, "OnlineEnd", MethodBase.GetCurrentMethod().Name, log);

            return new OnlineEndResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);

        }
        catch (Exception ex)
        {
            return new OnlineEndResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good +
                " ng:" + ng, "Exception", MethodBase.GetCurrentMethod().Name, log);

        }
    }

    public UpdateFinalinspectionResult UpdateFinalinspection(string lotNo, string opNo, Judge judge, string mcNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new UpdateFinalinspectionResult(true, MessageType.Apcs, "Only support Apcs Pro", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new UpdateFinalinspectionResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                    "GetLotInfo", MethodBase.GetCurrentMethod().Name, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new UpdateFinalinspectionResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
                    "CheckLotApcsPro", MethodBase.GetCurrentMethod().Name, log);
            }
            if (IsSkipped(mcNo))
            {
                return new UpdateFinalinspectionResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", MethodBase.GetCurrentMethod().Name, log);
            }
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new UpdateFinalinspectionResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.Update_Finalinspection(lotInfo.Id, (int)judge, userInfo.Id, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new UpdateFinalinspectionResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo +
                    " opNo:" + opNo + " mcNo:" + mcNo, "Update_Finalinspection", MethodBase.GetCurrentMethod().Name, log);

            return new UpdateFinalinspectionResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);
        }
        catch (Exception ex)
        {
            return new UpdateFinalinspectionResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
    }


    public EndLotResult EndLot(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new EndLotResult(false, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
        //      "", MethodBase.GetCurrentMethod().Name, log);
        EndLotEvenArgs endLotEvenArgs = new EndLotEvenArgs(MethodBase.GetCurrentMethod().Name, Licenser.NoCheck)
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Good = good,
            Ng = ng,
            IsOnline = 1
        };
        return EndLotCommon(endLotEvenArgs);
    }
    public EndLotResult EndLotNoCheckLicenser(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        //Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        //return new EndLotResult(false, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
        //      "", MethodBase.GetCurrentMethod().Name, log);
        EndLotEvenArgs endLotEvenArgs = new EndLotEvenArgs(MethodBase.GetCurrentMethod().Name, Licenser.NoCheck)
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Good = good,
            Ng = ng,
            IsOnline = 1
        };
        return EndLotCommon(endLotEvenArgs);
    }
    public EndLotResult EndLotOven(string lotNo, string mcNoApcs, string mcNoApcsPro, string opNo, int good, int ng)
    {
        //Logger log = new Logger(c_LogVersion, mcNoApcsPro, c_PahtLogFile);
        //return new EndLotResult(false, MessageType.ApcsPro, "Function " + MethodBase.GetCurrentMethod().Name + " นี้ถูกปิดใช้งานแล้ว กรุณาติดต่อ System", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNoApcsPro,
        //      "", MethodBase.GetCurrentMethod().Name, log);
        EndLotEvenArgs endLotEvenArgs = new EndLotEvenArgs(MethodBase.GetCurrentMethod().Name, Licenser.NoCheck)
        {
            LotNo = lotNo,
            MachineNo = mcNoApcs,
            MachineOven = mcNoApcsPro,
            OperatorNo = opNo,
            Good = good,
            Ng = ng,
            IsOnline = 1
        };
        return EndLotCommon(endLotEvenArgs);

    }

    public EndLotResult EndLotPhase2(string lotNo,string mcNo,string opNo,int good,int ng, Licenser licenser ,CarrierInfo carrierInfo,
        EndLotSpecialParametersEventArgs specialParametersEventArgs)
    {
        string mcNoOven = "";
        int isOnline = 1;
        if (specialParametersEventArgs != null)
        {
            if(!string.IsNullOrEmpty(specialParametersEventArgs.McNoOvenApcs)) mcNoOven = specialParametersEventArgs.McNoOvenApcs;

            if (specialParametersEventArgs.IsOffline)
                isOnline = 0;
            else
                isOnline = 1;
                    
        }
        EndLotEvenArgs endLotEvenArgs = new EndLotEvenArgs(MethodBase.GetCurrentMethod().Name, licenser)
        {
            LotNo = lotNo,
            MachineNo = mcNo,
            OperatorNo = opNo,
            Good = good,
            Ng = ng,
            CarrierInfo = carrierInfo,
            MachineOven = mcNoOven,
            IsOnline = isOnline
        };
        return EndLotCommon(endLotEvenArgs);
    }
    //public EndLotResult EndLotCustomMode(string lotNo, string mcNo, string opNo, int good, int ng ,EndMode endMode)
    //{
    //    return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, endMode, false);
    //}
    //public EndLotResult EndLotCostomModeNoCheckLicenser(string lotNo, string mcNo, string opNo, int good, int ng , EndMode endMode)
    //{
    //    return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, endMode, false);
    //}
    //  private EndLotResult EndLotCommon(string lotNo, string mcNo, string opNo, int good, int ng,string functionName,  bool isCheckLicenser = true,string mcNoApcs = "")
    private EndLotResult EndLotCommon(EndLotEvenArgs endLotEvenArgs)
    {
        string mcNo = endLotEvenArgs.MachineNo;
        string lotNo = endLotEvenArgs.LotNo;
        string opNo = endLotEvenArgs.OperatorNo;
        int good = endLotEvenArgs.Good;
        int ng = endLotEvenArgs.Ng;
        string functionName = endLotEvenArgs.FunctionName;
        string mcNoApcs = endLotEvenArgs.MachineOven;
        string unloadCarrierNo = "";
        if (endLotEvenArgs.CarrierInfo.TransferCarrier == CarrierInfo.CarrierStatus.Use_OnLotEnd)
            endLotEvenArgs.CarrierInfo.UnloadCarrierNo = endLotEvenArgs.CarrierInfo.TransferCarrierNo;
        if (!string.IsNullOrEmpty(endLotEvenArgs.CarrierInfo.UnloadCarrierNo))
            unloadCarrierNo = endLotEvenArgs.CarrierInfo.UnloadCarrierNo;
       
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            string mcNoToApcs;
            if (!string.IsNullOrEmpty(mcNoApcs))
            {
                mcNoToApcs = mcNoApcs;
            }
            else
            {
                mcNoToApcs = mcNo;
            }
            DateTime dateTime = DateTime.Now;
            LotInfo lotInfo = null;
           
            //EndModeType.AbnormalEndAccumulate รันใหม่ทั้งหมด
            //EndModeType.AbnormalEndReset รันต่อที่เหลือ
            //EndModeType.Normal จบ Lot ปกติ
            //TdcLotEndResult tdcLotEndResult = TdcLotEnd(mcNoToApcs, lotNo, opNo, dateTime, log, good, ng, EndModeType.Normal);
            //if (!tdcLotEndResult.IsPass)
            //{
            //    //
            //    // TODO: Add constructor logic here
            //    //
            //}
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new EndLotResult(true, MessageType.Apcs, "", "LotNo:" + lotNo + " opNo:" + opNo, "", functionName, log, "");

            dateTime = c_ApcsProService.Get_DateTimeInfo(log).Datetime;
            lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTime);
            if (lotInfo == null)
            {
                //return new EndLotResult(false, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                //   " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
                return new EndLotResult(true, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                   " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
            }
            int frame_Pass = 0;
            if (endLotEvenArgs.Frame_Pass == 0)
            {
                frame_Pass = lotInfo.FramePass;
            }
            else
            {
                frame_Pass = endLotEvenArgs.Frame_Pass;
            }

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new EndLotResult(true, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng,
                   "CheckLotApcsPro", functionName, log);
            }
            if (IsSkipped(mcNo))
            {
                return new EndLotResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", functionName, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTime, 30);
            if (userInfo == null)
            {
                return new EndLotResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTime);
            if (machineInfo == null)
                return new EndLotResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                    " good:" + good + " ng:" + ng, "GetMachineInfo", functionName, log);
             
            if (endLotEvenArgs.IsCheckLicenser == Licenser.Check && endLotEvenArgs.IsOnline == 1)
            {
                ResultBase resultByLMS = OnCheckPermissionMachineByLMS(mcNo, userInfo, log);
                if (!resultByLMS.IsPass)
                {
                    return new EndLotResult(false, MessageType.ApcsPro, "Check_PermissionMachinesByLMS", resultByLMS.Reason, "Check_PermissionMachinesByLMS",
                         functionName, log);
                }
                ResultBase resultLotAutoMotive = OnCheckPermissionUserLotAutoMotive(userInfo,lotInfo, log);
                if (!resultLotAutoMotive.IsPass)
                {
                    return new EndLotResult(false, MessageType.ApcsPro, "Check_PermissionMachinesByLMS", resultLotAutoMotive.Reason,
                         "Check_UserLotAutoMotive", functionName, log);
                }
            }

            //Carrier Control
            if (endLotEvenArgs.CarrierInfo != null && endLotEvenArgs.CarrierInfo.EnabledControlCarrier == CarrierInfo.CarrierStatus.Use && endLotEvenArgs.CarrierInfo.InControlCarrier == CarrierInfo.CarrierStatus.Use)
            {
                if (endLotEvenArgs.CarrierInfo.UnloadCarrier == CarrierInfo.CarrierStatus.Use)
                {
                    ResultBase resultBase = SetAutoCarrier(lotNo, endLotEvenArgs.CarrierInfo.UnloadCarrierNo, mcNo, CarrierStatue.Unload, log);
                    if (!resultBase.IsPass)
                    {
                        return new EndLotResult(false, MessageType.ApcsPro, resultBase.Reason,
                          "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng + " UnloadCarrierNo:" + endLotEvenArgs.CarrierInfo.UnloadCarrierNo,
                          "AutoRegisterCarrier", functionName, log);
                    }
                    if (endLotEvenArgs.CarrierInfo.TransferCarrier != CarrierInfo.CarrierStatus.No_Use)
                    {
                        CarrierControlResult carrierControlResult = c_ApcsProService.VerificationUnloadCarrier(machineInfo.Id, lotInfo.Id, endLotEvenArgs.CarrierInfo.UnloadCarrierNo, userInfo.Id, log);
                        if (!carrierControlResult.IsPass)
                        {
                            return new EndLotResult(false, MessageType.ApcsPro, carrierControlResult.ErrorMessageDetail.Error_Message,
                                 "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng + " UnloadCarrierNo:" + endLotEvenArgs.CarrierInfo.UnloadCarrierNo, "VerificationUnloadCarrier", functionName, log);
                        }
                    }
                        
                }
                if (endLotEvenArgs.CarrierInfo.TransferCarrier == CarrierInfo.CarrierStatus.Use_OnLotEnd)
                {
                    CarrierControlResult carrierControlResult = c_ApcsProService.CheckAndRegisterNextCarrier(machineInfo.Id, lotInfo.Id, endLotEvenArgs.CarrierInfo.UnloadCarrierNo, userInfo.Id, log);
                    if (!carrierControlResult.IsPass)
                    {
                        return new EndLotResult(false, MessageType.ApcsPro, carrierControlResult.ErrorMessageDetail.Error_Message,
                             "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng + " UnloadCarrierNo:" + endLotEvenArgs.CarrierInfo.UnloadCarrierNo, "CheckAndRegisterNextCarrier", functionName, log);
                    }
                }
            }

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotEnd(lotInfo.Id, machineInfo.Id, userInfo.Id, false, good, ng, 0, "", endLotEvenArgs.IsOnline, 
                dateTime, log, unloadCarrierNo, frame_Pass, endLotEvenArgs.Frame_Fail);
            if (!lotUpdateInfo.IsOk)
                return new EndLotResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo +
                    " mcNo:" + mcNo + " good:" + good + " ng:" + ng + " unloadCarrierNo:" + unloadCarrierNo + " IsOnline:" + endLotEvenArgs.IsOnline.ToString(), "LotEnd", functionName, log);

            string nextFlow = "";
            if (lotUpdateInfo.NextProcess != null)
            {
                //if (lotUpdateInfo.NextProcess.ReserveInfo.SFReserve_On || lotUpdateInfo.NextProcess.ReserveInfo.SFSFReserve_On)
                //{
                //    // MessageBoxDialog.ShowMessageDialog("SPECIAL FLOW", lotUpdateInfo.NextProcess.SpComments.SpComment + "\r\n" + endInfo.NextProcess.SpComments.CommentForStart);
                //}
                //else if (lotUpdateInfo.NextProcess.ReserveInfo.Is_EndStep)
                //{
                //    // MessageBoxDialog.ShowMessageDialog("SPECIAL FLOW", lotUpdateInfo.NextProcess.SpComments.SpComment + "\r\n" + endInfo.NextProcess.SpComments.CommentForEnd);
                //}
                //else
                //{
                  
                //    //MessageBoxDialog.ShowMessageDialog("NEXT PROCESS", endInfo.NextProcess.Process_name);
                //}
                nextFlow = "Next Process :" + lotUpdateInfo.NextProcess.Job_name;
            }
            return new EndLotResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo + " unloadCarrierNo:" + unloadCarrierNo + " IsOnline:" + endLotEvenArgs.IsOnline.ToString(), "", functionName, log, nextFlow);
        }
        catch (Exception ex)
        {
            return new EndLotResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng,
                "Exception", functionName, log);
        }

    }


    #endregion
    #region ReloadAndCancel

    public CancelLotResult CancelLot(string mcNo, string lotNo, string opNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new CancelLotResult(false, MessageType.Apcs, "Only support Apcs Pro", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new CancelLotResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                    "GetLotInfo", MethodBase.GetCurrentMethod().Name, log);

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new CancelLotResult(false,MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                    "CheckLotApcsPro", MethodBase.GetCurrentMethod().Name, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new CancelLotResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new CancelLotResult(false,MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                    "GetMachineInfo", MethodBase.GetCurrentMethod().Name, log);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotCancel(lotInfo.Id, machineInfo.Id, userInfo.Id, 1, log);
            if (!lotUpdateInfo.IsOk)
            {
                return new CancelLotResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                    "LotCancel", MethodBase.GetCurrentMethod().Name, log);
            }
            return new CancelLotResult(true,MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);
        }
        catch (Exception ex)
        {
            return new CancelLotResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                "Exception", MethodBase.GetCurrentMethod().Name, log);
        }

    }
    /// <summary>
    /// Ng = 0 is Reload
    /// </summary>
    /// <param name="lotNo"></param>
    /// <param name="mcNo"></param>
    /// <param name="opNo"></param>
    /// <param name="good"></param>
    /// <param name="ng"> NG=0 is Reload </param>
    /// <returns></returns>
    public ReinputResult ReinputAndHoldLot(string lotNo, string mcNo, string opNo, int good, int ng, EndMode endMode)
    {
        return AbnormalLotEnd_BackToThe_BeforeProcess(lotNo, mcNo, opNo, good, ng, true, MethodBase.GetCurrentMethod().Name, endMode);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lotNo"></param>
    /// <param name="mcNo"></param>
    /// <param name="opNo"></param>
    /// <param name="good"></param>
    /// <param name="ng"> NG=0 is Reload </param>
    /// <returns></returns>
    public ReinputResult Reinput(string lotNo, string mcNo, string opNo, int good, int ng,EndMode endMode)
    {
        return AbnormalLotEnd_BackToThe_BeforeProcess(lotNo, mcNo, opNo, good, ng, false, MethodBase.GetCurrentMethod().Name, endMode);
    }
    public ReinputResult ReinputOven(string lotNo, string mcNoApcsPro,string mcNoApcs, string opNo, int good, int ng, EndMode endMode)
    {
        return AbnormalLotEnd_BackToThe_BeforeProcess(lotNo, mcNoApcsPro, opNo, good, ng, false, MethodBase.GetCurrentMethod().Name, endMode, mcNoApcs);
    }
    private ReinputResult AbnormalLotEnd_BackToThe_BeforeProcess(string lotNo, string mcNo, string opNo, int good, int ng, bool holdLot, string functionName ,EndMode endMode,string mcNoApcs = "")
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            string mcNoToApcs;
            if (mcNoApcs != "")
            {
                mcNoToApcs = mcNoApcs;
            }
            else
            {
                mcNoToApcs = mcNo;
            }
            if (endMode == EndMode.AbnormalEndReset)
            {
                ng = 0;
            }
            DateTime dateTime = DateTime.Now;
            LotInfo lotInfo = null;
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
            {
                goto apcsProDisa;
            }
            dateTime = c_ApcsProService.Get_DateTimeInfo(log).Datetime;
            lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTime);
        apcsProDisa:

            if (lotInfo == null)
            {
                //TdcLotEndResult tdcLotEndResult = TdcLotEnd(mcNoToApcs, lotNo, opNo, dateTime, log, good, ng, (EndModeType)endMode);
                //if (!tdcLotEndResult.IsPass)
                //{
                //    //
                //    // TODO: Add constructor logic here
                //    //
                //}
                return new ReinputResult(false, MessageType.Apcs, "ไม่พบ lotInfo", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "TdcLotEnd",
                    functionName, log);
            }
                //return new ReinputResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                //    " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                //TdcLotEndResult tdcLotEndResult = TdcLotEnd(mcNoToApcs, lotNo, opNo, dateTime, log, good, ng, (EndModeType)endMode);
                //if (!tdcLotEndResult.IsPass)
                //{
                //    //
                //    // TODO: Add constructor logic here
                //    //
                //}
                return new ReinputResult(false, MessageType.Apcs, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "TdcLotEnd",
                    functionName, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTime, 30);
            if (userInfo == null)
            {
                return new ReinputResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTime);
            if (machineInfo == null)
                return new ReinputResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                    " good:" + good + " ng:" + ng, "GetMachineInfo", functionName, log);
            LotUpdateInfo lotUpdateInfo = c_ApcsProService.AbnormalLotEnd_BackToThe_BeforeProcess(lotInfo.Id, machineInfo.Id, userInfo.Id, holdLot,
                good, ng, 0, "", 1, dateTime, log);

            if (!lotUpdateInfo.IsOk)
            {
                return new ReinputResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                    " good:" + good + " ng:" + ng, "AbnormalLotEnd_BackToThe_BeforeProcess", functionName, log);
            }
            return new ReinputResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", functionName, log);
        }
        catch (Exception ex)
        {
            return new ReinputResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good +
                " ng:" + ng, "Exception", functionName, log);
        }
    }
    #endregion

    public LotInformation GetLotInfo(string lotNo, string mcNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo);
        try
        {
            DateTime dateTime = c_ApcsProService.Get_DateTimeInfo(log).Datetime;
            var lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTime);
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCF", "iLibrary", 0, "GetLotInfo", "", "LotNo[" + lotNo + "],MCNo[" + mcNo + "]");
            if (lotInfo == null)
            {
                return null;
            }

            LotInformation lotInformation = new LotInformation();

            if (c_ApcsProService.CheckPackageEnable(lotInfo.Package.Name, log))
                lotInformation.LotType = LotInformation.LotTypeState.ApcsPro;
            else
            {
                lotInformation.LotType = LotInformation.LotTypeState.Apcs;
            }
            
            lotInformation.LotId = lotInfo.Id;
            lotInformation.LotNo = lotInfo.Name;
            lotInformation.DeviceName = lotInfo.Device.Name;
            lotInformation.PackageName = lotInfo.Package.Name;
            lotInformation.PassQty = lotInfo.Quantity.Pass;
            lotInformation.FailQty = lotInfo.Quantity.Fail;
            string jobName = "";
            if (lotInfo.IsSpecialFlow)
            {
                jobName = lotInfo.SpJob.Name;
            }
            else
            {
                jobName = lotInfo.Job.Name;
            }

            lotInformation.JobName = jobName;
            //if (c_ApcsProService.CheckPackageEnable(lotInfo.Package.Name, log))
            //    lotInformation.LotType = LotInformation.LotTypeState.ApcsPro;
            //else
            //{
            //    lotInformation.LotType = LotInformation.LotTypeState.Apcs;
            //    return null;
            //}
                
            return lotInformation;
        }
        catch (Exception ex)
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCF", "iLibrary", 0, "GetLotInfo", "Exception:" + ex.Message.ToString(), "LotNo[" + lotNo + "],MCNo[" + mcNo + "]");
            return null;
        }

    }

    public MachineOnlineStateResult MachineOnlineState(string mcNo, MachineOnline online)
    {
        int countCheck = 0;
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
    refunction:
        try
        {
       
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new MachineOnlineStateResult(true, MessageType.Apcs, "Only support Apcs Pro", online.ToString(), "", MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new MachineOnlineStateResult(false,MessageType.ApcsPro,"ไม่พบ MCNo :" + mcNo + " ในระบบ","",
                    "GetMachineInfo", MethodBase.GetCurrentMethod().Name, log);

            int result = c_ApcsProService.Update_MachineOnlineState(machineInfo.Id, (int)online, -1, log);
            if (result == 0)
            {
                return new MachineOnlineStateResult(false, MessageType.ApcsPro, "Update_MachineOnlineState อัพเดทไม่ผ่าน","",
                   "Update_MachineOnlineState", MethodBase.GetCurrentMethod().Name, log);
            }

            return new MachineOnlineStateResult(true, MessageType.ApcsPro, "", online.ToString(),"", MethodBase.GetCurrentMethod().Name,log);
        }
        catch (SqlException ex)  // -2 is a sql timeout
        {
            if (ex.Number == -2)
            {
                if (countCheck <= 3)
                {
                    goto refunction;
                }
            }
            return new MachineOnlineStateResult(false, MessageType.ApcsPro, ex.Message.ToString(), online.ToString(), "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
        catch  (Exception ex)
        {
            return new MachineOnlineStateResult(false, MessageType.ApcsPro, ex.Message.ToString(), online.ToString(), "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
   
       // return new MachineOnlineStateResult();
    }
    public UpdateMachineStateResult UpdateMachineState(string mcNo, MachineProcessingState state)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new UpdateMachineStateResult(true, MessageType.Apcs, "Only support Apcs Pro", state.ToString(), "", MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new UpdateMachineStateResult(false,MessageType.ApcsPro,"ไม่พบ MCNo :" + mcNo + " ในระบบ","", "GetMachineInfo",
                    MethodBase.GetCurrentMethod().Name, log);

            int result = c_ApcsProService.Update_MachineState(machineInfo.Id,(int)state, -1, log);
            if (result ==0)
                return new UpdateMachineStateResult(false, MessageType.ApcsPro, "Update Machine State ไม่ได้","", "Update_MachineState", 
                    MethodBase.GetCurrentMethod().Name, log);

            return new UpdateMachineStateResult(true, MessageType.ApcsPro, "", state.ToString(), "", MethodBase.GetCurrentMethod().Name,log);
        }
        catch (Exception ex)
        {
            return new UpdateMachineStateResult(false, MessageType.ApcsPro, ex.Message.ToString(),"", "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
    }
 
    public MachineAlarmResult MachineAlarm(string lotNo, string mcNo, string opNo ,string AlarmNo, AlarmState alarm)
    {
        Logger log = new Logger();
        try
        {
            log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
            //Only support Apcs Pro
            var apcsProDisable = AppSettingHelper.GetAppSettingsValue("ApcsProDisable").ToUpper();
            if (apcsProDisable == c_ApcsProDisable)
                return new MachineAlarmResult(true, MessageType.Apcs, "Only support Apcs Pro", "AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(), "",
                 MethodBase.GetCurrentMethod().Name, log);

            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new MachineAlarmResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new MachineAlarmResult(false,MessageType.ApcsPro,"ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo
                    + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(), "GetLotInfo",
                    MethodBase.GetCurrentMethod().Name, log);

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new MachineAlarmResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo 
                    + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(), "GetMachineInfo",
                    MethodBase.GetCurrentMethod().Name, log);

            //ตัด 0### ออก
            int alarmNo;
            if (int.TryParse(AlarmNo,out alarmNo))
            {
                AlarmNo = alarmNo.ToString();
            }

            int[] lotArray = new int[1];
            lotArray[0] = lotInfo.Id;
            if (alarm == AlarmState.SET)
            {
                MachineUpdateInfo machineAlarmSet = c_ApcsProService.Update_ErrorHappenRecord(lotArray, machineInfo, userInfo.Id, AlarmNo,
               dateTimeInfo.Datetime, log);
                if (!machineAlarmSet.IsOk)
                    return new MachineAlarmResult(false, MessageType.ApcsPro, "Update_ErrorHappenRecord not Complete",
                        "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(),
                        "Update_ErrorHappenRecord", MethodBase.GetCurrentMethod().Name, log);
            }
            else
            {
                try
                {
                    MachineUpdateInfo machineAlarmReset = c_ApcsProService.Update_ErrorResetRecord(machineInfo, userInfo.Id, AlarmNo, dateTimeInfo.Datetime, log);
                    if (!machineAlarmReset.IsOk)
                        return new MachineAlarmResult(false, MessageType.ApcsPro, "Update_ErrorResetRecord not Complete",
                            "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(),
                            "Update_ErrorResetRecord", MethodBase.GetCurrentMethod().Name, log);
                }
                catch (Exception ex)
                {
                    LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, alarm.ToString(), "WCFService", "iLibrary",
                        "Update_ErrorResetRecord", ex.Message.ToString(), "Exception");
                }
             
            }

            return new MachineAlarmResult(true, MessageType.ApcsPro, "", "AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(), "",
                MethodBase.GetCurrentMethod().Name,log);
        }
        catch (Exception ex)
        {
            return new MachineAlarmResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + 
                " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(), "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
    }
   
   
    #region Check

    public CheckLotApcsProResult CheckLotApcsProManual(string lotNo, string mcNo, string package)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            string fullName = GetPackageFullName(package);
            if (!string.IsNullOrEmpty(fullName))
            {
                package = fullName.Trim();
            }
            CheckLotApcsProResult apcsProResult = CheckLotApcsPro(lotNo, package, log);
            if (!apcsProResult.IsPass)
            {
                LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "iLibrary",
                        "CheckLotApcsPro", apcsProResult.Cause, "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package);
            }
            else
            {
                LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "iLibrary",
                       "CheckLotApcsPro", apcsProResult.Cause, "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package);
            }
            return apcsProResult;
        }
        catch (Exception ex)
        {
            LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "iLibrary",
                     "Exception", ex.ToString(), "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package);
            return new CheckLotApcsProResult(false, "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package +
                " Exception:" + ex.ToString(), "", MessageType.ApcsPro, "", "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
    }
    private CheckLotApcsProResult CheckLotApcsPro(string lotNo, string package, Logger log)
    {
        //Check package and Lot Pro
        if (!c_ApcsProService.CheckPackageEnable(package, log))
        {
            return new CheckLotApcsProResult(false, "Package :" + package + " นี้ยังไม่เปิดใช้งานในระบบ Apcs Pro", "", MessageType.ApcsPro, "",
                "CheckPackageEnable", MethodBase.GetCurrentMethod().Name, log);
        }
        if (!c_ApcsProService.CheckLotisExist(lotNo, log))
        {
            return new CheckLotApcsProResult(false, "ไม่พบ LotNo :" + lotNo + " ในระบบ Apcs Pro", "", MessageType.ApcsPro, "",
                "CheckLotisExist", MethodBase.GetCurrentMethod().Name, log);
        }
        return new CheckLotApcsProResult(true, "", "", MessageType.ApcsPro, "", "", MethodBase.GetCurrentMethod().Name, log);
    }
    private bool IsSkipped(string mcNo)
    {
        string McNoSkipped = AppSettingHelper.GetAppSettingsValue("McNoSkipped");
        List<string> McList = McNoSkipped.Split(',').ToList();

        if (McList.Where(x => x == mcNo).Any())
        {
            return true;
        }
        return false;
    }
    #region CheckPermission
    private ResultBase OnCheckPermissionApplication(string machineNo,UserInfo userInfo,string applicationName,string functionName, DateTime dateTime)
    {
        ResultBase resultBase = new ResultBase();
        try
        {
            resultBase.IsPass = true;
            Logger log = new Logger(c_LogVersion, machineNo, c_PahtLogFile);
            CheckUserPermissionResult permissionResult = c_ApcsProService.CheckUserPermission(userInfo, applicationName,
                      functionName, log, dateTime);
            if (!permissionResult.IsPass)
            {
                resultBase.IsPass = false;
                resultBase.Reason = permissionResult.ErrorMessage;
                resultBase.ErrorNo = permissionResult.ErrorNo.ToString();
            }
        }
        catch (Exception ex)
        {
            resultBase.IsPass = false;
            resultBase.Reason = MethodBase.GetCurrentMethod().Name + "=> Exception:" + ex.Message.ToString();
        }
        return resultBase;

    }
    private ResultBase OnCheckPermissionMachineByLMS(string machineNo, UserInfo userInfo, Logger log)
    {
        ResultBase resultBase = new ResultBase();
        try
        {
            resultBase.IsPass = true;
          //  Logger log = new Logger(c_LogVersion, machineNo, c_PahtLogFile);
            if (!c_ApcsProService.Check_PermissionMachinesByLMS(userInfo.Id, machineNo, log))
            {
                resultBase.IsPass = false;
                resultBase.Reason = "รหัส : " + userInfo.Code + " ไม่ผ่านการตรวจสอบในระบบ Licenser กรุณาติดต่อ ETG (MCNo : " + machineNo + ")";
            }
        }
        catch (Exception ex)
        {
            resultBase.IsPass = false;
            resultBase.Reason = MethodBase.GetCurrentMethod().Name + "=> Exception:" + ex.Message.ToString();
        }
        return resultBase;

    }
    private ResultBase OnCheckPermissionUserLotAutoMotive(UserInfo userInfo, LotInfo lotInfo,Logger log)
    {
        ResultBase resultBase = new ResultBase();
        try
        {
            resultBase.IsPass = true;
           // Logger log = new Logger(c_LogVersion, machineNo, c_PahtLogFile);
            if (!c_ApcsProService.Check_UserLotAutoMotive(userInfo, lotInfo, log))
            {
                resultBase.IsPass = false;
                resultBase.Reason = "รหัส : " + userInfo.Code + " User ที่ไม่ใช่ Automotive ไม่สามารถรัน Lot Automotive ได้ กรุณาติดต่อ ETG (Lot Automotive : " + lotInfo.Name + ")";
            }
        }
        catch (Exception ex)
        {
            resultBase.IsPass = false;
            resultBase.Reason = MethodBase.GetCurrentMethod().Name + "=> Exception:" + ex.Message.ToString();
        }
        return resultBase;

    }
    public ResultBase CheckPermissionApplication(string machineNo, string opNo, string applicationName, string functionName)
    {
        ResultBase resultBase = new ResultBase();
        try
        {
            DateTime date = DateTime.Now;
            Logger log = new Logger(c_LogVersion, machineNo, c_PahtLogFile);
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo,log, date);

            resultBase = OnCheckPermissionApplication(machineNo,userInfo, applicationName,functionName, date);
        }
        catch (Exception ex)
        {
            resultBase.IsPass = false;
            resultBase.Reason = MethodBase.GetCurrentMethod().Name + "=> Exception:" + ex.Message.ToString();
        }
        return resultBase;

    }
    public ResultBase CheckPermissionMachineByLMS(string machineNo, string opNo)
    {
        ResultBase resultBase = new ResultBase();
        try
        {
            DateTime date = DateTime.Now;
            Logger log = new Logger(c_LogVersion, machineNo, c_PahtLogFile);
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, date);
            resultBase = OnCheckPermissionMachineByLMS(machineNo, userInfo, log);
        }
        catch (Exception ex)
        {
            resultBase.IsPass = false;
            resultBase.Reason = MethodBase.GetCurrentMethod().Name + "=> Exception:" + ex.Message.ToString();
        }
        return resultBase;
    }
    public ResultBase CheckPermissionUserLotAutoMotive(string opNo, string lotNo,string machineNo)
    {
        ResultBase resultBase = new ResultBase();
        try
        {
            DateTime date = DateTime.Now;
            Logger log = new Logger(c_LogVersion, machineNo, c_PahtLogFile);
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, date);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, date);
            resultBase = OnCheckPermissionUserLotAutoMotive(userInfo, lotInfo, log);
        }
        catch (Exception ex)
        {
            resultBase.IsPass = false;
            resultBase.Reason = MethodBase.GetCurrentMethod().Name + "=> Exception:" + ex.Message.ToString();
        }
        return resultBase;

    }
    #endregion
    #endregion


    #region TDC
    //private void TdcMove(string mcNo, string lotNo, string opNo, string layerNo, Logger log)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(layerNo))
    //        {
    //            LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "TDC",
    //                   "NoMove", "", "lotNo:" + lotNo + " opNo:" + opNo + " LayerNo:" + layerNo);
    //            return;
    //        }
    //        //Init TCD Library    
    //        c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
    //        TdcLotRequestResponse res = c_TdcService.LotRequest(mcNo, lotNo, RunModeType.Normal);
    //        //log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "TDC", 0, "LotRequest", "",
    //        //    "lotNo:" + lotNo + " opNo:" + opNo + " LayerNo:" + layerNo);
    //        if (res.HasError)
    //        {
    //            if (res.ErrorCode == "06" || res.ErrorCode == "02")
    //            {
    //                c_TdcService.MoveLot(lotNo, mcNo, opNo, layerNo);
    //                //log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "TDC", 0, "LotRequest", 
    //                //    res.ErrorCode + ":" + res.ErrorMessage, "lotNo:" + lotNo + " opNo:" + opNo + " LayerNo:" + layerNo);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name,"Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
    //    }

    //}
    //private TdcLotRequestResult TdcLotRequest(string mcNoApcs,string lotNo,RunModeType runMode, Logger log)
    //{
    //    try
    //    {
    //        //TDC priority
    //        c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
    //        TdcLotRequestResponse tdcLotRequest = c_TdcService.LotRequest(mcNoApcs, lotNo, runMode);
    //        if (tdcLotRequest.HasError)
    //        {
    //            using (ApcsWebServiceSoapClient svError = new ApcsWebServiceSoapClient())
    //            {
    //                if (!svError.LotRptIgnoreError(mcNoApcs, tdcLotRequest.ErrorCode))
    //                {
    //                    return new TdcLotRequestResult(tdcLotRequest.ErrorCode, tdcLotRequest.ErrorMessage);
    //                }
    //            }
    //        }
    //        return new TdcLotRequestResult(tdcLotRequest.GoodPieces, tdcLotRequest.BadPieces);
    //    }
    //    catch (Exception ex)
    //    {
    //        LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
    //        return new TdcLotRequestResult("0", ex.Message.ToString());
    //    }

    //}
    //private TdcLotSetResult TdcLotSet(string mcNoApcs, string lotNo, string opNo, RunModeType runMode, DateTime dateTime, Logger log)
    //{
    //    try
    //    {
    //        //Init TCD Library
    //        c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
    //        TdcResponse response = c_TdcService.LotSet(mcNoApcs, lotNo, dateTime, opNo, runMode);
    //        if (response.HasError)
    //        {
    //            return new TdcLotSetResult(response.ErrorCode, response.ErrorMessage);
    //        }
    //        return new TdcLotSetResult();
    //    }
    //    catch (Exception ex)
    //    {
    //        LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
    //        return new TdcLotSetResult("0", ex.Message.ToString());
    //    }


    //}
    //private TdcLotEndResult TdcLotEnd(string mcNoApcs, string lotNo, string opNo, DateTime dateTime, Logger log, int good, int ng , EndModeType endMode)
    //{
    //    try
    //    {
    //        //Init TCD Library
    //        c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
    //        TdcResponse response = c_TdcService.LotEnd(mcNoApcs, lotNo, dateTime, good, ng, endMode, opNo);
    //        if (response.HasError)
    //        {
    //            return new TdcLotEndResult(response.ErrorMessage, response.ErrorCode);
    //        }
    //        return new TdcLotEndResult();
    //    }
    //    catch (Exception ex)
    //    {
    //        LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
    //        return new TdcLotEndResult("0", ex.Message.ToString());
    //    }


    //}
    //private TdcLoggerCsvWriter CreateLogTdc(string pathLog)
    //{
    //    //try
    //    //{
    //    //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"~\\Log\\" + pathLog)))
    //    //    {
    //    //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\\Log\\" + pathLog));
    //    //    }
    //    //}
    //    //catch (Exception)
    //    //{

    //    //}
    //    try
    //    {
    //        if (File.Exists(Path.Combine(pathLog, c_FileTdcBackupName)))
    //        {
    //            if (!Directory.Exists(Path.Combine(pathLog, c_PathFolderBackupTdc)))
    //            {
    //                Directory.CreateDirectory(Path.Combine(pathLog, c_PathFolderBackupTdc));
    //            }

    //            List<FileData> fileDatas = new List<FileData>();
    //            var pathFiles = Directory.GetFileSystemEntries(Path.Combine(pathLog, c_PathFolderBackupTdc));
    //            foreach (string pathFile in pathFiles)
    //            {              
    //                FileData fileData = new FileData()
    //                {
    //                    FileName = Path.GetFileName(pathFile).Trim(),
    //                    ModifiedDate = File.GetLastWriteTime(pathFile),
    //                    CreateTime = File.GetCreationTime(pathFile),
    //                    Path = pathFile
    //                };
    //                fileDatas.Add(fileData);
    //            }
    //            int maxsimum = 100;
    //            int.TryParse(AppSettingHelper.GetAppSettingsValue("FileApcsLog"),out maxsimum);
    //            if (fileDatas.Count >= maxsimum)
    //            {
    //                int countRemove = fileDatas.Count - (maxsimum - 1);
    //                var fileOlds = fileDatas.OrderBy(x => x.CreateTime).Take(countRemove).ToList();
    //                foreach (FileData fileOld in fileOlds)
    //                {
    //                    File.Delete(fileOld.Path);
    //                }

    //            }
    //            File.Copy(Path.Combine(pathLog, c_FileTdcBackupName), Path.Combine(pathLog, c_PathFolderBackupTdc, "TDC_before_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv"), true);
    //            File.Delete(Path.Combine(pathLog, c_FileTdcBackupName));
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        LogFile.SaveLog("CatchLog",0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
    //    }

    //    TdcLoggerCsvWriter tdcLogger = new TdcLoggerCsvWriter();
    //    tdcLogger.LogFolder = pathLog;//HttpContext.Current.Server.MapPath(@"~\\Log\\" + pathLog);
    //    c_TdcService.Logger = tdcLogger;

    //    return tdcLogger;
    //}
    #endregion

    #region iReport

    public iReportResponse IRePortCheck(string mcNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTime dtConfig;
            DateTime.TryParse("13:30:00", out dtConfig); //Manual config time Limit 
            TimeSpan tsConfig = dtConfig.TimeOfDay;
            TimeSpan tmpTs = DateTime.Now.TimeOfDay;
            DateTime dtNow = DateTime.Now;
            if (tmpTs < tsConfig)
            {
                dtNow = dtNow.AddDays(-1);
            }
            using (Service1SoapClient iReport = new Service1SoapClient())
            {
                iReportResponse result = iReport.iRePortCheck(mcNo, dtNow);
                string typeState;
                if (result.HasError)
                {
                    typeState = "Error";
                }
                else
                {
                    typeState = "Normal";
                }
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, typeState, "WCFService", "iReportService", 0, "IRePortCheck", result.ErrorMessage, dtNow.ToString("yyyy/MM/dd HH:mm:ss"));
                return result;
            }
        }
        catch (Exception ex)
        {
            iReportResponse result = new iReportResponse();
            result.HasError = true;
            result.ErrorMessage = "Exception:" + ex.Message.ToString();

            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "iReportService", 0, "IRePortCheck", result.ErrorMessage, "");

            return result;
        }

    }
    #endregion

    #region CarrierControl
    private ResultBase GetCarrierControl(LotInfo lotInfo)
    {
        ResultBase result = new ResultBase();
        //Check process control carrier
        DataTable tmpDataTable = new DataTable();
        using (SqlCommand cmd = new SqlCommand())
        {
            string job = "";
            if (lotInfo.IsSpecialFlow)
            {
                job = lotInfo.SpJob.Name;
            }
            else
            {
                job = lotInfo.Job.Name;
            }
            cmd.Connection = new SqlConnection("Data Source = 172.16.0.102; Initial Catalog = StoredProcedureDB; Persist Security Info = True; User ID = system; Password = 'p@$$w0rd'");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[cellcon].[sp_get_carrier]";
            cmd.Parameters.Add("@job", SqlDbType.VarChar).Value = job;
            cmd.Connection.Open();
            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                if (rd.HasRows)
                {
                    tmpDataTable.Load(rd);
                }
            }
            cmd.Connection.Close();
        }
        if (tmpDataTable.Rows.Count <= 0)
        {
            result.IsPass = false;
            result.Reason = "ไม่สามารถเข้าถึง [cellcon].[sp_get_carrier] ได้(" + lotInfo.Job.Name + ")";
            return result;
        }

        foreach (DataRow row in tmpDataTable.Rows)
        {

            if (!(row["message"] is DBNull)) result.Reason = (string)row["message"];
            if (!(row["enabled"] is DBNull)) result.IsPass = (bool)row["enabled"];

        }
        return result;
    }
    public CarrierInfo GetCarrierInfo(string mcNo,string lotNo,string opNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        CarrierInfo result = new CarrierInfo();
        try
        {
          
            DateTime dateTime = DateTime.Now;
            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTime);
            if (machineInfo == null)
            {
                result.IsPass = false;
                result.Reason = "machineInfo not found (" + mcNo + ")";
                return result;
            }
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTime);
            if (lotInfo == null)
            {
                result.IsPass = false;
                result.Reason = "lotInfo not found (" + lotNo + ")";
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCF", "iLibrary", 0, "GetLotInfo", "mcNo :" + mcNo + ",lotNo :" + lotNo + ",opNo :" + opNo, "");
                return result;
            }

            ResultBase resultBase = GetCarrierControl(lotInfo);
            if (!resultBase.IsPass)
            {
                result.InControlCarrier = CarrierInfo.CarrierStatus.No_Use;
                result.LoadCarrier = CarrierInfo.CarrierStatus.No_Use;
                result.UnloadCarrier = CarrierInfo.CarrierStatus.No_Use;
                result.RegisterCarrier = CarrierInfo.CarrierStatus.No_Use;
                result.TransferCarrier = CarrierInfo.CarrierStatus.No_Use;
                result.EnabledControlCarrier = CarrierInfo.CarrierStatus.No_Use;
                result.IsPass = true;
                result.Reason = resultBase.Reason;
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCF", "iLibrary", 0, "GetCarrierControl", result.Reason, "mcNo :" + mcNo + ",lotNo :" + lotNo + ",opNo :" + opNo);
                return result;
            }
            result.EnabledControlCarrier = CarrierInfo.CarrierStatus.Use;

            CarrierControlResult carrierControlResult = c_ApcsProService.GetCarrierInformation(machineInfo.Id, lotInfo.Id, log);

            if (!carrierControlResult.IsPass)
            {
                result.IsPass = false;
                result.Reason = carrierControlResult.ErrorMessageDetail.Error_Message;
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCF", "iLibrary", 0, "GetCarrierInformation", carrierControlResult.ErrorMessageDetail.Error_Message , "mcNo :" + mcNo + ",lotNo :" + lotNo + ",opNo :" + opNo);
                return result;
            }
          
            if (carrierControlResult.CarrierInfo == null)
            {
                result.IsPass = false;
                result.Reason = "CarrierInfo not found";
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCF", "iLibrary", 0, "GetCarrierInformation", result.Reason, "mcNo :" + mcNo + ",lotNo :" + lotNo + ",opNo :" + opNo);
                return result;
            }

            //ResultBase resultBase = GetCarrierControl(lotInfo);
            //if (!resultBase.IsPass)
            //{
            //    result.InControlCarrier = CarrierInfo.Status.No_Use;
            //    result.LoadCarrier = CarrierInfo.Status.No_Use;
            //    result.UnloadCarrier = CarrierInfo.Status.No_Use;
            //    result.RegisterCarrier = CarrierInfo.Status.No_Use;
            //    result.TransferCarrier = CarrierInfo.Status.No_Use;
            //    result.EnabledControlCarrier = CarrierInfo.Status.No_Use;
            //    result.IsPass = true;
            //    result.Reason = resultBase.Reason;
            //    log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCF", "iLibrary", 0, "GetCarrierControl", result.Reason, "mcNo :" + mcNo + ",lotNo :" + lotNo + ",opNo :" + opNo);
            //    return result;
            //}
            result.CurrentCarrierNo = carrierControlResult.CarrierInfo.CurrentCarrier;
            result.NextCarrierNo = carrierControlResult.CarrierInfo.NextCarrier;
            result.TransferCarrierNo = carrierControlResult.CarrierInfo.NextCarrier;
            result.InControlCarrier = (CarrierInfo.CarrierStatus)carrierControlResult.CarrierInfo.InControl;
            result.LoadCarrier = (CarrierInfo.CarrierStatus)carrierControlResult.CarrierInfo.VerificationOnStart;
            result.RegisterCarrier = (CarrierInfo.CarrierStatus)carrierControlResult.CarrierInfo.CarrierRegister;
            result.UnloadCarrier = (CarrierInfo.CarrierStatus)carrierControlResult.CarrierInfo.VerificationOnEnd;
            result.TransferCarrier = (CarrierInfo.CarrierStatus)carrierControlResult.CarrierInfo.CarrierTransfer;
            result.IsPass = true;
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCF", "iLibrary", 0, "GetCarrierInfo", result.Reason, "mcNo :" + mcNo + ",lotNo :" + lotNo + ",opNo :" + opNo);
            return result;

        }
        catch (Exception ex)
        {
            
            result.IsPass = false;
            result.Reason = "Exception :" + ex.Message.ToString();
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCF", "iLibrary", 0, "GetCarrierInformation", result.Reason, "mcNo :" + mcNo + ",lotNo :" + lotNo + ",opNo :" + opNo);
            return result;
        }
               
    }

    #endregion
    #region DENPYO_PRINT
    private string GetPackageName(string lotNo)
    {
        string pkg = "";
        using (SqlCommand cmd = new SqlCommand())
        {
            cmd.Connection = new SqlConnection("Data Source = 172.16.0.102; Initial Catalog = APCSDB; Persist Security Info = True; User ID = system; Password = 'p@$$w0rd'");
            cmd.Connection.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT FORM_NAME_1,FORM_NAME_2,FORM_NAME_3,FORM_NAME_4,FORM_NAME_6" +
                ",LOT_NO_1,LOT_NO_2,LOT_NO_3,LOT_NO_4,ZUBAN_1 FROM [APCSDB].[dbo].[LCQW_UNION_WORK_DENPYO_PRINT]" +
                " where LOT_NO_1 = @lotNo or LOT_NO_2 = @lotNo or LOT_NO_3 = @lotNo or LOT_NO_4 = @lotNo";
            cmd.Parameters.Add("@lotNo", SqlDbType.NVarChar).Value = lotNo;
            DataTable dataTable = new DataTable();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
                //if (reader.HasRows)
                //{
                //    //if (reader["ZUBAN_1"] != DBNull.Value)
                //    //{
                //    //    return (string)reader["FORM_NAME_1"];
                //    //}
                //    if (reader["FORM_NAME_1"] != DBNull.Value) return (string)reader["FORM_NAME_1"];
                //    else if (reader["FORM_NAME_2"] != DBNull.Value) return (string)reader["FORM_NAME_2"];
                //    else if (reader["FORM_NAME_3"] != DBNull.Value) return (string)reader["FORM_NAME_3"];
                //    else if (reader["FORM_NAME_4"] != DBNull.Value) return (string)reader["FORM_NAME_4"];
                //    else if (reader["FORM_NAME_6"] != DBNull.Value) return (string)reader["FORM_NAME_6"];
                //}
            }
            cmd.Connection.Close();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["FORM_NAME_1"] != DBNull.Value && (string)row["FORM_NAME_1"] != "")
                {
                    pkg = (string)row["FORM_NAME_1"];
                }
                else if (row["FORM_NAME_2"] != DBNull.Value && (string)row["FORM_NAME_2"] != "")
                {
                    pkg=(string)row["FORM_NAME_2"];
                }
                else if (row["FORM_NAME_3"] != DBNull.Value && (string)row["FORM_NAME_3"] != "")
                {
                    pkg=(string)row["FORM_NAME_3"];

                }
                else if (row["FORM_NAME_4"] != DBNull.Value && (string)row["FORM_NAME_4"] != "")
                {
                    pkg=(string)row["FORM_NAME_4"];

                }
                else if (row["FORM_NAME_6"] != DBNull.Value && (string)row["FORM_NAME_6"] != "")
                {
                    pkg=(string)row["FORM_NAME_6"];

                }
            }
            string fullName = GetPackageFullName(pkg);
            if (!string.IsNullOrEmpty(fullName))
            {
                pkg = fullName;
            }

        }
        return pkg;
    }
    #endregion
    #region TemporaryFunction
    //Temporary for D lot ที่ยังไม่เปิดระบบ APCS Pro เพราะ D lot มันจะไม่มีในระบบ apcs 2019/12/18 (พี่ฟรี พี่ก็อตเล็ก)
    public bool CheckPackageOnlyApcsPro(string mcNo,string package,string opNo,string lotNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            string fullName = GetPackageFullName(package);
            if (!string.IsNullOrEmpty(fullName))
            {
                package = fullName.Trim();
            }

            bool result =  c_ApcsProService.CheckPackageEnable(package, log);
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "iLibrary", 0, "CheckPackageEnable", "result=" + result, "package[" + package + "] opNo[" + opNo + "] lotNo["+ lotNo + "]");
            return result;
        }
        catch (Exception ex)
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "iLibrary", 0, "CheckPackageEnable", "Exception:" + ex.Message.ToString(), "package[" + package + "] opNo[" + opNo + "] lotNo[" + lotNo + "]");
            throw;
        }
     
    }
    private string GetPackageFullName(string packageShortName)
    {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(AppSettingHelper.GetConnectionStringValue("ApcsProConnectionString"));
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT [name],[short_name] FROM [APCSProDB].[method].[packages] WHERE [short_name] = @package_short_name";
                cmd.Parameters.Add("@package_short_name", SqlDbType.NVarChar).Value = packageShortName;
                DataTable dataTable = new DataTable();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
                cmd.Connection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["name"] != DBNull.Value && (string)row["name"] != "")
                    {
                        return (string)row["name"];
                    }
                }

            }
            return "";
    }
    #endregion
    #region AutoRegisterCarrier
    private ResultBase SetAutoCarrier(string lotNo,string carrierNo,string mcNo, CarrierStatue carrierStatue,Logger log)
    {
        ResultBase result = new ResultBase();
        try
        {
          
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection("Data Source = 172.16.0.102; Initial Catalog = StoredProcedureDB; Persist Security Info = True; User ID = system; Password = 'p@$$w0rd'; Application Name = iLibraryService");
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[cellcon].[sp_set_register_carrier]";
                cmd.Parameters.Add("@lot_no", SqlDbType.VarChar).Value = lotNo;
                cmd.Parameters.Add("@carrier_no", SqlDbType.VarChar).Value = carrierNo;
                cmd.Parameters.Add("@status", SqlDbType.Int).Value = (int)carrierStatue;
                cmd.Parameters.Add("@mcno", SqlDbType.VarChar).Value = mcNo;
                DataTable dataTable = new DataTable();
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    dataTable.Load(dataReader);
                }
                //cmd.Connection.Close();
                foreach (DataRow item in dataTable.Rows)
                {
                    bool isUpdate = false;
                    CarrierStatue statusCarrier = CarrierStatue.NotSet;
                    if (item["is_update"] != null)
                    {
                        isUpdate = (bool)item["is_update"];
                    }
                    if (item["status_carrier"] != null)
                    {
                        if ((int)item["status_carrier"] == 1)
                        {
                            statusCarrier = CarrierStatue.Load;
                        }
                        else if ((int)item["status_carrier"] == 2)
                        {
                            statusCarrier = CarrierStatue.Unload;
                        }
                    }
                    log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCF", "StoredProcedureDB", 0, "SetAutoCarrier", "IsUpdate:" + isUpdate + " status_carrier:" + statusCarrier.ToString(), "status_carrier 1 = Load ,2 = Unload");

                }
            }
            result.IsPass = true;
            
        }
        catch (Exception ex)
        {
            result.IsPass = false;
            result.Reason = "Exception : " + ex.Message.ToString();
        }
        return result;
    }
    enum CarrierStatue
    {
        NotSet =0,
        Load = 1,
        Unload = 2
      
    }
    #endregion

}
