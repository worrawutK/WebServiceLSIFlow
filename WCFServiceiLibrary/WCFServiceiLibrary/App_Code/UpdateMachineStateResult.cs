using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

[DataContract()]
public class UpdateMachineStateResult
{
    [DataMember()]
    public bool IsPass { get; internal set; }
    [DataMember()]
    public string Cause { get; internal set; }

    public UpdateMachineStateResult()
        :this(true,"")
    {
        
    }
    public UpdateMachineStateResult(string cause,string cause2, string functionName,Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, cause2);
    }
    private UpdateMachineStateResult(bool isPass,string cause)
    {
        this.IsPass = IsPass;
        this.Cause = cause;
    }
}