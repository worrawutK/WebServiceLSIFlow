using Rohm.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LogFile
/// </summary>
public static class LogFile
{
    public static void SaveLog(Logger log,int errorNo,string functionName,string typeState,
        string from,string to,string subFunction,string cause,string cause2)
    {
        try
        {
            log.ConnectionLogger.Write(0, functionName, typeState, from, to, 0, subFunction, cause, cause2);
        }
        catch (Exception ex)
        {
            Logger logCatch = new Logger("1.0.0", "CatchLog", HttpContext.Current.Server.MapPath(@"~\\Log"));
            logCatch.ConnectionLogger.Write(0, functionName, typeState, from, to, 0, subFunction, cause, ex.Message.ToString());
        }

    }
    /// <summary>
    /// Save New Log
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="errorNo"></param>
    /// <param name="functionName"></param>
    /// <param name="typeState"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="subFunction"></param>
    /// <param name="cause"></param>
    /// <param name="cause2"></param>
    public static void SaveLog(string folderName,int errorNo, string functionName, string typeState,
        string from, string to, string subFunction, string cause, string cause2)
    {
        Logger logCatch = new Logger("1.0.0", folderName, HttpContext.Current.Server.MapPath(@"~\\Log"));
        logCatch.ConnectionLogger.Write(0, functionName, typeState, from, to, 0, subFunction, cause, cause2);
    }
}