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
// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceiLibrary" in code, svc and config file together.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServiceiLibrary : IServiceiLibrary
{
    private ApcsProService c_ApcsProService;
    public int count;
    private string c_PahtLogFile;
    private const string c_LogVersion = "1.0.0";
    public ServiceiLibrary()
    {
        c_ApcsProService = new ApcsProService();

        //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
        c_PahtLogFile = HttpContext.Current.Server.MapPath(@"~\\Log");

    }

    public SetupLotResult SetupLot(string lotNo, string mcNo, string opNo, string processName ,string layerNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new SetupLotResult(SetupLotResult.Status.NotPass, "ไม่พบ lotNo :" + lotNo + " ในระบบ","LotNo:" + lotNo + " opNo:" + opNo , "", MethodBase.GetCurrentMethod().Name, log);

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new SetupLotResult(SetupLotResult.Status.NotPass,proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " package:" + lotInfo.Package.Name, "", MethodBase.GetCurrentMethod().Name, log);
            }

            //TDC Move
            TdcMove(mcNo, lotNo, opNo, layerNo, log);

           
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            string warningMessage = "";
            if (userInfo.License != null)
            {
                if (userInfo.License[0].Is_Warning)
                {
                    warningMessage = "แจ้งเตือน!! รหัส :" + userInfo.Code + Environment.NewLine + "License " + userInfo.License[0].Name + Environment.NewLine + " ใกล้หมดอายุ กรุณาต่ออายุ License ที่ ETG " + Environment.NewLine + "วันหมดอายุ " + userInfo.License[0].ExpireDate.ToString("yyyy/MM/dd");
                }
            }
           
            //Check Permission
            CheckUserPermissionResult permissionResult =  c_ApcsProService.CheckUserPermission(userInfo, "CellController", processName + "-SetupLot", log, dateTimeInfo.Datetime);
            if (!permissionResult.IsPass)
                  return new SetupLotResult(SetupLotResult.Status.NotPass, permissionResult.ErrorNo + ":" + permissionResult.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " processName:" + processName, "", MethodBase.GetCurrentMethod().Name, log);

         
            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new SetupLotResult(SetupLotResult.Status.NotPass, "ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo" + mcNo, "", MethodBase.GetCurrentMethod().Name,log);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotSetup(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                 return new SetupLotResult(SetupLotResult.Status.NotPass,lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name,log); 

            if (warningMessage != "")
            {
                return new SetupLotResult(SetupLotResult.Status.Warning, warningMessage, "", lotUpdateInfo.Recipe1, MethodBase.GetCurrentMethod().Name, log);
            }
            else
            {
                return new SetupLotResult(SetupLotResult.Status.Pass, "", "", lotUpdateInfo.Recipe1, MethodBase.GetCurrentMethod().Name, log);
            }
        }
        catch (Exception ex)
        {
            return new SetupLotResult(SetupLotResult.Status.NotPass,ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo, "", MethodBase.GetCurrentMethod().Name, log);
        }
    }

  
    public StartLotResult StartLot(string lotNo, string mcNo, string opNo,string recipe)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
          
      
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new StartLotResult("ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
         
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new StartLotResult(proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new StartLotResult("ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotStart(lotInfo.Id, machineInfo.Id, userInfo.Id, 0, "", 1, recipe, log,1,-1);
            if (!lotUpdateInfo.IsOk)
                return new StartLotResult(lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);

            return new StartLotResult();
        }
        catch (Exception ex)
        {
            return new StartLotResult(ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
        }
    }

    public UpdateFirstinspectionResult UpdateFirstinspection(string lotNo, string opNo, Judge judge, string mcNo)
    {
        Logger log = new Logger();
        try
        {
            log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
           

            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new UpdateFirstinspectionResult("ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new UpdateFirstinspectionResult(proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);
            LotUpdateInfo lotUpdateInfo = c_ApcsProService.Update_Firstinspection(lotInfo.Id, (int)judge, userInfo.Id, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new UpdateFirstinspectionResult(lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);

            return new UpdateFirstinspectionResult();
        }
        catch (Exception ex)
        {
            return new UpdateFirstinspectionResult(ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
        }
    }

    public UpdateFinalinspectionResult UpdateFinalinspection(string lotNo, string opNo, Judge judge, string mcNo)
    {
        Logger log = new Logger();
        try
        {
            log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
           
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new UpdateFinalinspectionResult("ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new UpdateFinalinspectionResult(proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.Update_Finalinspection(lotInfo.Id, (int)judge, userInfo.Id, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new UpdateFinalinspectionResult(lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);

            return new UpdateFinalinspectionResult();
        }
        catch (Exception ex)
        {
            return new UpdateFinalinspectionResult(ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
        }
    }

    public EndLotResult EndLot(string lotNo, string mcNo, string opNo, int good, int ng)
    {
        Logger log = new Logger();
        try
        {
            log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
          
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new EndLotResult("ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new EndLotResult(proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);
            }
            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new EndLotResult("ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotEnd(lotInfo.Id, machineInfo.Id, userInfo.Id, false, good, ng, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
                return new EndLotResult(lotUpdateInfo.ErrorNo + ":" + lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);

            return new EndLotResult();
        }
        catch (Exception ex)
        {
            return new EndLotResult(ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);
        }
    }

    public CancelLotResult CancelLot(string mcNo,string lotNo,string opNo)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
            
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new CancelLotResult("ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);

            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new CancelLotResult(proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

            MachineInfo machineInfo =  c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new CancelLotResult("ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);

            LotUpdateInfo lotUpdateInfo = c_ApcsProService.LotCancel(lotInfo.Id, machineInfo.Id, userInfo.Id, 1, log);
            if (!lotUpdateInfo.IsOk)
            {
                return new CancelLotResult(lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            }
            return new CancelLotResult(); 
        }
        catch (Exception ex)
        {
            return new CancelLotResult(ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
        }
            
    }
    #region Reinput
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
        return AbnormalLotEnd_BackToThe_BeforeProcess(lotNo, mcNo, opNo, good, ng, true);
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
        return AbnormalLotEnd_BackToThe_BeforeProcess(lotNo, mcNo, opNo, good, ng, false);
    }

    private ReinputResult AbnormalLotEnd_BackToThe_BeforeProcess(string lotNo, string mcNo, string opNo, int good, int ng, bool holdLot)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);
          
            LotInfo lotInfo = c_ApcsProService.GetLotInfo(lotNo, log, dateTimeInfo.Datetime);
            if (lotInfo == null)
                return new ReinputResult("ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);
            //Check package and Lot Pro
            CheckLotApcsProResult proResult = CheckLotApcsPro(lotNo, lotInfo.Package.Name, log);
            if (!proResult.IsPass)
            {
                return new ReinputResult(proResult.Cause, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo, MethodBase.GetCurrentMethod().Name, log);
            }

            UserInfo userInfo = c_ApcsProService.GetUserInfo(opNo, log, dateTimeInfo.Datetime, 30);

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new ReinputResult("ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);
            LotUpdateInfo lotUpdateInfo = c_ApcsProService.AbnormalLotEnd_BackToThe_BeforeProcess(lotInfo.Id, machineInfo.Id, userInfo.Id, holdLot, good, ng, 0, "", 1, dateTimeInfo.Datetime, log);
            if (!lotUpdateInfo.IsOk)
            {
                return new ReinputResult(lotUpdateInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);
            }
            return new ReinputResult();
        }
        catch (Exception ex)
        {
            return new ReinputResult(ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " good:" + good + " ng:" + ng, MethodBase.GetCurrentMethod().Name, log);
        }
    }
    #endregion

    public MachineOnlineStateResult MachineOnlineState(string mcNo, MachineOnline online)
    {
        Logger log = new Logger();
        try
        {
            log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new MachineOnlineStateResult("ไม่พบ MCNo :" + mcNo + " ในระบบ","", MethodBase.GetCurrentMethod().Name, log);

            int result = c_ApcsProService.Update_MachineOnlineState(machineInfo.Id, (int)online, -1, log);
            if (result == 0)
            {
                return new MachineOnlineStateResult("Update_MachineOnlineState อัพเดทไม่ผ่าน","", MethodBase.GetCurrentMethod().Name, log);
            }

            return new MachineOnlineStateResult();
        }
        catch (Exception ex)
        {
            return new MachineOnlineStateResult(ex.Message.ToString(), mcNo, MethodBase.GetCurrentMethod().Name, log);
        }
       // return new MachineOnlineStateResult();
    }
    public UpdateMachineStateResult UpdateMachineState(string mcNo, MachineProcessingState state)
    {
        Logger log = new Logger();
        try
        {
            log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
            DateTimeInfo dateTimeInfo = c_ApcsProService.Get_DateTimeInfo(log);

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new UpdateMachineStateResult("ไม่พบ MCNo :" + mcNo + " ในระบบ","", MethodBase.GetCurrentMethod().Name, log);

            int result = c_ApcsProService.Update_MachineState(machineInfo.Id,(int)state, -1, log);
            if (result ==0)
                return new UpdateMachineStateResult("Update Machine State ไม่ได้","", MethodBase.GetCurrentMethod().Name, log);

            return new UpdateMachineStateResult();
        }
        catch (Exception ex)
        {
            return new UpdateMachineStateResult(ex.Message.ToString(),"", MethodBase.GetCurrentMethod().Name, log);
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
                return new MachineAlarmResult("ไม่พบ lotNo :" + lotNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo, MethodBase.GetCurrentMethod().Name, log);

            MachineInfo machineInfo = c_ApcsProService.GetMachineInfo(mcNo, log, dateTimeInfo.Datetime);
            if (machineInfo == null)
                return new MachineAlarmResult("ไม่พบ MCNo :" + mcNo + " ในระบบ", "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo, MethodBase.GetCurrentMethod().Name, log);


            int[] lotArray = new int[1];
            lotArray[0] = lotInfo.Id;
            MachineUpdateInfo machineAlarmInfo  = c_ApcsProService.Update_ErrorHappenRecord(lotArray, machineInfo, userInfo.Id, AlarmNo,  dateTimeInfo.Datetime, log);
            if (!machineAlarmInfo.IsOk)
                return new MachineAlarmResult(machineAlarmInfo.ErrorNo + ":" + machineAlarmInfo.ErrorMessage, "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo, MethodBase.GetCurrentMethod().Name, log);

            return new MachineAlarmResult();
        }
        catch (Exception ex)
        {
            return new MachineAlarmResult(ex.Message.ToString(), "LotNo:" + lotNo + " opNo:" + opNo + " mcNo:" + mcNo + " AlarmNo:" + AlarmNo, MethodBase.GetCurrentMethod().Name, log);
        }
    }


    private CheckLotApcsProResult CheckLotApcsPro(string lotNo,string package,Logger log)
    {
        //Check package and Lot Pro
        if (!c_ApcsProService.CheckPackageEnable(package, log))
        {
            return new CheckLotApcsProResult("Package :" + package + " นี้ยังไม่เปิดใช้งานในระบบ Apcs Pro", MethodBase.GetCurrentMethod().Name, log);
        }
        if (!c_ApcsProService.CheckLotisExist(lotNo, log))
        {
            return new CheckLotApcsProResult("ไม่พบ LotNo :" + lotNo + " ในระบบ Apcs Pro", MethodBase.GetCurrentMethod().Name, log);
        }
        return new CheckLotApcsProResult();
    }
    public CheckLotApcsProResult CheckLotApcsProManual(string lotNo, string mcNo, string package)
    {
        Logger log = new Logger(c_LogVersion, mcNo, c_PahtLogFile);
        try
        {
            CheckLotApcsProResult apcsProResult = CheckLotApcsPro(lotNo, package, log);
            if (!apcsProResult.IsPass)
            {
                log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "", "WCF Service", "iLibrary", 0, "CheckLotApcsProManual", "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package + " cause:" + apcsProResult.Cause, "");
            }
            return apcsProResult;
        }
        catch (Exception ex)
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "", "WCF Service", "iLibrary", 0, "CheckLotApcsProManual", "lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package + " Exception:" + ex.ToString(),"");
            return new CheckLotApcsProResult("lotNo:" + lotNo + " mcNo:" + mcNo + " package:" + package + " Exception:" + ex.ToString(), "CheckLotApcsProManual", log);
        }
    }

    #region TDC
    private void TdcMove(string mcNo, string lotNo, string opNo, string layerNo, Logger log)
    {
        try
        {
            if (layerNo == "")
            {
                return;
            }
            //Init TCD Library
            TdcService tdcService = TdcService.GetInstance();
            TdcLotRequestResponse res = tdcService.LotRequest(mcNo, lotNo, RunModeType.Normal);
            if (res.HasError)
            {
                if (res.ErrorCode == "06" || res.ErrorCode == "02")
                {
                    tdcService.MoveLot(lotNo, mcNo, opNo, layerNo);
                }
            }
        }
        catch (Exception ex)
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "", "WCFService", "TDC", 0, "", ex.Message.ToString(), "");
        }
    }
    private void TdcLotSet(string mcNo, string lotNo, string opNo, DateTime dateTime, Logger log)
    {
        try
        {
            //Init TCD Library
            TdcService tdcService = TdcService.GetInstance();
            tdcService.LotSet(mcNo, lotNo, dateTime, opNo, RunModeType.Normal);
        }
        catch (Exception ex)
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "", "WCFService", "TDC", 0, "", ex.Message.ToString(), "");
        }
    }
    private void TdcLotEnd(string mcNo, string lotNo, string opNo, DateTime dateTime, Logger log, int good, int ng)
    {
        try
        {
            //Init TCD Library
            TdcService tdcService = TdcService.GetInstance();
            tdcService.LotEnd(mcNo, lotNo, dateTime, good, ng, EndModeType.Normal, opNo);
        }
        catch (Exception ex)
        {
            log.ConnectionLogger.Write(0, MethodBase.GetCurrentMethod().Name, "", "WCFService", "TDC", 0, "", ex.Message.ToString(), "");
        }
    }
    #endregion
}
