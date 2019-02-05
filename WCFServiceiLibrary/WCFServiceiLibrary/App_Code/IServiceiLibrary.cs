using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceiLibrary" in both code and config file together.
[ServiceContract]
public interface IServiceiLibrary
{
    [OperationContract]
    MachineOnlineStateResult MachineOnlineState(string mcNo, MachineOnline online);    

    [OperationContract]
    UpdateMachineStateResult UpdateMachineState(string mcNo,MachineProcessingState state);


    /// <summary>
    /// จำเป็นต้องเก็บ recipe จาก SetupLotResult เพื่อใช้ต่อ StartLot
    /// </summary>
    /// <param name="lotNo"></param>
    /// <param name="mcNo"></param>
    /// <param name="opNo"></param>
    /// <param name="processName">เพื่อเช็คสิทธิ์</param>
    /// <param name="layerNo">layerNo=""จะไม่ moveTDC</param>
    /// <returns></returns>
    [OperationContract]
    SetupLotResult SetupLot(string lotNo, string mcNo, string opNo, string processName, string layerNo);
    [OperationContract]
    SetupLotResult SetupLotNoCheckLicenser(string lotNo, string mcNo, string opNo, string processName, string layerNo);
    //CheckPermissionApcsPro(mcNo, userInf, "PL-SetupLot", c_Log)
    //LicenseWarning(userInf)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lotNo"></param>
    /// <param name="mcNo"></param>
    /// <param name="opNo"></param>
    /// <param name="recipe">recipe เอามาจาก Setup Result</param>
    /// <returns></returns>
    [OperationContract]
    StartLotResult StartLot(string lotNo,string mcNo,string opNo,string recipe);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lotNo"></param>
    /// <param name="opNo"></param>
    /// <param name="judge"></param>
    /// <param name="mcNo">เพื่อสร้าง log</param>
    /// <returns></returns>
    [OperationContract]
    UpdateFirstinspectionResult UpdateFirstinspection(string lotNo, string opNo, Judge judge, string mcNo);

    [OperationContract]
    EndLotResult EndLot(string lotNo,string mcNo,string opNo,int good,int ng);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lotNo"></param>
    /// <param name="opNo"></param>
    /// <param name="judge"></param>
    /// <param name="mcNo">เพื่อสร้าง log</param>
    /// <returns></returns>
    [OperationContract]
    UpdateFinalinspectionResult UpdateFinalinspection(string lotNo, string opNo, Judge judge, string mcNo);

    [OperationContract]
    MachineAlarmResult MachineAlarm(string lotNo, string mcNo, string opNo, string AlarmNo, AlarmState alarm);

    [OperationContract]
    CancelLotResult CancelLot(string mcNo, string lotNo, string opNo);

    [OperationContract]
    ReinputResult Reinput(string lotNo, string mcNo, string opNo, int good, int ng);
    [OperationContract]
    ReinputResult ReinputAndHoldLot(string lotNo, string mcNo, string opNo, int good, int ng);

    [OperationContract]
    CheckLotApcsProResult CheckLotApcsProManual(string lotNo, string mcNo, string package);

    [OperationContract]
    OnlineStartResult OnlineStart(string lotNo, string mcNo, string opNo);

    [OperationContract]
    OnlineEndResult OnlineEnd(string lotNo, string mcNo, string opNo, int good, int ng);


}

[DataContract()]
public enum MachineProcessingState
{
    [EnumMember()]
    Initial = 0,
    [EnumMember()]
    Idle = 1,
    [EnumMember()]
    Setup = 2,
    [EnumMember()]
    Ready = 3,
    [EnumMember()]
    Execute = 4,
    [EnumMember()]
    Pause = 5,
    [EnumMember()]
    LotSetUp = 6,
    [EnumMember()]
    Maintenance = 7
}
[DataContract()]
public enum AlarmState
{
    [EnumMember()]
    RESET = 0,
    [EnumMember()]
    SET = 1
}
[DataContract()]
public enum MachineOnline
{
    [EnumMember()]
    Offline = 0,
    [EnumMember()]
    Online = 1,
  
}
[DataContract()]
public enum Judge
{
    [EnumMember()]
    NG = 0,
    [EnumMember()]
    OK = 1
}

// Use a data contract as illustrated in the sample below to add composite types to service operations.
[DataContract]
public class MachineState
{
    //[DataMember]
    //public bool BoolValue
    //{
    //    get { return boolValue; }
    //    set { boolValue = value; }
    //}

    //[DataMember]
    //public string StringValue
    //{
    //    get { return stringValue; }
    //    set { stringValue = value; }
    //}
}