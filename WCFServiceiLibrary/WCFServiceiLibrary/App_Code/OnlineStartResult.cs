using Rohm.Common.Logging;
using System.Runtime.Serialization;

[DataContract()]
public class OnlineStartResult
{
    [DataMember()]
    public bool IsPass { get; internal set; }

    [DataMember()]
    public string Cause { get; internal set; }

    public OnlineStartResult(string cause, string cause2, string functionName, Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "Error", "WCFService", "iLibrary", 0, "", cause, cause2);
    }
    public OnlineStartResult(string functionName, Logger log)
        :this(true,"")
    {
        log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, "", "", "");
    }
    private OnlineStartResult(bool isPass, string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}