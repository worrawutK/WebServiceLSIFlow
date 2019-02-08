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

    [DataMember()]
    public MessageType Type { get; internal set; }
    [DataMember()]
    public string FunctionName { get; set; }
    [DataMember()]
    public string SubFunction { get; set; }
    /// <summary>
    /// Is Pass
    /// </summary>
    //public CheckLotApcsProResult()
    //    :this(true,"")
    //{

    //}
    ///// <summary>
    ///// Not Pass
    ///// </summary>
    ///// <param name="cause"></param>
    ///// <param name="functionName"></param>
    ///// <param name="log"></param>
    //public CheckLotApcsProResult(string cause)
    //    :this(false,cause)
    //{
    //    //log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
    //}
    public CheckLotApcsProResult(bool isPass, string cause, string cause2, MessageType type, string errorNo, string subFunction, string functionName, Logger log)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}