using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using Rohm.Common.Logging;
/// <summary>
/// Summary description for MachineOnlineStateResult
/// </summary>
[DataContract()]
public class MachineOnlineStateResult
{

    [DataMember()]
    public bool IsPass { get; internal set; }   

    [DataMember()]
    public string Cause { get; internal set; }

    /// <summary>
    /// IsPass
    /// </summary>
    public MachineOnlineStateResult(string functionName, Logger log)
        :this(true,"")
    {
        log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, "", "", "");
    }
    /// <summary>
    /// Not Pass
    /// </summary>
    /// <param name="cause"></param>
    public MachineOnlineStateResult(string cause, string cause2, string functionName,Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "Error", "WCFService", "iLibrary", 0, "", cause, cause2);
    }
    private MachineOnlineStateResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}