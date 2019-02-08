using Rohm.Common.Logging;
using System.Runtime.Serialization;

[DataContract()]
public class OnlineEndResult
{
    [DataMember()]
    public bool IsPass { get; set; }
    [DataMember()]
    public string Cause { get; set; }

    [DataMember()]
    public MessageType Type { get; internal set; }
    [DataMember()]
    public string FunctionName { get; internal set; }
    [DataMember()]
    public string SubFunction { get; internal set; }
    //public OnlineEndResult(string functionName, Logger log)
    //    :this(true,"")
    //{
    //    log.ConnectionLogger.Write(0, functionName, "Normal", "WCFService", "iLibrary", 0, "", "", "");
    //}
    public OnlineEndResult(bool isPass,MessageType type,string cause,string cause2, string subFunction, string functionName, Logger log)
    {
        string typeState;
        if (isPass)
            typeState = "Normal";
        else
            typeState = "Error";
        log.ConnectionLogger.Write(0, functionName, typeState, "WCFService", "iLibrary", 0, subFunction, cause, cause2);
        this.IsPass = isPass;
        this.Cause = cause;
        this.FunctionName = functionName;
        this.Type = type;
        this.SubFunction = subFunction;
    }
}