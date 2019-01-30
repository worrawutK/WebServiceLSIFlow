using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

[DataContract()]
public class MachineAlarmResult
{
    [DataMember()]
    public bool IsPass { get; internal set; }
    [DataMember()]
    public string Cause { get; internal set; }

    public MachineAlarmResult()
        :this(true,"")
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public MachineAlarmResult(string cause,string functionName,Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
    }
    private MachineAlarmResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}