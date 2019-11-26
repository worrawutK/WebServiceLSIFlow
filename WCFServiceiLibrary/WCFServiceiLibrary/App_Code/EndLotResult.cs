using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

[DataContract()]
public class EndLotResult
{
    [DataMember()]
    public bool IsPass { get; internal set; }
    [DataMember()]
    public string Cause { get; internal set; }

    [DataMember()]
    public MessageType Type { get; internal set; }
    [DataMember()]
    public string FunctionName { get; internal set; }
    [DataMember()]
    public string SubFunction { get; internal set; }

    [DataMember()]
    public string NextFlow { get; internal set; }
    //[DataMember()]
    //public Status Result { get; internal set; }

    //public EndLotResult(string functionName, Logger log)
    //    :this(true,"")
    //{
    //    log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, "", "", "");
    //}
    //public EndLotResult(Status result, MessageType type, string cause, string cause2, string subFunction, string functionName, Logger log)
    //{
    //    string typeState = "";
    //    switch (result)
    //    {
    //        case Status.Pass:
    //            typeState = "Normal";
    //            this.IsPass = true;
    //            break;
    //        case Status.NotPass:
    //            this.IsPass = false;
    //            typeState = "Error";
    //            break;
    //        case Status.Warning:
    //            this.IsPass = true;
    //            typeState = "Warning";
    //            break;
    //    }

    //    try
    //    {
    //        log.ConnectionLogger.Write(0, functionName, typeState, "WCFService", "iLibrary", 0, "", cause, cause2);
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger logCatch = new Logger("1.0.0", "CatchLog", HttpContext.Current.Server.MapPath(@"~\\Log"));
    //        logCatch.ConnectionLogger.Write(0, functionName, typeState, "WCFService", "iLibrary", 0, subFunction, cause, ex.Message.ToString());

    //    }


    //    this.Result = result;
    //    this.Cause = cause;
    //    this.FunctionName = functionName;
    //    this.Type = type;
    //    this.SubFunction = subFunction;
    //}
    public EndLotResult(bool isPass, MessageType type, string cause, string cause2, string subFunction, string functionName, Logger log,string nextFlow)
    {
        string typeState;
        if (isPass)
        {
            typeState = "Normal";
            //Result = Status.Pass;
        }
        else
        {
            typeState = "Error";
            //Result = Status.NotPass;
        }

        try
        {
            log.ConnectionLogger.Write(0, functionName, typeState, "WCFService", "iLibrary", 0, subFunction, cause, cause2);
        }
        catch (Exception ex)
        {
            Logger logCatch = new Logger("1.0.0", "CatchLog", HttpContext.Current.Server.MapPath(@"~\\Log"));
            logCatch.ConnectionLogger.Write(0, functionName, typeState, "WCFService", "iLibrary", 0, subFunction, cause, ex.Message.ToString());

        }


        this.IsPass = isPass;
        this.Cause = cause;
        this.FunctionName = functionName;
        this.Type = type;
        this.SubFunction = subFunction;
        this.NextFlow = nextFlow;
    }
    public EndLotResult(bool isPass, MessageType type, string cause, string cause2, string subFunction, string functionName, Logger log)
        :this(isPass, type, cause, cause2, subFunction, functionName, log, "")
    {
       
    }
    //[DataContract()]
    //public enum Status
    //{
    //    [EnumMember()]
    //    Pass,
    //    [EnumMember()]
    //    NotPass,
    //    [EnumMember()]
    //    Warning
    //}
}