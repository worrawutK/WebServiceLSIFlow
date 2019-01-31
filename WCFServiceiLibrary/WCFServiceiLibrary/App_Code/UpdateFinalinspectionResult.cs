﻿using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

[DataContract()]
public class UpdateFinalinspectionResult
{
    [DataMember()]
    public bool IsPass { get; internal set; }
    [DataMember()]
    public string Cause { get; internal set; }

    public UpdateFinalinspectionResult()
        :this(true,"")
    {
       
    }
    public UpdateFinalinspectionResult(string cause, string cause2,string functionName,Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, cause2);
    }
    private UpdateFinalinspectionResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}