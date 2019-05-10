using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

[DataContract()]
public class UpdateFirstinspectionResult
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
    //public UpdateFirstinspectionResult()
    //    :this(true,"")
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
    public UpdateFirstinspectionResult(bool isPass,MessageType type,string cause, string cause2,string subFunction,string functionName,Logger log)
    {
        string typeState;
        if (isPass)
            typeState = "Normal";
        else
            typeState = "Error";
        LogFile.SaveLog(log, 0, functionName, typeState, "WCFService", "iLibrary", subFunction, cause, cause2);
          
        this.IsPass = isPass;
        this.Cause = cause;
        this.FunctionName = functionName;
        this.Type = type;
        this.SubFunction = subFunction;
    }
}