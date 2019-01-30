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
    public MachineOnlineStateResult()
        :this(true,"")
    {

    }
    /// <summary>
    /// Not Pass
    /// </summary>
    /// <param name="cause"></param>
    public MachineOnlineStateResult(string cause,string functionName,Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
    }
    private MachineOnlineStateResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}