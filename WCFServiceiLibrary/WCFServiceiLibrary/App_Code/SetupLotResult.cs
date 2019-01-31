using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using Rohm.Common.Logging;
[DataContract()]
public class SetupLotResult
{
    [DataMember()]
    public Status IsPass { get; internal set; }

    [DataMember()]
    public string Cause { get; internal set; }
    /// <summary>
    /// จำเป็นต้องเก็บ recipe เพื่อส่งค่าตอน Start
    /// </summary>
    [DataMember()]
    public string Recipe { get; internal set; }

    public SetupLotResult(Status isPass, string cause ,string cause2, string recipe, string functionName, Logger log)
    {
        if (isPass != Status.Pass)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, cause2);
        }
        this.IsPass = isPass;
        this.Cause = cause;
        this.Recipe = recipe;
    }
    public enum Status
    {
        Pass,
        NotPass,
        Warning
    }
}