using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using iLibrary;
using Rohm.Common.Logging;
using System.IO;
using System.ServiceModel.Activation;
using Rohm.Apcs.Tdc;
using LotInfo = iLibrary.LotInfo;
using TDCService;
using System.Windows.Forms;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceiLibrary" in code, svc and config file together.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServiceiLibrary : IServiceiLibrary
{
    private ApcsProService c_ApcsProService;
    private TdcService c_TdcService;
    public int count;
    private string c_PahtLogFile;
    private const string c_LogVersion = "1.0.0";
    public ServiceiLibrary()
    {
        c_ApcsProService = new ApcsProService();
        c_TdcService = TdcService.GetInstance();
        //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
        c_PahtLogFile = HttpContext.Current.Server.MapPath(@"~\\Log");
    }
    #region Setup
    
    public SetupLotResult SetupLot(string lotNo, string mcNo, string opNo, string processName, string layerNo)
    {
        //Message.MessageError messageError = new Message.MessageError();
        //messageError.ShowDialog();
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

    private SetupLotResult SetupLotCommon(string lotNo, string mcNo, string opNo, string processName, string layerNo, RunMode tdcRunModeType,string functionName, bool isCheckLicenser = true)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", 
                    "LotNo:" + lotNo + " opNo:" + opNo,"","", "GetLotInfo", functionName, log);

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                //TDC priority
                TdcLotRequestResult requestResult = TdcLotRequest(mcNo, lotNo, (RunModeType)tdcRunModeType,log);
                if (!requestResult.IsPass)
                {
                    return new SetupLotResult(SetupLotResult.Status.NotPass, MessageType.TDC, requestResult.Cause,
                              "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name, "", requestResult.ErrorNo,
                              "CheckLotApcsPro", functionName, log);
                }
                return new SetupLotResult(SetupLotResult.Status.Pass, MessageType.TDC, "", "LotNo:" + lotNo + " opNo:" + opNo, "", "",
                    "CheckLotApcsPro", functionName, log);
            }
            else
            {
                //Apcs Pro priority
                //TDC Move
                TdcMove(mcNo, lotNo, opNo, layerNo, log);
                
                UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
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
                    return new SetupLotResult(SetupLotResult.Status.NotPass,MessageType.ApcsPro, lotUpdateInfo.ErrorMessage,
                        "LotNo:" + lotNo + " opNo:" + opNo, "", lotUpdateInfo.ErrorNo.ToString(), "LotSetup",
                        functionName, log);


                if (warningMessage != "")
                {
                    return new SetupLotResult(SetupLotResult.Status.Warning,MessageType.ApcsPro, warningMessage, 
                        "LotNo:" + lotNo + " opNo:" + opNo, lotUpdateInfo.Recipe1, "","GetUserInfo", functionName, log);
                }
                else
                {
                    return new SetupLotResult(SetupLotResult.Status.Pass,MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, lotUpdateInfo.Recipe1, "", "",
                        functionName, log);
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
    private StartLotResult StartLotCommon(string lotNo, string mcNo, string opNo, string recipe,string functionName,RunMode runMode)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);

            //TDC LotSet
            TdcLotSetResult tdcLotSetResult = TdcLotSet(mcNo, lotNo, opNo, (RunModeType)runMode, dateTimeInfo.Datetime, log);
            if (!tdcLotSetResult.IsPass)
            {
                //
                // TODO: Add constructor logic here
                //
            }

            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new StartLotResult(false, MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo,
                    "GetLotInfo", functionName, log);

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new StartLotResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name,
                    "CheckLotApcsPro", functionName, log);
            }
            else
            {
                UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

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

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

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

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
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

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

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

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

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
        return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, EndMode.Normal, false);
    }
    public EndLotResult EndLotNoCheckLicenser(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        return EndLotCommon(lotNo, mcNo, opNo, good, ng,MethodBase.GetCurrentMethod().Name, EndMode.Normal, false);
    }
    public EndLotResult EndLotCustomMode(string lotNo, string mcNo, string opNo, int good, int ng ,EndMode endMode)
    {
        return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, endMode, false);
    }
    public EndLotResult EndLotCostomModeNoCheckLicenser(string lotNo, string mcNo, string opNo, int good, int ng , EndMode endMode)
    {
        return EndLotCommon(lotNo, mcNo, opNo, good, ng, MethodBase.GetCurrentMethod().Name, endMode, false);
    }
    private EndLotResult EndLotCommon(string lotNo, string mcNo, string opNo, int good, int ng,string functionName, EndMode endMode, bool isCheckLicenser = true)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);

            TdcLotEndResult tdcLotEndResult = TdcLotEnd(mcNo, lotNo, opNo, dateTimeInfo.Datetime, log, good, ng, (EndModeType)endMode);
            if (!tdcLotEndResult.IsPass)
            {
                //
                // TODO: Add constructor logic here
                //
            }

            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new EndLotResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                    " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new EndLotResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng,
                   "CheckLotApcsPro", functionName, log);
            }
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

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

            return new EndLotResult(true, MessageType.ApcsPro, "", "LotNo:" + lotNo + " opNo:" + opNo, "", functionName, log);
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
    public ReinputResult ReinputAndHoldLot(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        return AbnormalLotEnd_BackToThe_BeforeProcess(lotNo, mcNo, opNo, good, ng, true, MethodBase.GetCurrentMethod().Name);
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
    public ReinputResult Reinput(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        return AbnormalLotEnd_BackToThe_BeforeProcess(lotNo, mcNo, opNo, good, ng, false, MethodBase.GetCurrentMethod().Name);
    }

    private ReinputResult AbnormalLotEnd_BackToThe_BeforeProcess(string lotNo, string mcNo, string opNo, int good, int ng, bool holdLot, string functionName)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);

            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new ReinputResult(false,MessageType.ApcsPro, "ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo +
                    " good:" + good + " ng:" + ng, "GetLotInfo", functionName, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new ReinputResult(false, MessageType.ApcsPro, proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, "CheckLotApcsPro",
                    functionName, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

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


            int[] lotArray = new int[1];
            lotArray[0] = lotInfo.Id;
            MachineUpdateInfo machineAlarmInfo  = c_ApcsProService.Update_ErrorHappenRecord(lotArray, machineInfo, userInfo.Id, AlarmNo,
                dateTimeInfo.Datetime, log);
            if (!machineAlarmInfo.IsOk)
                return new MachineAlarmResult(false, MessageType.ApcsPro, machineAlarmInfo.ErrorNo + ":" + machineAlarmInfo.ErrorMessage, 
                    "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(),
                    "Update_ErrorHappenRecord", MethodBase.GetCurrentMethod().Name, log);

            return new MachineAlarmResult(true, MessageType.ApcsPro, "", "AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(), "",
                MethodBase.GetCurrentMethod().Name,log);
        }
        catch (Exception ex)
        {
            return new MachineAlarmResult(false, MessageType.ApcsPro, ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + 
                " AlarmNo:" + AlarmNo + " AlarmState:" + alarm.ToString(), "Exception", MethodBase.GetCurrentMethod().Name, log);
        }
    }
   
    private CheckLotApcsProResult CheckLotApcsPro(string lotNo,string package,Logger log)
    {
        //Check package and Lot Pro
        if (!c_ApcsProService.CheckPackageEnable(package, log))
        {
            return new CheckLotApcsProResult(false,"Package :" + package + " นี้ยังไม่เปิดใช้งานในระบบ Apcs Pro","",MessageType.ApcsPro,"",
                "CheckPackageEnable",MethodBase.GetCurrentMethod().Name,log);
        }
        if (!c_ApcsProService.CheckLotisExist(lotNo, log))
        {
            return new CheckLotApcsProResult(false,"ไม่พบ LotNo :" + lotNo + " ในระบบ Apcs Pro","",MessageType.ApcsPro,"", 
                "CheckLotisExist",MethodBase.GetCurrentMethod().Name,log);
        }
        return new CheckLotApcsProResult(true, "", "", MessageType.ApcsPro, "", "", MethodBase.GetCurrentMethod().Name, log);
    }


    public CheckLotApcsProResult CheckLotApcsProManual(string lotNo, string mcNo, string package)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            CheckLotApcsProResult apcsProResult = CheckLotApcsPro(lotNo, package, log);
            if (!apcsProResult.IsPass)
            {
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCF Service", "iLibrary", 0,
                    "CheckLotApcsPro", apcsProResult.Cause, "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package);
            }
            else
            {
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCF Service", "iLibrary", 0,
                    "CheckLotApcsPro", apcsProResult.Cause, "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package);
            }
            return apcsProResult;
        }
        catch (Exception ex)
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Error", "WCF Service", "iLibrary", 0,
                "Exception", ex.ToString(), "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package);
            return new CheckLotApcsProResult(false,"lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package + 
                " Exception:" + ex.ToString(),"",MessageType.ApcsPro,"", "Exception",MethodBase.GetCurrentMethod().Name,log);
        }
    }

    #region TDC
    private void TdcMove(string mcNo, string lotNo, string opNo, string layerNo, Logger log)
    {        
        if (layerNo == "")
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "Normal", "WCFService", "TDC", 0, "NoMove", "",
                "lotNo:" + lotNo + " opNo:" + opNo + " LayerNo:" + layerNo);
            return;
        }
        //Init TCD Library    
        c_TdcService.Logger = CreateLogTdc(mcNo);
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
    private TdcLotRequestResult TdcLotRequest(string mcNo,string lotNo,RunModeType runMode, Logger log)
    {
        //TDC priority
        c_TdcService.Logger = CreateLogTdc(mcNo);
        TdcLotRequestResponse tdcLotRequest = c_TdcService.LotRequest(mcNo, lotNo, runMode);
        if (tdcLotRequest.HasError)
        {
            using (ApcsWebServiceSoapClient svError = new ApcsWebServiceSoapClient())
            {
                if (svError.LotRptIgnoreError(mcNo, tdcLotRequest.ErrorCode))
                {
                    return new TdcLotRequestResult(tdcLotRequest.ErrorCode, tdcLotRequest.ErrorMessage);
                }
            }
        }
        return new TdcLotRequestResult();       
    }
    private TdcLotSetResult TdcLotSet(string mcNo, string lotNo, string opNo, RunModeType runMode, DateTime dateTime, Logger log)
    {
        //Init TCD Library
        c_TdcService.Logger = CreateLogTdc(mcNo);
        TdcResponse response = c_TdcService.LotSet(mcNo, lotNo, dateTime, opNo, runMode);
        if (response.HasError)
        {
            return new TdcLotSetResult(response.ErrorCode, response.ErrorMessage);
        }
        return new TdcLotSetResult();

    }
    private TdcLotEndResult TdcLotEnd(string mcNo, string lotNo, string opNo, DateTime dateTime, Logger log, int good, int ng , EndModeType endMode)
    {
        //Init TCD Library
        c_TdcService.Logger = CreateLogTdc(mcNo);
        TdcResponse response = c_TdcService.LotEnd(mcNo, lotNo, dateTime, good, ng, endMode, opNo);
        if (response.HasError)
        {
            return new TdcLotEndResult(response.ErrorMessage, response.ErrorCode);
        }
        return new TdcLotEndResult();

    }
    private TdcLoggerCsvWriter CreateLogTdc(string mcNo)
    {
        TdcLoggerCsvWriter tdcLogger = new TdcLoggerCsvWriter
        {
            LogFolder = HttpContext.Current.Server.MapPath(@"~\\Log\\" + mcNo)
        };
        return tdcLogger;
    }
    #endregion
}
