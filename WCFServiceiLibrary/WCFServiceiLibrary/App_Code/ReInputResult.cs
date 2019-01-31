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
    /// <summary>
    /// Is Pass
    /// </summary>
    public ReinputResult()
        :this(true,"")
    {
       
    }
    /// <summary>
    /// Is Not Pass
    /// </summary>
    /// <param name="cause"></param>
    public ReinputResult(string cause, string cause2, string functionName, Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, cause2);
    }

    private ReinputResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}