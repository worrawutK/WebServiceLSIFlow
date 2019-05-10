using Rohm.Common.Logging;
using System.Runtime.Serialization;
using System.Web;

[DataContract()]
public class OnlineStartResult
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

    public OnlineStartResult(bool isPass,MessageType type, string cause, string cause2,string subFunction, string functionName, Logger log)
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