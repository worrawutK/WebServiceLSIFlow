using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AppSettingHelper
/// </summary>
internal class AppSettingHelper
{
    private AppSettingHelper() { }

    internal static string GetAppSettingsValue(string key)
    {
        string val = ConfigurationManager.AppSettings[key];
        if (string.IsNullOrEmpty(val))
        {
            throw new Exception("Application Setting Key was not found [Key=" + key + "]");
        }
        return val;
    }
    internal static string GetConnectionStringValue(string key)
    {
        string val = ConfigurationManager.ConnectionStrings[key].ConnectionString;
        if (string.IsNullOrEmpty(val))
        {
            throw new Exception("Application Setting Key was not found [Key=" + key + "]");
        }
        return val;
    }
}