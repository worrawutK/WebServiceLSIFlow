using Rohm.Common.Logging;
using System.Runtime.Serialization;

[DataContract()]
public class OnlineEndResult
{
    [DataMember()]
    public bool IsPass { get; set; }
    [DataMember()]
    public string Cause { get; set; }

    public OnlineEndResult(string functionName, Logger log)
        :this(true,"")
    {
        log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, "", "", "");
    }
    public OnlineEndResult(string cause,string cause2, string functionName, Logger log)
        :this(false,cause)
    {
        log.ConnectionLogger.Write(0, functionName, "Error", "WCFService", "iLibrary", 0, "", cause, cause2);
    }
    private OnlineEndResult(bool isPass,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
    }
}