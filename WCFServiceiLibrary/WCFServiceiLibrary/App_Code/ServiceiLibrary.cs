using iLibrary;
using Rohm.Apcs.Tdc;
using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web;
using TDCService;
using IReport;
using LotInfo = iLibrary.LotInfo;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceiLibrary" in code, svc and config file together.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServiceiLibrary : IServiceiLibrary
{
    private ApcsProService c_ApcsProService;
    private TdcService c_TdcService;
    public int count;
    private string c_PahtLogFile;
    private const string c_LogVersion = "1.0.0";
    private const string c_PathFolderBackupTdc = "backupTDC";
    private const string c_FileTdcBackupName = "TDC_before.csv";
    public ServiceiLibrary()
    {
        c_ApcsProService = new ApcsProService();
        c_TdcService =  TdcService.GetInstance();
        //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
        c_PahtLogFile = HttpContext.Current.Server.MapPath(@"~\\Log");
    }
    #region Setup
    
    public SetupLotResult SetupLot(string lotNo, string mcNo, string opNo, string processName, string layerNo)
    {
        return SetupLotCommon(lotNo, mcNo, opNo, processName, layerNo, RunMode.Normal, MethodBase.GetCurrentMethod().Name);
    }
    
    public SetupLotResult SetupLotNoCheckLicenser(string lotNo, string mcNo, string opNo, string processName, string layerNo)
    {
        return SetupLotCommon(lotNo, mcNo, opNo, processName, layerNo, RunMode.Normal, MethodBase.GetCurrentMethod().Name, false);
    }

    public SetupLotResult SetupLotCustomMode(string lotNo, string mcNo, string opNo, string processName, string layerNo, RunMode runMode)
    {
        return SetupLotCommon(lotNo, mcNo, opNo, processName, layerNo, runMode, MethodBase.GetCurrentMethod().Name);
    }
    public SetupLotResult SetupLotCustomModeNoCheckLicenser(string lotNo, string mcNo, string opNo, string processName, string layerNo, RunMode runMode)
    {
        return SetupLotCommon(lotNo, mcNo, opNo, processName, layerNo,runMode, MethodBase.GetCurrentMethod().Name, false);
    }

    //Oven พี่อาร์ม
    public SetupLotResult SetupLotOven(string lotNo, string mcNoApcsPro, string mcNoApcs, string opNo, string processName, string layerNo)
    {
        return SetupLotCommon(lotNo, mcNoApcsPro, opNo, processName, layerNo, RunMode.Normal, MethodBase.GetCurrentMethod().Name,false, mcNoApcs);
    }

    private SetupLotResult SetupLotCommon(string lotNo, string mcNo, string opNo, string processName, string layerNo, RunMode tdcRunModeType,string functionName, bool isCheckLicenser = true,string mcNoApcs = "")
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
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
          
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
            {
                //รอการแก้บัค เนื่องจากคิดว่าทุก lot จะมีใน db ของ ApcsPro แต่ไม่ใช่จะมีแค่ device slip ที่ลงทะเบียนแล้ว ถึงจะนำ lot เข้ามาในระบบ Apcs Pro (SetUp Start End)
                TdcLotRequestResult requestResult = TdcLotRequest(mcNoToApcs, lotNo, (RunModeType)tdcRunModeType, log);
                if (!requestResult.IsPass)
                {
                    return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Apcs, requestResult.Cause,
                              "LotNo:" + lotNo + " opNo:" + opNo, "", requestResult.ErrorNo,
                              "CheckLotApcsPro", functionName, log);
                }
                return new SetupLotResult(SetupLotResult.Status.Pass, MessageType.Apcs, "lotInfo is null", "LotNo:" + lotNo + " opNo:" + opNo, "", "",
                    "TdcLotRequest", functionName, log, requestResult.GoodQty, requestResult.NgQty);
                //return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ",
                //   "LotNo:" + lotNo + " opNo:" + opNo, "", "", "GetLotInfo", functionName, log);
            }
            //Check package and Lot  
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                //TDC priority
                TdcLotRequestResult requestResult = TdcLotRequest(mcNoToApcs, lotNo, (RunModeType)tdcRunModeType,log);
                if (!requestResult.IsPass)
                {
                    return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Apcs, requestResult.Cause + 
                        Environment.NewLine + "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
                              "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name, "", requestResult.ErrorNo,
                              "CheckLotApcsPro", functionName, log);
                }
                return new SetupLotResult(SetupLotResult.Status.Pass, MessageType.Apcs, "", "LotNo:" + lotNo + " opNo:" + opNo, "", "",
                    "TdcLotRequest", functionName, log, requestResult.GoodQty, requestResult.NgQty);
            }
            else
            {
                //Apcs Pro priority
               
                if (IsSkipped(mcNo))
                {
                    return new SetupLotResult(SetupLotResult.Status.Pass, MessageType.ApcsPro, "IsSkipped", "web.config", "", "", "IsSkipped", functionName, log);
                }

                UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
                if (userInfo == null)
                {
                    return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
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

                //Check Permission
                CheckUserPermissionResult permissionResult = c_ApcsProService.CheckUserPermission(userInfo, "CellController",
                    processName + "-SetupLot", log, dateTimeInfo.Datetime);
                if (!permissionResult.IsPass)
                    return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, permissionResult.ErrorMessage,
                        "LotNo:" + lotNo + " opNo:" + opNo + " processName:" + processName, "", permissionResult.ErrorNo.ToString(), 
                        "CheckUserPermission", functionName, log);

                if (isCheckLicenser)
                {
                    if (!c_ApcsProService.Check_PermissionMachinesByLMS(userInfo.Id, mcNo, log))
                    {
                        return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro,"รหัส : " + userInfo.Code +
                             " ไม่ผ่านการตรวจสอบในระบบ Licenser กรุณาติดต่อ ETG (MCNo : " + mcNo + ")","","", "",
                             "Check_PermissionMachinesByLMS", functionName, log);
                    }
                    if (!c_ApcsProService.Check_UserLotAutoMotive(userInfo, lotInfo, log))
                    {
                        return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, "รหัส : " + userInfo.Code +
                             " User ที่ไม่ใช่ Automotive ไม่สามารถรัน Lot Automotive ได้ กรุณาติดต่อ ETG (Lot Automotive : " + lotInfo.Name + ")","", "","",
                             "Check_UserLotAutoMotive", functionName, log);
                    }
                }

                MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
                if (machineInfo == null)
                    return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", 
                        "LotNo:" + lotNo + " opNo:" + opNo + " mcNo" + mcNo, "","", "GetMachineInfo",
                        functionName, log);
            
                LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotSetup(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", 1, dateTimeInfo.Datetime, log);
                if (!lotUpdateInfo.IsOk)
                    return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, lotUpdateInfo.ErrorMessage + Environment.NewLine
                        + "MCNo:" + machineInfo.Name + Environment.NewLine
                        + "LotNo:" + lotInfo.Name + " || OPID:" + userInfo.Code + Environment.NewLine
                        + "เป็นงานของ:" + lotInfo.Job.Name,
                       "", "", lotUpdateInfo.ErrorNo.ToString(), "LotSetup",
                        functionName, log);


                //TDC Move
                TdcMove(mcNoToApcs, lotNo, opNo, layerNo, log);

                if (warningMessage != "")
                {
                    return new SetupLotResult(SetupLotResult.Status.Warning,MessageType.ApcsPro, warningMessage, 
                        "LotNo:" + lotNo + " opNo:" + opNo, lotUpdateInfo.Recipe1, "","GetUserInfo", functionName, log, lotInfo.Quantity.Pass, lotInfo.Quantity.LastFail);
                }
                else
                {
                    return new SetupLotResult(SetupLotResult.Status.Pass,MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, lotUpdateInfo.Recipe1, "", "",
                        functionName, log, lotInfo.Quantity.Pass, lotInfo.Quantity.LastFail);
                }
            }
           
        }
        catch (Exception ex)
        {
            return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.Unknown, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo,"", "", "Exception",
                functionName, log);
        }
    }

    #endregion

    #region Start
    public StartLotResult StartLot(string lotNo, string mcNo, string opNo, string recipe)
    {
        return StartLotCommon(lotNo, mcNo, opNo, recipe,MethodBase.GetCurrentMethod().Name, RunMode.Normal);
    }
    public StartLotResult StartLotCustomMode(string lotNo, string mcNo, string opNo, string recipe,RunMode runMode)
    {
        return StartLotCommon(lotNo, mcNo, opNo, recipe, MethodBase.GetCurrentMethod().Name, runMode);
    }
    public StartLotResult StartLotOven(string lotNo, string mcNoApcsPro ,string mcNoApcs, string opNo, string recipe)
    {
        return StartLotCommon(lotNo, mcNoApcsPro, opNo, recipe, MethodBase.GetCurrentMethod().Name, RunMode.Normal, mcNoApcs);
    }
    private StartLotResult StartLotCommon(string lotNo, string mcNo, string opNo, string recipe,string functionName,RunMode runMode,string mcNoApcs = "")
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            string mcNoToApcs;
            if (mcNoApcs != "")
            {
                mcNoToApcs = mcNoApcs;
            }
            else
            {
                mcNoToApcs = mcNo;
            }
            //TDC LotSet
            TdcLotSetResult tdcLotSetResult = TdcLotSet(mcNoToApcs, lotNo, opNo, (RunModeType)runMode, dateTimeInfo.Datetime, log);
            if (!tdcLotSetResult.IsPass)
            {
                //
                // TODO: Add constructor logic here
                //
            }
       
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
            {
                return new StartLotResult(true, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
              "GetLotInfo", functionName, log);
                //return new StartLotResult(false, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                //  "GetLotInfo", functionName, log);
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
                    return new StartLotResult(true, MessageType.ApcsPro, "IsSkipped", "web.config", "IsSkipped", functionName, log);
                }
                UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
                if (userInfo == null)
                {
                    return new StartLotResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                        "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
                }
                MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
                if (machineInfo == null)
                    return new StartLotResult(false,MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                        "GetMachineInfo", functionName, log);

                LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotStart(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", 1, recipe, log, 1, -1);
                if (!lotUpdateInfo.IsOk)
                    return new StartLotResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo +
                        " mcNo:" + mcNo, "LotStart", functionName, log);
            }
           
           
            return new StartLotResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", functionName, log);
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
        return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, false);
    }
    public EndLotResult EndLotNoCheckLicenser(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        return EndLotCommon(lotNo, mcNo, opNo, good, ng,MethodBase.GetCurrentMethod().Name, false);
    }
    public EndLotResult EndLotOven(string lotNo, string mcNoApcs,string mcNoApcsPro, string opNo, int good, int ng)
    {
        return EndLotCommon(lotNo, mcNoApcs, opNo, good, ng, MethodBase.GetCurrentMethod().Name, false, mcNoApcsPro);
    }
    //public EndLotResult EndLotCustomMode(string lotNo, string mcNo, string opNo, int good, int ng ,EndMode endMode)
    //{
    //    return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, endMode, false);
    //}
    //public EndLotResult EndLotCostomModeNoCheckLicenser(string lotNo, string mcNo, string opNo, int good, int ng , EndMode endMode)
    //{
    //    return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, endMode, false);
    //}
    private EndLotResult EndLotCommon(string lotNo, string mcNo, string opNo, int good, int ng,string functionName,  bool isCheckLicenser = true,string mcNoApcs = "")
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            string mcNoToApcs;
            if (mcNoApcs != "")
            {
                mcNoToApcs = mcNoApcs;
            }
            else
            {
                mcNoToApcs = mcNo;
            }
            //EndModeType.AbnormalEndAccumulate รันใหม่ทั้งหมด
            //EndModeType.AbnormalEndReset รันต่อที่เหลือ
            //EndModeType.Normal จบ Lot ปกติ
            TdcLotEndResult tdcLotEndResult = TdcLotEnd(mcNoToApcs, lotNo, opNo, dateTimeInfo.Datetime, log, good, ng, EndModeType.Normal);
            if (!tdcLotEndResult.IsPass)
            {
                //
                // TODO: Add constructor logic here
                //
            }

            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
            {
                //return new EndLotResult(false, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                //   " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
                return new EndLotResult(true, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                   " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
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

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new EndLotResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new EndLotResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                    " good:" + good + " ng:" + ng, "GetMachineInfo", functionName, log);

            if (isCheckLicenser)
            {
                if (!c_ApcsProService.Check_PermissionMachinesByLMS(userInfo.Id, mcNo, log))
                {
                    return new EndLotResult(false, MessageType.ApcsPro, "Check_PermissionMachinesByLMS", "รหัส : " + userInfo.Code +
                         " ไม่ผ่านการตรวจสอบในระบบ Licenser กรุณาติดต่อ ETG (MCNo : " + mcNo + ")", "Check_PermissionMachinesByLMS",
                         functionName, log);
                }
                if (!c_ApcsProService.Check_UserLotAutoMotive(userInfo, lotInfo, log))
                {
                    return new EndLotResult(false, MessageType.ApcsPro, "Check_PermissionMachinesByLMS", "รหัส : " + userInfo.Code +
                         " User ที่ไม่ใช่ Automotive ไม่สามารถรัน Lot Automotive ได้ กรุณาติดต่อ ETG (Lot Automotive : " + lotInfo.Name + ")",
                         "Check_UserLotAutoMotive", functionName, log);
                }
            }

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotEnd(lotInfo.Id, machineInfo.Id, userInfo.Id, false, good, ng, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new EndLotResult(false, MessageType.ApcsPro, lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo +
                    " mcNo:" + mcNo + " good:" + good + " ng:" + ng, "LotEnd", functionName, log);

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
            return new EndLotResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", functionName, log, nextFlow);
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
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);

            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
            {
                TdcLotEndResult tdcLotEndResult = TdcLotEnd(mcNoToApcs, lotNo, opNo, dateTimeInfo.Datetime, log, good, ng, (EndModeType)endMode);
                if (!tdcLotEndResult.IsPass)
                {
                    //
                    // TODO: Add constructor logic here
                    //
                }
                return new ReinputResult(true, MessageType.Apcs, "ไม่พบ lotInfo", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "TdcLotEnd",
                    functionName, log);
            }
                //return new ReinputResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                //    " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                TdcLotEndResult tdcLotEndResult = TdcLotEnd(mcNoToApcs, lotNo, opNo, dateTimeInfo.Datetime, log, good, ng, (EndModeType)endMode);
                if (!tdcLotEndResult.IsPass)
                {
                    //
                    // TODO: Add constructor logic here
                    //
                }
                return new ReinputResult(true, MessageType.Apcs, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "TdcLotEnd",
                    functionName, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            if (userInfo == null)
            {
                return new ReinputResult(false, MessageType.Unknown, "ไม่พบผู้ใช้งานนี้ :" + opNo,
                    "", "GetUserInfo", MethodBase.GetCurrentMethod().Name, log);
            }

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new ReinputResult(false, MessageType.ApcsPro, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                    " good:" + good + " ng:" + ng, "GetMachineInfo", functionName, log);
            LotUpdateInfo lotUpdateInfo = c_ApcsProService.AbnormalLotEnd_BackToThe_BeforeProcess(lotInfo.Id, machineInfo.Id, userInfo.Id, holdLot,
                good, ng, 0, "", 1, dateTimeInfo.Datetime, log);

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
    
    

    public MachineOnlineStateResult MachineOnlineState(string mcNo, MachineOnline online)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile); 
        try
        {
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
        catch (Exception ex)
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
                    return new MachineAlarmResult(false, MessageType.ApcsPro, machineAlarmSet.ErrorNo + ":" + machineAlarmSet.ErrorMessage,
                        "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(),
                        "Update_ErrorHappenRecord", MethodBase.GetCurrentMethod().Name, log);
            }
            else
            {
                try
                {
                    MachineUpdateInfo machineAlarmReset = c_ApcsProService.Update_ErrorResetRecord(machineInfo, userInfo.Id, AlarmNo, dateTimeInfo.Datetime, log);
                    if (!machineAlarmReset.IsOk)
                        return new MachineAlarmResult(false, MessageType.ApcsPro, machineAlarmReset.ErrorNo + ":" + machineAlarmReset.ErrorMessage,
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
    #endregion


    #region TDC
    private void TdcMove(string mcNo, string lotNo, string opNo, string layerNo, Logger log)
    {
        try
        {
            if (string.IsNullOrEmpty(layerNo))
            {
                LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "TDC",
                       "NoMove", "", "lotNo:" + lotNo + " opNo:" + opNo + " LayerNo:" + layerNo);
                return;
            }
            //Init TCD Library    
            c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
            TdcLotRequestResponse res = c_TdcService.LotRequest(mcNo, lotNo, RunModeType.Normal);
            //log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "TDC", 0, "LotRequest", "",
            //    "lotNo:" + lotNo + " opNo:" + opNo + " LayerNo:" + layerNo);
            if (res.HasError)
            {
                if (res.ErrorCode == "06" || res.ErrorCode == "02")
                {
                    c_TdcService.MoveLot(lotNo, mcNo, opNo, layerNo);
                    //log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "TDC", 0, "LotRequest", 
                    //    res.ErrorCode + ":" + res.ErrorMessage, "lotNo:" + lotNo + " opNo:" + opNo + " LayerNo:" + layerNo);
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name,"Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
        }
        
    }
    private TdcLotRequestResult TdcLotRequest(string mcNoApcs,string lotNo,RunModeType runMode, Logger log)
    {
        try
        {
            //TDC priority
            c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
            TdcLotRequestResponse tdcLotRequest = c_TdcService.LotRequest(mcNoApcs, lotNo, runMode);
            if (tdcLotRequest.HasError)
            {
                using (ApcsWebServiceSoapClient svError = new ApcsWebServiceSoapClient())
                {
                    if (!svError.LotRptIgnoreError(mcNoApcs, tdcLotRequest.ErrorCode))
                    {
                        return new TdcLotRequestResult(tdcLotRequest.ErrorCode, tdcLotRequest.ErrorMessage);
                    }
                }
            }
            return new TdcLotRequestResult(tdcLotRequest.GoodPieces, tdcLotRequest.BadPieces);
        }
        catch (Exception ex)
        {
            LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
            return new TdcLotRequestResult("0", ex.Message.ToString());
        }
        
    }
    private TdcLotSetResult TdcLotSet(string mcNoApcs, string lotNo, string opNo, RunModeType runMode, DateTime dateTime, Logger log)
    {
        try
        {
            //Init TCD Library
            c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
            TdcResponse response = c_TdcService.LotSet(mcNoApcs, lotNo, dateTime, opNo, runMode);
            if (response.HasError)
            {
                return new TdcLotSetResult(response.ErrorCode, response.ErrorMessage);
            }
            return new TdcLotSetResult();
        }
        catch (Exception ex)
        {
            LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
            return new TdcLotSetResult("0", ex.Message.ToString());
        }


    }
    private TdcLotEndResult TdcLotEnd(string mcNoApcs, string lotNo, string opNo, DateTime dateTime, Logger log, int good, int ng , EndModeType endMode)
    {
        try
        {
            //Init TCD Library
            c_TdcService.Logger = CreateLogTdc(c_PahtLogFile);
            TdcResponse response = c_TdcService.LotEnd(mcNoApcs, lotNo, dateTime, good, ng, endMode, opNo);
            if (response.HasError)
            {
                return new TdcLotEndResult(response.ErrorMessage, response.ErrorCode);
            }
            return new TdcLotEndResult();
        }
        catch (Exception ex)
        {
            LogFile.SaveLog(log, 0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
            return new TdcLotEndResult("0", ex.Message.ToString());
        }


    }
    private TdcLoggerCsvWriter CreateLogTdc(string pathLog)
    {
        //try
        //{
        //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(@"~\\Log\\" + pathLog)))
        //    {
        //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\\Log\\" + pathLog));
        //    }
        //}
        //catch (Exception)
        //{

        //}
        try
        {
            if (File.Exists(Path.Combine(pathLog, c_FileTdcBackupName)))
            {
                if (!Directory.Exists(Path.Combine(pathLog, c_PathFolderBackupTdc)))
                {
                    Directory.CreateDirectory(Path.Combine(pathLog, c_PathFolderBackupTdc));
                }

                List<FileData> fileDatas = new List<FileData>();
                var pathFiles = Directory.GetFileSystemEntries(Path.Combine(pathLog, c_PathFolderBackupTdc));
                foreach (string pathFile in pathFiles)
                {              
                    FileData fileData = new FileData()
                    {
                        FileName = Path.GetFileName(pathFile).Trim(),
                        ModifiedDate = File.GetLastWriteTime(pathFile),
                        CreateTime = File.GetCreationTime(pathFile),
                        Path = pathFile
                    };
                    fileDatas.Add(fileData);
                }
                int maxsimum = 100;
                int.TryParse(AppSettingHelper.GetAppSettingsValue("FileApcsLog"),out maxsimum);
                if (fileDatas.Count >= maxsimum)
                {
                    int countRemove = fileDatas.Count - (maxsimum - 1);
                    var fileOlds = fileDatas.OrderBy(x => x.CreateTime).Take(countRemove).ToList();
                    foreach (FileData fileOld in fileOlds)
                    {
                        File.Delete(fileOld.Path);
                    }
                   
                }
                File.Copy(Path.Combine(pathLog, c_FileTdcBackupName), Path.Combine(pathLog, c_PathFolderBackupTdc, "TDC_before_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv"), true);
                File.Delete(Path.Combine(pathLog, c_FileTdcBackupName));
            }
        }
        catch (Exception ex)
        {
            LogFile.SaveLog("CatchLog",0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "TDC", "Exception", ex.Message.ToString(), "");
        }
      
        TdcLoggerCsvWriter tdcLogger = new TdcLoggerCsvWriter();
        tdcLogger.LogFolder = pathLog;//HttpContext.Current.Server.MapPath(@"~\\Log\\" + pathLog);
        c_TdcService.Logger = tdcLogger;
       
        return tdcLogger;
    }
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

            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCFService", "iReportService", 0, "IRePortCheck", result.ErrorMessage,"");

            return result;
        }

    }
    #endregion
}
