using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

[DataContract()]
public class CheckLotApcsProResult
{
    [DataMember()]
    public bool IsPass { get;internal set; }
    [DataMember()]
    public string Cause { get;internal set; }

    /// <summary>
    /// Is Pass
    /// </summary>
    public CheckLotApcsProResult()
        :this(true,"")
    {

    }
    /// <summary>
    /// Not Pass
    /// </summary>
    /// <param name="cause"></param>
    /// <param name="functionName"></param>
    /// <param name="log"></param>
    public CheckLotApcsProResult(string cause,string functionName,Logger log)
        :this(false,cause)
    {
        //log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
    }
    private CheckLotApcsProResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}