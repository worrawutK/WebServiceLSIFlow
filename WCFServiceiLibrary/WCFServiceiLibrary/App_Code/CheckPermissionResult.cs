using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for CheckPermissionResult
/// </summary>
public class CheckPermissionResult
{
    public CheckPermissionResult(Status isPass, MessageType type, string cause, string cause2, string recipe, string errorNo, string subFunction, string functionName, Logger log)
    {
        try
        {
            if (isPass == Status.NotPass)
                log.ConnectionLogger.Write(0, functionName, "Error", "WCFService", "iLibrary", 0, subFunction, type.ToString() + "=>" + errorNo, cause2);
            else if (isPass == Status.Warning)
                log.ConnectionLogger.Write(0, functionName, "Warning", "WCFService", "iLibrary", 0, subFunction, type.ToString() + "=>" + errorNo, cause2);
            else
                log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, subFunction, type.ToString() + "=>" + errorNo, cause2);

        }
        catch (Exception ex)
        {
            Logger logCatch = new Logger("1.0.0", "CatchLog", HttpContext.Current.Server.MapPath(@"~\\Log"));
            logCatch.ConnectionLogger.Write(0, functionName, "Error", "WCFService", "iLibrary", 0, subFunction, type.ToString() + "=>" + errorNo, ex.Message.ToString());
        }

        this.IsPass = isPass;
        this.Cause = cause;
        this.Recipe = recipe;
        this.ErrorNo = errorNo;
        this.FunctionName = functionName;
        this.SubFunction = subFunction;

    }
    [DataMember()]
    public Status IsPass { get; internal set; }

    [DataMember()]
    public string Cause { get; internal set; }

    [DataMember()]
    public string ErrorNo { get; internal set; }

    /// <summary>
    /// จำเป็นต้องเก็บ recipe เพื่อส่งค่าตอน Start
    /// </summary>
    [DataMember()]
    public string Recipe { get; internal set; }

    //[DataMember()]
    //public MessageType Type { get; internal set; }
    [DataMember()]
    public string FunctionName { get; internal set; }
    [DataMember()]
    public string SubFunction { get; internal set; }

    [DataContract()]
    public enum Status
    {
        [EnumMember()]
        Pass,
        [EnumMember()]
        NotPass,
        [EnumMember()]
        Warning
    }
}