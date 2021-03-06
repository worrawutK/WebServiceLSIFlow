﻿using System;
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

    [DataMember()]
    public MessageType Type { get; internal set; }
    [DataMember()]
    public string FunctionName { get; internal set; }
    [DataMember()]
    public string SubFunction { get; internal set; }
    /// <summary>
    /// IsPass
    /// </summary>
    //public MachineOnlineStateResult(string functionName, Logger log)
    //    :this(true,"")
    //{
    //    log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, "", "", "");
    //}
    /// <summary>
    /// Not Pass
    /// </summary>
    /// <param name="cause"></param>
    public MachineOnlineStateResult(bool isPass,MessageType type ,string cause, string cause2, string subFunction, string functionName,Logger log)
    {
        string typeState;
        if (isPass)
            typeState = "Normal";
        else
            typeState = "Error";
        LogFile.SaveLog(log, 0, functionName, typeState, "WCFService", "iLibrary", subFunction, cause, cause2);
          

        this.IsPass = isPass;
        this.Cause = cause;
        this.Type = type;
        this.FunctionName = functionName;
        this.SubFunction = subFunction;
    }
}