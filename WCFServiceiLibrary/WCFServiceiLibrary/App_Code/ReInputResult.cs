using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for ReInputResult
/// </summary>
[DataContract()]
public class ReinputResult
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
    /// <summary>
    /// Is Pass
    /// </summary>
    //public ReinputResult(string functionName, Logger log)
    //    :this(true,"")
    //{
    //    log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, "", "", "");
    //}
    /// <summary>
    /// Is Not Pass
    /// </summary>
    /// <param name="cause"></param>
    public ReinputResult(bool isPass,MessageType type,string cause, string cause2, string subFunction, string functionName, Logger log)
    {
        string typeState;
        if (isPass)
            typeState = "Normal";
        else
            typeState = "Error";
        log.ConnectionLogger.Write(0, functionName, typeState, "WCFService", "iLibrary", 0, "", cause, cause2);

        this.IsPass = isPass;
        this.Cause = cause;
        this.FunctionName = functionName;
        this.Type = type;
        this.SubFunction = subFunction;
    }
}