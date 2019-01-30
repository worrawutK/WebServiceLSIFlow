using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for CancelLot
/// </summary>
[DataContract()]
public class CancelLotResult
{
    [DataMember()]
    public bool IsPass { get; internal set; }
    [DataMember()]
    public string Cause { get; internal set; }

    /// <summary>
    /// Is Pass
    /// </summary>
    public CancelLotResult()
        :this(true,"")
    {
        
    }
    /// <summary>
    /// Is Not Pass
    /// </summary>
    /// <param name="cause"></param>
    /// <param name="functionName"></param>
    /// <param name="log"></param>
    public CancelLotResult(string cause, string functionName,Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
    }
    private CancelLotResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }

}